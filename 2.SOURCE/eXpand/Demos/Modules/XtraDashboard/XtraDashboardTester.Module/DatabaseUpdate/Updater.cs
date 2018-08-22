using System;
using System.Text.RegularExpressions;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Updating;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using Xpand.ExpressApp.Dashboard.BusinessObjects;
using Xpand.ExpressApp.Security.Core;
using Xpand.Persistent.Base.General;
using Xpand.Utils.Helpers;
using XtraDashboardTester.Module.BusinessObjects;

namespace XtraDashboardTester.Module.DatabaseUpdate {
    public class Updater : ModuleUpdater {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
            base(objectSpace, currentDBVersion) {
        }

        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            var defaultRole = (PermissionPolicyRole)ObjectSpace.GetDefaultRole();

            var adminRole = ObjectSpace.GetAdminRole("Admin");
            var adminUser = adminRole.GetUser("Admin");

            var userRole = (IPermissionPolicyRole) ObjectSpace.GetRole("User");
            userRole.CanEditModel = true;

            var user = (PermissionPolicyUser) ((ISecurityRole) userRole).GetUser("user");
            user.Roles.Add(defaultRole);

            if (ObjectSpace.FindObject<Customer>(null) == null) {
                var customer = ObjectSpace.CreateObject<Customer>();
                customer.FirstName = "Apostolis";
                customer.LastName = "Bekiaris";
                customer.User=user;
                customer = ObjectSpace.CreateObject<Customer>();
                customer.FirstName = "FilteredApostolis";
                customer.LastName = "FilteredBekiaris";
                customer.User =  (PermissionPolicyUser) adminUser;
                var dashboardDefinition = ObjectSpace.CreateObject<DashboardDefinition>();
                dashboardDefinition.Name = "Filtered from model";
                dashboardDefinition.Xml = GetXML();
            }

            ObjectSpace.CommitChanges();
        }

        private string GetXML(){
            var xml = GetType().Assembly.GetManifestResourceStream(GetType(), "FilterDashboard.xml").ReadToEndAsString();
            var database = ObjectSpace.Session().Connection.Database;
            xml = Regex.Replace(xml, "(.*<Parameter Name=\"database\" Value=\")([^\"]*)(.*)", "$1" + database + "$3",
                RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Multiline);
            return xml;
        }
    }
}
