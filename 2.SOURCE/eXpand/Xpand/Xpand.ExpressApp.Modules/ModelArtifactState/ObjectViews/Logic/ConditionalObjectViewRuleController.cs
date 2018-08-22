﻿using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.SystemModule;
using Xpand.ExpressApp.Logic;
using Xpand.Persistent.Base.General;
using Xpand.Persistent.Base.Logic;
using Xpand.Persistent.Base.ModelArtifact;

namespace Xpand.ExpressApp.ModelArtifactState.ObjectViews.Logic {
    public class ConditionalObjectViewRuleController : ViewController {

        protected override void OnFrameAssigned() {
            base.OnFrameAssigned();
            Frame.Disposing+=FrameOnDisposing;
            Frame.GetController<LogicRuleViewController>(controller => controller.LogicRuleExecutor.LogicRuleExecute += LogicRuleViewControllerOnLogicRuleExecute);
        }

        void FrameOnDisposing(object sender, EventArgs eventArgs) {
            Frame.Disposing-=FrameOnDisposing;
            Frame.GetController<LogicRuleViewController>(controller => controller.LogicRuleExecutor.LogicRuleExecute -= LogicRuleViewControllerOnLogicRuleExecute);
        }


        public class InfoController:ViewController {
            public InfoController(bool active) {
                Active["context"] = active;
            }

            public InfoController() {
                Active["context"] = false;
            }

            public IModelView Model { get; set; }
        }

        void LogicRuleViewControllerOnLogicRuleExecute(object sender, LogicRuleExecuteEventArgs e) {
            LogicRuleInfo info = e.LogicRuleInfo;
            if (info.InvertCustomization)
                return;
            var objectViewRule = info.Rule as IObjectViewRule;
            if (objectViewRule!=null) {
                ExecutionContext executionContext = e.ExecutionContext;
                switch (executionContext) {
                    case ExecutionContext.None:
                        if (info.Active) {
                            ProcessActions(info, objectViewRule);
                        }
                        break;
                    case ExecutionContext.CustomProcessSelectedItem:
                        if (info.Active) {
                            CustomProcessSelectedItem(info, objectViewRule);
                        }
                        break;
                    case ExecutionContext.CurrentObjectChanged:
                        if (View.Model.AsObjectView is IModelDetailView && objectViewRule.ObjectView is IModelDetailView&&View.ObjectTypeInfo!=null) {
                            var modelView = info.Active ? objectViewRule.ObjectView : Application.Model.BOModel.GetClass(View.ObjectTypeInfo.Type).DefaultDetailView;
                            if (modelView!=null){
                                var shortcut = new ViewShortcut(modelView.Id,View.ObjectTypeInfo.KeyMember.GetValue(View.CurrentObject).ToString());
                                Frame.SetView(Application.ProcessShortcut(shortcut),View.Tag as Frame);
                            }
                        }
                        break;
                }

            }
        }
        
        void ProcessActions(LogicRuleInfo info, IObjectViewRule objectViewRule) {
            var createdView = ((ActionBaseEventArgs)info.EventArgs).ShowViewParameters.CreatedView;
            if (createdView.Model.GetType() == objectViewRule.ObjectView.GetType())
                createdView.SetModel(objectViewRule.ObjectView);
        }

        void CustomProcessSelectedItem(LogicRuleInfo info, IObjectViewRule objectViewRule) {
            var customProcessListViewSelectedItemEventArgs = ((CustomProcessListViewSelectedItemEventArgs) info.EventArgs);
            var showViewParameters = customProcessListViewSelectedItemEventArgs.InnerArgs.ShowViewParameters;
            showViewParameters.CreatedView = Application.CreateView(objectViewRule.ObjectView);
            if (showViewParameters.CreatedView is DetailView)
                showViewParameters.CreatedView.CurrentObject = showViewParameters.CreatedView.ObjectSpace.GetObject(View.CurrentObject);
            customProcessListViewSelectedItemEventArgs.Handled = true;
        }
    }
}