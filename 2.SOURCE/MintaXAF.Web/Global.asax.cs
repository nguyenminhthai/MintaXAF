﻿using System;
using System.Configuration;
using System.Web.Configuration;
using System.Web;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Web;
using DevExpress.Web;
using System.Collections.Generic;

namespace MintaXAF.Web {
    public class Global : System.Web.HttpApplication {
        public Global() {
            InitializeComponent();
        }
        protected void Application_Start(Object sender, EventArgs e) {
            SecurityAdapterHelper.Enable();
            ASPxWebControl.CallbackError += new EventHandler(Application_Error);
#if EASYTEST
            DevExpress.ExpressApp.Web.TestScripts.TestScriptsManager.EasyTestEnabled = true;
#endif
        }
        protected void Session_Start(Object sender, EventArgs e) {
            Tracing.Initialize();
            WebApplication.SetInstance(Session, new MintaXAFAspNetApplication());
            DevExpress.ExpressApp.Web.Templates.DefaultVerticalTemplateContentNew.ClearSizeLimit();
            DeviceCategory deviceCategory = DeviceDetector.Instance.GetDeviceCategory();

            #region Hiển thị giao diện theo thiết bị client            
            if (deviceCategory == DeviceCategory.Mobile || deviceCategory == DeviceCategory.Tablet)
            {   
                WebApplication.Instance.SwitchToNewStyle();
            }
            #endregion

            #region Cấu hình ngôn ngữ và định dạng mặc định
            WebApplication.Instance.CustomizeLanguage += new EventHandler<CustomizeLanguageEventArgs>(Instance_CustomizeLanguage);
            WebApplication.Instance.CustomizeFormattingCulture += new EventHandler<CustomizeFormattingCultureEventArgs>(Instance_CustomizeFormattingCulture);
            #endregion

            if (ConfigurationManager.ConnectionStrings["ConnectionString"] != null)
            {
                WebApplication.Instance.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            }            
#if EASYTEST
            if(ConfigurationManager.ConnectionStrings["EasyTestConnectionString"] != null) {
                WebApplication.Instance.ConnectionString = ConfigurationManager.ConnectionStrings["EasyTestConnectionString"].ConnectionString;
            }
#endif
#if DEBUG
            if(System.Diagnostics.Debugger.IsAttached && WebApplication.Instance.CheckCompatibilityType == CheckCompatibilityType.DatabaseSchema) {
                WebApplication.Instance.DatabaseUpdateMode = DatabaseUpdateMode.UpdateDatabaseAlways;
            }
#endif
            WebApplication.Instance.Setup();
            WebApplication.Instance.Start();
        }

        private void Instance_CustomizeFormattingCulture(object sender, CustomizeFormattingCultureEventArgs e)
        {
            e.FormattingCulture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
        }

        private void Instance_CustomizeLanguage(object sender, CustomizeLanguageEventArgs e)
        {
            var currentLanguage = e.LanguageName;
            List<string> availableLanguages = new List<string>();
            string languagesValue = ConfigurationManager.AppSettings["Languages"];
            if (languagesValue != null)
            {
                availableLanguages.AddRange(languagesValue.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
            }
            if (!availableLanguages.Contains(currentLanguage))
            {
                e.LanguageName = "vi";
            }
        }

        protected void Application_BeginRequest(Object sender, EventArgs e) {
        }
        protected void Application_EndRequest(Object sender, EventArgs e) {
        }
        protected void Application_AuthenticateRequest(Object sender, EventArgs e) {
        }
        protected void Application_Error(Object sender, EventArgs e) {
            ErrorHandling.Instance.ProcessApplicationError();
        }
        protected void Session_End(Object sender, EventArgs e) {
            WebApplication.LogOff(Session);
            WebApplication.DisposeInstance(Session);
        }
        protected void Application_End(Object sender, EventArgs e) {
        }
        #region Web Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
        }
        #endregion
    }
}
