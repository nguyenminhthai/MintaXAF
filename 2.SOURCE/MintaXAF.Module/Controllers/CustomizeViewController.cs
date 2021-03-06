﻿using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;

namespace MintaXAF.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class CustomizeViewController : ViewController
    {
        private readonly string Key = "ViewPermission";
        #region Controller System
        RecordsNavigationController recordsNavigationController;
        ViewNavigationController viewNavigationController;
        ResetViewSettingsController resetViewSettingsController;        
        //ResetPasswordController resetPasswordController;        
        #endregion
        public CustomizeViewController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            // Ẩn một số controller của XAF
            recordsNavigationController = Frame.GetController<RecordsNavigationController>();
            if (recordsNavigationController != null)
                recordsNavigationController.Active[Key] = false;
            viewNavigationController = Frame.GetController<ViewNavigationController>();
            if (viewNavigationController != null)
                viewNavigationController.Active[Key] = false;
            resetViewSettingsController = Frame.GetController<ResetViewSettingsController>();
            if (resetViewSettingsController != null)
                resetViewSettingsController.Active[Key] = false;
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
