using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;

namespace MintaXAF.Module.Web.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class SwitchLanguageLogonController : ViewController
    {
        public SwitchLanguageLogonController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
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

        private void sLanguageEnglish_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            SwitchCaption();
        }

        private void sLanguageVietnamese_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            SwitchCaption();
        }

        private void SwitchLanguageLogonController_ViewControlsCreated(object sender, EventArgs e)
        {
            SwitchCaption();
        }
        private void SwitchCaption()
        {
            if (Application.CurrentAspectProvider.CurrentAspect == "vi-VN")
            {
                sLanguageVietnamese.Caption = "Tiếng việt";
                sLanguageEnglish.Caption = "Tiếng anh";
            }
            else
            {

                sLanguageVietnamese.Caption = "Vietnamese";
                sLanguageEnglish.Caption = "English";
            }
        }
    }
}
