
using System;
using System.Configuration;
using System.Windows.Forms;
using AdditionalViewControlProviderTester.Module;
using DevExpress.ExpressApp.Security;

namespace AdditionalViewControlProviderTester.Win {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
#if EASYTEST
			DevExpress.ExpressApp.Win.EasyTest.EasyTestRemotingRegistration.Register();
#endif

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            EditModelPermission.AlwaysGranted = true;
            AdditionalViewControlProviderTesterWindowsFormsApplication winApplication = new AdditionalViewControlProviderTesterWindowsFormsApplication();
#if EASYTEST
			if(ConfigurationManager.ConnectionStrings["EasyTestConnectionString"] != null) {
				winApplication.ConnectionString = ConfigurationManager.ConnectionStrings["EasyTestConnectionString"].ConnectionString;
			}
#else
            if (ConfigurationManager.ConnectionStrings["ConnectionString"] != null&&string.IsNullOrEmpty(winApplication.ConnectionString)) {
                winApplication.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            }
#endif
            try {
                winApplication.UseOldTemplates=false;
                winApplication.ProjectSetup();
                winApplication.Setup();
                winApplication.Start();
            } catch (Exception e) {
                winApplication.HandleException(e);
            }
        }
    }
}
