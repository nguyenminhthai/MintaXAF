#if !EASYTEST
using System;
using System.Diagnostics;
using System.Windows.Forms;
using Xpand.Persistent.Base.General;
#endif
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Xpo;
using Xpand.ExpressApp.Security.Core;
using Xpand.ExpressApp.Win;
using Xpand.Persistent.Base.General;
using Xpand.Persistent.BaseImpl.Security;


namespace FeatureCenter.Win {
    public partial class FeatureCenterWindowsFormsApplication : XpandWinApplication {
        public FeatureCenterWindowsFormsApplication() {
            InitializeComponent();
            this.NewSecurityStrategyComplexV2<XpandPermissionPolicyUser, XpandPermissionPolicyRole>(typeof(AuthenticationStandard), typeof(AuthenticationStandardLogonParameters));
        }

        protected override void CreateDefaultObjectSpaceProvider(CreateCustomObjectSpaceProviderEventArgs args) {
//            args.ObjectSpaceProviders.Add(new XpandObjectSpaceProvider(new MultiDataStoreProvider(args.ConnectionString), this.Security, false));
            args.ObjectSpaceProviders.Add(new XPObjectSpaceProvider(args.ConnectionString));
            args.ObjectSpaceProviders.Add(new NonPersistentObjectSpaceProvider());
        }
        //        protected override ShowViewStrategyBase CreateShowViewStrategy() {
        //            return new ShowInSingleWindowStrategy(this);
        //        }
#if EASYTEST
        protected override string GetUserCultureName() {
            return "en-US";
        }
#endif
        private void FeatureCenterWindowsFormsApplication_DatabaseVersionMismatch(object sender, DatabaseVersionMismatchEventArgs e) {

#if EASYTEST
			e.Updater.Update();
			e.Handled = true;
#else
            if (Debugger.IsAttached) {
                e.Updater.Update();
                e.Handled = true;
            } else {
                throw new InvalidOperationException(
                    "The application cannot connect to the specified database, because the latter doesn't exist or its version is older than that of the application.\r\n" +
                    "The automatic update is disabled, because the application was started without debugging.\r\n" +
                    "You should start the application under Visual Studio, or modify the " +
                    "source code of the 'DatabaseVersionMismatch' event handler to enable automatic database update, " +
                    "or manually create a database using the 'DBUpdater' tool.");
            }
#endif
        }
    }
}
