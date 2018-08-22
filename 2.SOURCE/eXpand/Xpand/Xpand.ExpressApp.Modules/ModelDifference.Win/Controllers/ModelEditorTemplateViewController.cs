﻿using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Templates.ActionControls;
using DevExpress.ExpressApp.Templates.ActionControls.Binding;
using DevExpress.ExpressApp.Win;
using DevExpress.ExpressApp.Win.Controls;
using DevExpress.ExpressApp.Win.SystemModule;
using DevExpress.ExpressApp.Win.Templates;
using DevExpress.Persistent.Base;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using Fasterflect;
using Xpand.ExpressApp.ModelDifference.DataStore.BaseObjects;
using Xpand.ExpressApp.ModelDifference.Win.PropertyEditors;
using Xpand.Persistent.Base.General;

namespace Xpand.ExpressApp.ModelDifference.Win.Controllers {
    public class ModelEditorTemplateViewController : ViewController<ObjectView> {
        public ModelEditorTemplateViewController() {
            TargetObjectType = typeof(ModelDifferenceObject);
        }

        bool UseOldTemplates => ((WinApplication) Application).UseOldTemplates;

        protected override void OnActivated(){
            base.OnActivated();
            if (!UseOldTemplates&& ((IModelOptionsWin) Application.Model.Options).FormStyle==RibbonFormStyle.Ribbon)
                Frame.GetController<ListViewProcessCurrentObjectController>(controller => controller.ProcessCurrentObjectAction.Execute += ProcessCurrentObjectActionOnExecute);
        }

        protected override void OnDeactivated(){
            base.OnDeactivated();
            Frame.GetController<ListViewProcessCurrentObjectController>(controller => controller.ProcessCurrentObjectAction.Execute -= ProcessCurrentObjectActionOnExecute);
        }

        private void ProcessCurrentObjectActionOnExecute(object sender, SimpleActionExecuteEventArgs e){
            var modelEditorPropertyEditor = ((DetailView) e.ShowViewParameters.CreatedView).GetItems<ModelEditorPropertyEditor>().FirstOrDefault();
            if (modelEditorPropertyEditor != null){
                var controller = modelEditorPropertyEditor.ModelEditorViewModelEditorViewController;
                var mainBarActions = (Dictionary<ActionBase, string>) controller.GetFieldValue("mainBarActions");
                var actionBases = mainBarActions.Select(pair => pair.Key).ToArray().Where(@base => !new [] {"Open","Save","Language"}.Contains(@base.Id)).ToArray();
                foreach (var actionBase in actionBases){
                    actionBase.Category = PredefinedCategory.View.ToString();
                }
                e.ShowViewParameters.Controllers.Add(new ModelEditorActionsController(actionBases));
            }
        }

        class ModelEditorActionsController:ViewController<DetailView>{
            private readonly ActionBase[] _actionBases;
            private readonly ActionBinding[] _actionBindings;

            public ModelEditorActionsController(ActionBase[] actionBases){
                _actionBases = actionBases;
                _actionBindings=new ActionBinding[_actionBases.Length];
                RegisterActions(_actionBases);
            }

            protected override void OnActivated(){
                base.OnActivated();
                Frame.ViewChanged+=FrameOnTemplateChanged;
            }

            protected override void OnDeactivated(){
                base.OnDeactivated();
                Frame.ViewChanged-=FrameOnTemplateChanged;
                foreach (var actionBinding in _actionBindings){
                    actionBinding.Dispose();
                }
            }

            private void FrameOnTemplateChanged(object sender, EventArgs eventArgs){
                for (var index = 0; index < _actionBases.Length; index++){
                    var actionBase = _actionBases[index];
                    var actionBinding = CreateActionBinding(actionBase);
                    _actionBindings[index]=actionBinding;
                }
            }

            private ActionBinding CreateActionBinding(ActionBase action) {
                var site = Frame.Template as IActionControlsSite;
                var actionControl = AddActionControl(action,site);
                return ActionBindingFactory.Instance.Create(action, actionControl);
            }

