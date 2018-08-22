using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.Data.Filtering;
using DevExpress.Data.Filtering.Helpers;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.TreeListEditors.Win;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base.General;
using DevExpress.Xpo;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Filtering;
using Xpand.ExpressApp.Win.Editors;
using Xpand.ExpressApp.Win.SystemModule;
using FilterControl = DevExpress.XtraEditors.FilterControl;
using LookupEdit = DevExpress.ExpressApp.Win.Editors.LookupEdit;

namespace Xpand.ExpressApp.TreeListEditors.Win.Controllers {
    public partial class RecursiveFilteringViewController : ViewController {
        private static bool _recursive;
        private static bool _lookUpQueryPopUp;
        public RecursiveFilteringViewController() {
            InitializeComponent();
            RegisterActions(components);
            TargetViewType = ViewType.ListView;
        }

        protected override void OnActivated() {
            RecursiveFilterPopLookUpTreeSelectionSimpleAction.Active["is " + typeof(ITreeNode).Name] = typeof(ITreeNode).IsAssignableFrom(View.ObjectTypeInfo.Type);
            if (!_lookUpQueryPopUp)
                RecursiveFilterPopLookUpTreeSelectionSimpleAction.Active["Default"] = false;
            _lookUpQueryPopUp = false;

            var filterControlListViewController = Frame.GetController<FilterControlListViewController>();
            if (filterControlListViewController != null)
                filterControlListViewController.CustomAssignFilterControlSourceControl += CustomAssignFilterControlSourceControlListViewControllerOnCustomAssignFilterControlSourceControl;
        }

        private void CustomAssignFilterControlSourceControlListViewControllerOnCustomAssignFilterControlSourceControl(object sender, EventArgs args) {
            var filterControlListViewController = Frame.GetController<FilterControlListViewController>();
            UpdateActionState(filterControlListViewController.FilterControl);
            var categorizedListEditor = ((ListView)View).Editor as CategorizedListEditor;
            if (categorizedListEditor != null) filterControlListViewController.FilterControl.SourceControl = (categorizedListEditor).Grid;
            filterControlListViewController.FilterControl.FilterChanged += FilterOnFilterChanged;

        }

        private List<ITreeNode> GetAllChildTreeNodes(InOperator inOperator, string propertyName) {
            var allTreeNodes = new List<ITreeNode>();
            XPCollection treeNodes = GetTreeNodes(propertyName, inOperator);
            if (treeNodes != null)
                allTreeNodes = treeNodes.Cast<ITreeNode>().ToList().GetAllTreeNodes();
            return allTreeNodes.Distinct().ToList();
        }
        private XPCollection GetTreeNodes(string propertyName, InOperator inOperator) {

            Type memberType = View.ObjectTypeInfo.FindMember(propertyName).MemberType;
            string keyName = XafTypesInfo.Instance.FindTypeInfo(memberType).KeyMember.Name;
            InOperator clone = inOperator.Clone();
            ((OperandProperty)clone.LeftOperand).PropertyName = keyName;
            return new XPCollection(((XPObjectSpace)ObjectSpace).Session, memberType, clone);
        }

        private void FilterOnFilterChanged(object sender, FilterChangedEventArgs args) {
            var clauseNode = (args.CurrentNode) as ClauseNode;
            if (clauseNode != null && _recursive && clauseNode.Operation == ClauseType.AnyOf && args.Action == FilterChangedAction.ValueChanged) {
                _recursive = false;
                string propertyName = ((ClauseNode)args.CurrentNode).FirstOperand.PropertyName;
                var inOperator = (InOperator)FilterControlHelpers.ToCriteria(args.CurrentNode);
                List<ITreeNode> allChildTreeNodes = GetAllChildTreeNodes(inOperator, propertyName);
                if (allChildTreeNodes.Count > 0)
                    ((FilterControl)sender).FilterCriteria = new InOperator(propertyName, allChildTreeNodes);
                RecursiveFilterPopLookUpTreeSelectionSimpleAction.Active[ClauseType.AnyOf.ToString()] = true;
            }
        }

        private void UpdateActionState(XpandFilterControl xpandFilter) {
            xpandFilter.EditorActivated +=
                edit => {
                    var lookupEdit = edit as LookupEdit;
                    if (lookupEdit != null)
                        (lookupEdit).QueryPopUp +=
                            (o, args) => {
                                RecursiveFilterPopLookUpTreeSelectionSimpleAction.Active["Default"] =
                                    true;
                                _lookUpQueryPopUp = true;
                            };
                };
        }

        private void RecursiveFilterPopLookUpTreeSelectionSimpleAction_Execute(object sender, DevExpress.ExpressApp.Actions.SimpleActionExecuteEventArgs e) {
            _recursive = true;
            Frame.GetController<ListViewProcessCurrentObjectController>().ProcessCurrentObjectAction.DoExecute();
        }

    }
}
