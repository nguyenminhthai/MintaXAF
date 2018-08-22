﻿using System.ComponentModel;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using Xpand.Persistent.Base.General;
using Xpand.Persistent.Base.General.Model;

namespace Xpand.ExpressApp.SystemModule.Actions {
    [ModelAbstractClass]
    public interface IModelActionState:IModelAction {
        [DefaultValue(true)]
        [Category(AttributeCategoryNameProvider.Xpand)]
        bool Active { get; set; }
    }
    public class GlobalActionStateController:Controller,IModelExtender {
        public const string ModelActiveAttribute = "ModelActiveAttribute";
        protected override void OnFrameAssigned(){
            base.OnFrameAssigned();
            foreach (var variable in Application.Modules.SelectMany(m=>m.GetControllerTypes())){
                Application.TypesInfo.RegisterEntity(variable);
            }
            
            var modelActionStates = Application.Model.ActionDesign.Actions.Cast<IModelActionState>();
            foreach (var modelActionState in modelActionStates.Where(state => !state.Active)) {
                var actionBase = Frame.Actions().FirstOrDefault(a => a.Id==modelActionState.Id);
                if (actionBase != null){
                    actionBase.Active.BeginUpdate();
                    actionBase.Active[ModelActiveAttribute] = false;
                    actionBase.Active.EndUpdate();
                }
            }
        }

        public void ExtendModelInterfaces(ModelInterfaceExtenders extenders) {
            extenders.Add<IModelAction,IModelActionState>();
        }
    }
}
