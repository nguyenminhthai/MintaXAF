using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using System.Collections.Generic;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Xpo;
using MintaXAF.Module.Extension.FriendlyUrl;
using MintaXAF.Module.Controllers;

namespace MintaXAF.Module
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppModuleBasetopic.aspx.
    public sealed partial class MintaXAFModule : ModuleBase {
        public MintaXAFModule() {
            InitializeComponent();
			BaseObject.OidInitializationMode = OidInitializationMode.AfterConstruction;
        }
        public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB) {
            ModuleUpdater updater = new DatabaseUpdate.Updater(objectSpace, versionFromDB);
            return new ModuleUpdater[] { updater };
        }
        public override void Setup(XafApplication application) {
            base.Setup(application);
            // Manage various aspects of the application UI and behavior at the module level.
            application.CreateCustomLogonWindowControllers += Application_CreateCustomLogonWindowControllers;
        }
        private void Application_CreateCustomLogonWindowControllers(object sender, CreateCustomLogonWindowControllersEventArgs e)
        {
            e.Controllers.Add(((XafApplication)sender).CreateController<SwitchLanguageLogonController>());
        }

        public override void CustomizeTypesInfo(ITypesInfo typesInfo) {
            base.CustomizeTypesInfo(typesInfo);
            CalculatedPersistentAliasHelper.CustomizeTypesInfo(typesInfo);
        }

        public override void ExtendModelInterfaces(DevExpress.ExpressApp.Model.ModelInterfaceExtenders extenders)
        {
            base.ExtendModelInterfaces(extenders);
            #region Friendly Url
            extenders.Add<IModelView, IModelViewFriendlyUrl>();
            extenders.Add<IModelDetailView, IModelDetailViewFriendlyUrl>();
            extenders.Add<IModelOptions, IModelOptionsFriendlyUrl>();
            #endregion

        }
    }
}
