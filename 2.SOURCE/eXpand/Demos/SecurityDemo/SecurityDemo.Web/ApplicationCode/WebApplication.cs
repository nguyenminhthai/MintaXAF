using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Xpo;
using Xpand.Persistent.Base.General;

namespace SecurityDemo.Web
{
    public partial class SecurityDemoAspNetApplication : WebApplication {
        private DevExpress.ExpressApp.SystemModule.SystemModule module1;
        private DevExpress.ExpressApp.Web.SystemModule.SystemAspNetModule module2;
        private SecurityDemo.Module.SecurityDemoModule module3;
        private SecurityDemo.Module.Web.SecurityDemoAspNetModule module4;
        private DevExpress.ExpressApp.Security.SecurityModule securityModule1;
        private DevExpress.ExpressApp.Validation.ValidationModule validationModule;
        private DevExpress.ExpressApp.Validation.Web.ValidationAspNetModule validationAspNetModule;
        private DevExpress.ExpressApp.Objects.BusinessClassLibraryCustomizationModule module6;
        private DevExpress.ExpressApp.TreeListEditors.Web.TreeListEditorsAspNetModule module8;

        public SecurityDemoAspNetApplication()
        {
            InitializeComponent();
        }

		protected override void CreateDefaultObjectSpaceProvider(CreateCustomObjectSpaceProviderEventArgs args) {
			args.ObjectSpaceProvider = new XPObjectSpaceProvider(args.ConnectionString, args.Connection, true);
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
            this.validationModule = new DevExpress.ExpressApp.Validation.ValidationModule();
            this.validationAspNetModule = new DevExpress.ExpressApp.Validation.Web.ValidationAspNetModule();

			((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
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
            this.Modules.Add(this.validationModule);
            this.Modules.Add(this.validationAspNetModule);

            this.Modules.Add(this.securityModule1);
            this.CheckCompatibilityType = DevExpress.ExpressApp.CheckCompatibilityType.DatabaseSchema;
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }


    }
}
