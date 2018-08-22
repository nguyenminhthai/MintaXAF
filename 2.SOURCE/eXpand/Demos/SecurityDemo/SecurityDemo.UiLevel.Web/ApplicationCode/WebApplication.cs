using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using Xpand.Persistent.Base.General;

namespace SecurityDemo.Web
{
    public partial class SecurityDemoAspNetApplication : WebApplication {
        private DevExpress.ExpressApp.SystemModule.SystemModule module1;
        private DevExpress.ExpressApp.Web.SystemModule.SystemAspNetModule module2;
        private SecurityDemo.Module.SecurityDemoModule module3;
        private SecurityDemo.Module.Web.SecurityDemoAspNetModule module4;
        private DevExpress.ExpressApp.Security.SecurityModule securityModule1;
        private DevExpress.ExpressApp.Objects.BusinessClassLibraryCustomizationModule module6;
        private DevExpress.ExpressApp.TreeListEditors.Web.TreeListEditorsAspNetModule module8;
        private SecurityDemo.Module.SecurityDemoAuthentication authentication1;
		private DevExpress.ExpressApp.Security.SecurityStrategyComplex securityComplex1;

        public SecurityDemoAspNetApplication()
        {
            InitializeComponent();
        }


        

        protected override void CreateDefaultObjectSpaceProvider(CreateCustomObjectSpaceProviderEventArgs args) {
			args.ObjectSpaceProvider = new XPObjectSpaceProvider(args.ConnectionString, args.Connection, true);
		}

        private void SecurityDemoAspNetApplication_DatabaseVersionMismatch(object sender, DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs e) {
            try {
                e.Updater.Update();
                e.Handled = true;
            }
            catch(CompatibilityException exception) {
                if(exception.Error is CompatibilityUnableToOpenDatabaseError) {
                    throw new UserFriendlyException(
                    "The connection to the database failed. This demo requires the local instance of Microsoft SQL Server Express. To use another database server,\r\nopen the demo solution in Visual Studio and modify connection string in the \"app.config\" file.");
                }
            }
        }

        private void InitializeComponent()
        {
            this.module1 = new DevExpress.ExpressApp.SystemModule.SystemModule();
            this.module2 = new DevExpress.ExpressApp.Web.SystemModule.SystemAspNetModule();
            this.module3 = new SecurityDemo.Module.SecurityDemoModule();
            this.module4 = new SecurityDemo.Module.Web.SecurityDemoAspNetModule();
            this.module6 = new DevExpress.ExpressApp.Objects.BusinessClassLibraryCustomizationModule();
            this.module8 = new DevExpress.ExpressApp.TreeListEditors.Web.TreeListEditorsAspNetModule();
            this.securityModule1 = new DevExpress.ExpressApp.Security.SecurityModule();
            this.authentication1 = new SecurityDemo.Module.SecurityDemoAuthentication();
			this.securityComplex1 = new DevExpress.ExpressApp.Security.SecurityStrategyComplex();

			((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
			// 
			// securityComplex1
			// 
			this.securityComplex1.Authentication = this.authentication1;
            this.securityComplex1.UserType = typeof(PermissionPolicyUser);
            this.securityComplex1.RoleType = typeof(PermissionPolicyRole);
            // 
            // SecurityDemoAspNetApplication
            // 
            this.ApplicationName = "SecurityDemo";
            this.Modules.Add(this.module1);
            this.Modules.Add(this.module2);
            this.Modules.Add(this.module3);
            this.Modules.Add(this.module4);
            this.Modules.Add(this.module6);
            this.Modules.Add(this.module8);

            this.Modules.Add(this.securityModule1);
			this.Security = this.securityComplex1;
            this.CheckCompatibilityType = DevExpress.ExpressApp.CheckCompatibilityType.DatabaseSchema;
			this.DatabaseVersionMismatch += new System.EventHandler<DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs>(this.SecurityDemoAspNetApplication_DatabaseVersionMismatch);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
    }
}
