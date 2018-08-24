using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;

namespace MintaXAF.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class SwitchLanguageLogonController : ViewController
    {
        private SimpleAction saSwitchLanguageLogon;

        public SwitchLanguageLogonController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.               
            saSwitchLanguageLogon = new SimpleAction(this, "SwitchLanguageLogon", "SwitchLanguageLogon");
            saSwitchLanguageLogon.PaintStyle = ActionItemPaintStyle.Image;
            saSwitchLanguageLogon.Execute += SwitchLanguageDelegate;
        }

        protected virtual void SwitchLanguageDelegate(object sender, SimpleActionExecuteEventArgs e)
        {   
            Application.SetLanguage(((IModelApplicationServices)(Application.Model)).CurrentAspect == "vi"?"en":"vi");            
            saSwitchLanguageLogon.ImageName = ((IModelApplicationServices)(Application.Model)).CurrentAspect == "vi" ? "Flag_eng" : "Flag_vn";
            // TODO: Nếu thay đổi LogonParameter phải chú ý ở đây!
            IObjectSpace ObjectSpace = ObjectSpaceInMemory.CreateNew();
            DevExpress.ExpressApp.Security.AuthenticationStandardLogonParameters logonActionParameters = Activator.CreateInstance<DevExpress.ExpressApp.Security.AuthenticationStandardLogonParameters>();
            DetailView dv = Application.CreateDetailView(ObjectSpace, logonActionParameters,true);
            dv.ViewEditMode = ViewEditMode.Edit;
            Frame.SetView(dv);
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            saSwitchLanguageLogon.ImageName = Application.CurrentAspectProvider.CurrentAspect == "vi-VN" ? "Flag_eng" : "Flag_vn";
        }       
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
       
    }
}
