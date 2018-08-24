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

namespace MintaXAF.Win
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class SwitchLanguageLogonWindowController : MintaXAF.Module.Controllers.SwitchLanguageLogonController
    {
        public SwitchLanguageLogonWindowController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void SwitchLanguageDelegate(object sender, SimpleActionExecuteEventArgs e)
        {   
            View logonView = null;
            if (Program.winApplication.LogonWindow != null)
            {
                logonView = Program.winApplication.LogonWindow.View;
                Program.winApplication.LogonWindow.ViewChanging += new EventHandler<ViewChangingEventArgs>(LogonWindow_ViewChanging);
                Program.winApplication.LogonWindow.SetView(null);
            }
            base.SwitchLanguageDelegate(sender, e);
            if (Program.winApplication.LogonWindow != null)
            {
                logonView.LoadModel();
                Program.winApplication.LogonWindow.SetView(logonView);
            }
        }
        void LogonWindow_ViewChanging(object sender, ViewChangingEventArgs e)
        {
            Program.winApplication.LogonWindow.ViewChanging -= new EventHandler<ViewChangingEventArgs>(LogonWindow_ViewChanging);
            e.DisposeOldView = false;
        }
    }
}
