using DevExpress.ExpressApp.Win;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;

namespace WorkflowTester.Win {
    public partial class WorkflowTesterWindowsFormsApplication : WinApplication {
        public WorkflowTesterWindowsFormsApplication() {
            InitializeComponent();
            DelayedViewItemsInitialization = true;
        }
#if EASYTEST
        protected override string GetUserCultureName() {
            return "en-US";
        }
#endif
        protected override void CreateDefaultObjectSpaceProvider(CreateCustomObjectSpaceProviderEventArgs args) {
            args.ObjectSpaceProviders.Add(new XPObjectSpaceProvider(args.ConnectionString, args.Connection));
            args.ObjectSpaceProviders.Add(new NonPersistentObjectSpaceProvider());
        }

        private void WorkflowTesterWindowsFormsApplication_DatabaseVersionMismatch(object sender, DatabaseVersionMismatchEventArgs e) {
            e.Updater.Update();
            e.Handled = true;
        }

        private void WorkflowTesterWindowsFormsApplication_CustomizeLanguagesList(object sender, CustomizeLanguagesListEventArgs e) {
            string userLanguageName = System.Threading.Thread.CurrentThread.CurrentUICulture.Name;
            if (userLanguageName != "en-US" && e.Languages.IndexOf(userLanguageName) == -1) {
                e.Languages.Add(userLanguageName);
            }
        }
    }
}
