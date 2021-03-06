﻿using System;
using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Win;
using System.Collections.Generic;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Xpo;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.ClientServer;

namespace MintaXAF.Win {
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/DevExpressExpressAppWinWinApplicationMembersTopicAll.aspx
    public partial class MintaXAFWindowsFormsApplication : WinApplication {
        #region Default XAF configuration options (https://www.devexpress.com/kb=T501418)
        static MintaXAFWindowsFormsApplication() {
            DevExpress.Persistent.Base.PasswordCryptographer.EnableRfc2898 = true;
            DevExpress.Persistent.Base.PasswordCryptographer.SupportLegacySha512 = false;
        }
        private void InitializeDefaults() {
            LinkNewObjectToParentImmediately = false;
            OptimizedControllersCreation = true;
            UseLightStyle = true;
        }
        #endregion
        public MintaXAFWindowsFormsApplication() {
            InitializeComponent();
			InitializeDefaults();
        }
        protected override void CreateDefaultObjectSpaceProvider(CreateCustomObjectSpaceProviderEventArgs args) {
            args.ObjectSpaceProviders.Add(new SecuredObjectSpaceProvider((SecurityStrategyComplex)Security, XPObjectSpaceProvider.GetDataStoreProvider(args.ConnectionString, args.Connection, true), false));
            args.ObjectSpaceProviders.Add(new NonPersistentObjectSpaceProvider(TypesInfo, null));
        }
        private void MintaXAFWindowsFormsApplication_CustomizeLanguagesList(object sender, CustomizeLanguagesListEventArgs e) {
            string userLanguageName = System.Threading.Thread.CurrentThread.CurrentUICulture.Name;
            if(userLanguageName != "en-US" && e.Languages.IndexOf(userLanguageName) == -1) {
                e.Languages.Add(userLanguageName);
            }
        }
        private void MintaXAFWindowsFormsApplication_DatabaseVersionMismatch(object sender, DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs e) {
#if EASYTEST
            e.Updater.Update();
            e.Handled = true;
#else
            if(System.Diagnostics.Debugger.IsAttached) {
                e.Updater.Update();
                e.Handled = true;
            }
            else {
				string message = "The application cannot connect to the specified database, " +
					"because the database doesn't exist, its version is older " +
					"than that of the application or its schema does not match " +
					"the ORM data model structure. To avoid this error, use one " +
					"of the solutions from the https://www.devexpress.com/kb=T367835 KB Article.";

				if(e.CompatibilityError != null && e.CompatibilityError.Exception != null) {
					message += "\r\n\r\nInner exception: " + e.CompatibilityError.Exception.Message;
				}
				throw new InvalidOperationException(message);
            }
#endif
        }
        protected override void ShowLogonWindow(WinWindow popupWindow)
        {
            LogonWindow = popupWindow;
            View logonView = LogonWindow.View;
            LogonWindow.ViewChanging += new EventHandler<ViewChangingEventArgs>(LogonWindow_ViewChanging);
            LogonWindow.SetView(null);
            logonView.LoadModel();
            LogonWindow.SetView(logonView);

            base.ShowLogonWindow(popupWindow);
        }
        void LogonWindow_ViewChanging(object sender, ViewChangingEventArgs e)
        {
            LogonWindow.ViewChanging -= new EventHandler<ViewChangingEventArgs>(LogonWindow_ViewChanging);
            e.DisposeOldView = false;
        }
        protected override void OnLoggedOn(LogonEventArgs args)
        {
            LogonWindow = null;
            base.OnLoggedOn(args);
        }
        private bool userDifferencesLoaded = false;
        protected override void OnSetupComplete()
        {
            base.OnSetupComplete();
            userDifferencesLoaded = false;
            LoadUserDifferences();
            userDifferencesLoaded = true;
            InitializeLanguage();
        }
        protected override void LoadUserDifferences()
        {
            if (!userDifferencesLoaded)
            {
                base.LoadUserDifferences();
            }
        }
        public Window LogonWindow { get; set; }
    }
}