            private IActionControl AddActionControl(ActionBase action, IActionControlsSite site){
                var container = GetTargetActionContainer(site);
                if (action is SimpleAction)
                    return container.AddSimpleActionControl(action.Id);
                var singleChoiceAction = action as SingleChoiceAction;
                if (singleChoiceAction!=null)
                    return container.AddSingleChoiceActionControl(action.Id,false,singleChoiceAction.ItemType);

                var parametrizedAction = ((ParametrizedAction) action);
                return container.AddParametrizedActionControl(parametrizedAction.Id, parametrizedAction.ValueType);
            }

            private IActionControlContainer GetTargetActionContainer(IActionControlsSite site) {
                if (site == null) return null;
                foreach (IActionControlContainer container in site.ActionContainers) {
                    if (container.ActionCategory == PredefinedCategory.View.ToString()) {
                        return container;
                    }
                }
                return null;
            }
        }
        protected override void OnFrameAssigned(){
            base.OnFrameAssigned();
            if (!UseOldTemplates && ((IModelOptionsWin)Application.Model.Options).FormStyle == RibbonFormStyle.Ribbon) {
                Frame.GetController<ActionControlsSiteController>(controller => controller.CustomBindActionControlToAction += OnCustomBindActionControlToAction);
            }
        }

        private void OnCustomBindActionControlToAction(object sender, CustomBindEventArgs e){
            if (ModelEditorActions.Contains(e.Action)) {
                e.Binding = ActionBindingFactory.Instance.Create(e.Action, e.ActionControl);
            }
        }

        IEnumerable<ActionBase> ModelEditorActions{
            get{
                var controller = Frame.GetController<ModelEditorActionsController>();
                return controller != null ? controller.Actions.AsEnumerable() : Enumerable.Empty<ActionBase>();
            }
        }

        protected override void OnViewControlsCreated() {
            base.OnViewControlsCreated();
            var template = Frame.Template as XtraFormTemplateBase;
            var modelEditorPropertyEditor = View.GetItems<ModelEditorPropertyEditor>().FirstOrDefault();
            if (modelEditorPropertyEditor!=null) {
                if (template != null){
                    SetTemplate(modelEditorPropertyEditor);
                }
                else{
                    var controller = modelEditorPropertyEditor.ModelEditorViewModelEditorViewController;
                    controller.SetControl(modelEditorPropertyEditor.Control);
                    SetTemplateNew(modelEditorPropertyEditor);
                    controller.LoadSettings();
                }
            }
        }

        private void SetTemplateNew(ModelEditorPropertyEditor modelEditorPropertyEditor){
            var barManagerHolder = ((IBarManagerHolder) Frame.Template);
            var modelEditorControl = modelEditorPropertyEditor.Control;
            modelEditorControl.PopupContainer.Manager = barManagerHolder.BarManager;
            modelEditorControl.PopupContainer.BeginUpdate();
            foreach (ActionBase action in (IEnumerable<ActionBase>) modelEditorPropertyEditor.ModelEditorViewModelEditorViewController.GetFieldValue("popupMenuActions")) {
                modelEditorControl.PopupContainer.Register(action);
            }
            modelEditorControl.PopupContainer.EndUpdate();
            modelEditorControl.PopupMenu.CreateActionItems(barManagerHolder, modelEditorControl, new IActionContainer[] { modelEditorControl.PopupContainer });
        }

        private void SetTemplate(ModelEditorPropertyEditor modelEditorPropertyEditor) {
            var controller = modelEditorPropertyEditor.ModelEditorViewModelEditorViewController;
            var caption = Guid.NewGuid().ToString();
            controller.SaveAction.Caption = caption;
            controller.SetControl(modelEditorPropertyEditor.Control);
            controller.SetTemplate(Frame.Template);
            var barManagerHolder = ((IBarManagerHolder)Frame.Template);
            var barButtonItems = barManagerHolder.BarManager.Items.OfType<BarButtonItem>().ToArray();
            barButtonItems.First(item => item.Caption.IndexOf(caption, StringComparison.Ordinal) > -1).Visibility = BarItemVisibility.Never;

        }
    }
}
