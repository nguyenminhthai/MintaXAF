using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.XtraGrid;
using Fasterflect;
using Xpand.Persistent.Base.General;
using ListView = DevExpress.ExpressApp.ListView;

namespace Xpand.ExpressApp.Win.ListEditors.GridListEditors.GridView.MasterDetail {
    public abstract class MasterDetailActionsController : ViewController<ListView> {
        readonly Dictionary<string, BoolList> _enabledBoolLists = new Dictionary<string, BoolList>();
        readonly Dictionary<string, BoolList> _activeBoolLists = new Dictionary<string, BoolList>();
        bool _disposing;
        private bool _isSyncState;
        private readonly Dictionary<string, object> _valueLists=new Dictionary<string, object>();
        private IEnumerable<ActionBase> _actionBases;

        protected override void OnFrameAssigned() {
            base.OnFrameAssigned();
            Frame.Disposing += FrameOnDisposing;
        }

        void FrameOnDisposing(object sender, EventArgs eventArgs) {
            Frame.Disposing -= FrameOnDisposing;
            _disposing = true;
        }

        void SubscribeToActionStateResultChange(ActionBase actionBase) {
            if (!IsExcluded(actionBase)) {
                actionBase.Enabled.ResultValueChanged += (sender, args) => {
                    if (CanClone())
                        CloneBoolList(actionBase.Id, actionBase.Enabled, _enabledBoolLists);
                };
                actionBase.Active.ResultValueChanged += (sender, args) => {
                    if (CanClone())
                        CloneBoolList(actionBase.Id, actionBase.Active, _activeBoolLists);
                };
            }
        }

        bool CanClone() {
            return !_isSyncState && !_disposing && GridListEditor?.Grid?.FocusedView != null && !HasDetailFrame();
        }

        private bool HasDetailFrame() {
            var masterDetailColumnView = GridListEditor.Grid.FocusedView as IMasterDetailColumnView;
            return masterDetailColumnView != null && masterDetailColumnView.Window == null;
        }

        protected virtual IEnumerable<ActionBase> GetActions(Frame frame) {
            return frame.Actions();
        }

        protected override void OnDeactivated() {
            base.OnDeactivated();
            if (_actionBases != null)
                foreach (var actionBase in _actionBases){
                    actionBase.Executing -= ActionOnExecuting;
                }
            _activeBoolLists.Clear();
            _enabledBoolLists.Clear();
            _valueLists.Clear();
        }

        protected override void OnViewControlsCreated() {
            base.OnViewControlsCreated();
            if (GridListEditor?.ColumnView is IMasterDetailColumnView) {
                var gridView = ((IMasterDetailColumnView)GridListEditor.ColumnView);
                if (gridView.MasterFrame == null && HasRules && SynchronizeActions()) {
                    _actionBases = GetActions(Frame);
                    foreach (var action in _actionBases) {
                        SubscribeToActionStateResultChange(action);
                        action.Executing += ActionOnExecuting;
                    }
                    if (gridView.MasterFrame == null) {
                        CloneActionState(Frame, _activeBoolLists, _enabledBoolLists,_valueLists);
                        gridView.GridControl.FocusedViewChanged += OnFocusedViewChanged;
                    }
                }
            }

        }

        private void ActionOnExecuting(object o, CancelEventArgs cancelEventArgs){
            PushExecutionToNestedFrame((ActionBase) o, () => cancelEventArgs.Cancel = true);
        }

        protected virtual bool SynchronizeActions() {
            return Frame.GetController<MasterDetailViewController>().FilterRules(View.CurrentObject, Frame).Any();
        }

        void OnFocusedViewChanged(object sender, ViewFocusEventArgs e) {
            if (GridListEditor != null&& GridListEditor.Grid.FocusedView!=null) {
                Frame frame = Frame;
                var activeBoolLists = new Dictionary<string, BoolList>();
                var enableBoolLists = new Dictionary<string, BoolList>();
                var valueLists =new Dictionary<string,object>();
                if (GridListEditor.Grid.MainView != e.View) {
                    frame = ((IMasterDetailColumnView)GridListEditor.Grid.FocusedView).Window;
                }
                CloneActionState(frame, activeBoolLists, enableBoolLists,valueLists);
                if (GridListEditor.Grid.MainView == e.View) {
                    activeBoolLists = _activeBoolLists;
                    enableBoolLists = _enabledBoolLists;
                    valueLists = _valueLists;
                }
                foreach (var action in GetActions(Frame)) {
                    var singleChoiceAction = action as SingleChoiceAction;
                    if (singleChoiceAction != null && valueLists.ContainsKey(singleChoiceAction.Id))
                        singleChoiceAction.SelectedItem = (ChoiceActionItem) valueLists[singleChoiceAction.Id];
                    SyncStates(action.Id, action.Active, activeBoolLists);
                    SyncStates(action.Id, action.Enabled, enableBoolLists);
                    _isSyncState = true;
                    action.CallMethod("UpdateState");
                    _isSyncState = false;
                }
            }
        }

        void SyncStates(string id, BoolList boolList, Dictionary<string, BoolList> boolLists) {
            if (boolLists.ContainsKey(id)) {
                var list = boolLists[id];
                _isSyncState = true;
                boolList.BeginUpdate();
                if (list.GetKeys().FirstOrDefault() != null) {
                    boolList.Clear();
                    foreach (var key in list.GetKeys()) {
                        boolList.SetItemValue(key, list[key]);
                    }
                }
                boolList.EndUpdate();
                _isSyncState = false;
            }
        }

        void CloneActionState(Frame frame, Dictionary<string, BoolList> active, Dictionary<string, BoolList> enable, Dictionary<string, object> valueLists) {
            foreach (var action in GetActions(frame)) {
                var singleChoiceAction = action as SingleChoiceAction;
                if (singleChoiceAction != null)
                    valueLists.Add(action.Id, singleChoiceAction.SelectedItem);
                CloneBoolList(action.Id, action.Active, active);
                CloneBoolList(action.Id, action.Enabled, enable);
            }
        }

        void CloneBoolList(string id, BoolList boolList, Dictionary<string, BoolList> boolLists) {
            boolLists[id] = new BoolList();
            boolLists[id].BeginUpdate();
            foreach (var key in boolList.GetKeys()) {
                boolLists[id].SetItemValue(key, boolList[key]);
            }
            boolLists[id].EndUpdate();
        }

        public virtual bool HasRules {
            get {
                if (GridListEditor == null)
                    return false;
                var masterDetailViewController = Frame.GetController<MasterDetailViewController>();
                return masterDetailViewController != null && masterDetailViewController.IsMasterDetail();
            }
        }

        WinColumnsListEditor GridListEditor => View?.Editor as WinColumnsListEditor;

        void PushExecutionToNestedFrame(ActionBase action, Action cancelAction) {
            var xpandXafGridView = (IMasterDetailColumnView) GridListEditor?.Grid?.FocusedView;
            if (xpandXafGridView?.MasterFrame != null) {
                var controller = Controller(action.Controller, xpandXafGridView);
                if (controller != action.Controller) {
                    cancelAction.Invoke();
                    ActionBase a = controller.Actions[action.Id];
                    a.CloneParameter(action);
                    a.DoExecute();
                }
            }
        }

        Controller Controller(Controller sender, IMasterDetailColumnView xpandXafGridView) {
            return xpandXafGridView.Window.Controllers.Cast<Controller>().FirstOrDefault(controller1 => sender.GetType() == controller1.GetType());
        }

        protected virtual bool IsExcluded(ActionBase action) {
            return false;
        }
    }
}