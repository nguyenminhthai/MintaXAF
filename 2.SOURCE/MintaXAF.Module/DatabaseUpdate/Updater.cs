using System;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Security.Strategy;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using MintaXAF.Module.BusinessObjects;

namespace MintaXAF.Module.DatabaseUpdate {
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppUpdatingModuleUpdatertopic.aspx
    public class Updater : ModuleUpdater {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
            base(objectSpace, currentDBVersion) {
        }
        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();

            #region Security
            PermissionPolicyUser sampleUser = ObjectSpace.FindObject<PermissionPolicyUser>(new BinaryOperator("UserName", "User"));
            if (sampleUser == null)
            {
                sampleUser = ObjectSpace.CreateObject<PermissionPolicyUser>();
                sampleUser.UserName = "User";
                sampleUser.SetPassword("");
            }
            PermissionPolicyRole defaultRole = CreateDefaultRole();
            sampleUser.Roles.Add(defaultRole);

            PermissionPolicyUser userAdmin = ObjectSpace.FindObject<PermissionPolicyUser>(new BinaryOperator("UserName", "Admin"));
            if (userAdmin == null)
            {
                userAdmin = ObjectSpace.CreateObject<PermissionPolicyUser>();
                userAdmin.UserName = "Admin";
                // Set a password if the standard authentication type is used
                userAdmin.SetPassword("");
            }
            // If a role with the Administrators name doesn't exist in the database, create this role
            PermissionPolicyRole adminRole = ObjectSpace.FindObject<PermissionPolicyRole>(new BinaryOperator("Name", "Administrators"));
            if (adminRole == null)
            {
                adminRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
                adminRole.Name = "Administrators";
            }
            adminRole.IsAdministrative = true;
            userAdmin.Roles.Add(adminRole);
            #endregion

            #region Location
            // Province
            using (StreamReader sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("MintaXAF.Module.DatabaseUpdate.Data.Province.json")))
            {
                dynamic lstProvince = JsonConvert.DeserializeObject(sr.ReadToEnd());
                foreach (var item in lstProvince.Province)
                {
                    Province province = ObjectSpace.FindObject<Province>(new BinaryOperator("Code", item.code.Value));
                    if (province == null)
                    {
                        province = ObjectSpace.CreateObject<Province>();
                        province.Code = item.code.Value;
                        province.Title = item.name_with_type.Value;
                        province.Save();
                    }
                }
            }
            // District
            using (StreamReader sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("MintaXAF.Module.DatabaseUpdate.Data.District.json")))
            {
                dynamic lstDistrict = JsonConvert.DeserializeObject(sr.ReadToEnd());
                foreach (var item in lstDistrict.District)
                {
                    District district = ObjectSpace.FindObject<District>(new BinaryOperator("Code", item.code.Value));
                    Province province = ObjectSpace.FindObject<Province>(new BinaryOperator("Code", item.parent_code.Value));
                    if (district == null && province != null)
                    {
                        district = ObjectSpace.CreateObject<District>();
                        district.Code = item.code.Value;
                        district.Title = item.name_with_type.Value;
                        province.Districts.Add(district);
                        district.Save();
                    }
                }
            }

            // Commune
            using (StreamReader sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("MintaXAF.Module.DatabaseUpdate.Data.Commune.json")))
            {
                dynamic lstCommune = JsonConvert.DeserializeObject(sr.ReadToEnd());
                foreach (var item in lstCommune.Commune)
                {
                    Commune commune = ObjectSpace.FindObject<Commune>(new BinaryOperator("Code", item.code.Value));
                    District district = ObjectSpace.FindObject<District>(new BinaryOperator("Code", item.parent_code.Value));
                    if (commune == null && district != null)
                    {
                        commune = ObjectSpace.CreateObject<Commune>();
                        commune.Code = item.code.Value;
                        commune.Title = item.name_with_type.Value;
                        commune.Province = district.Province;
                        district.Communes.Add(commune);
                        commune.Save();
                    }
                }
            }
            #endregion

            ObjectSpace.CommitChanges(); //This line persists created object(s).
        }
        public override void UpdateDatabaseBeforeUpdateSchema() {
            base.UpdateDatabaseBeforeUpdateSchema();
            //if(CurrentDBVersion < new Version("1.1.0.0") && CurrentDBVersion > new Version("0.0.0.0")) {
            //    RenameColumn("DomainObject1Table", "OldColumnName", "NewColumnName");
            //}
        }

        private PermissionPolicyRole CreateDefaultRole() {
            PermissionPolicyRole defaultRole = ObjectSpace.FindObject<PermissionPolicyRole>(new BinaryOperator("Name", "Default"));
            if(defaultRole == null) {
                defaultRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
                defaultRole.Name = "Default";

				defaultRole.AddObjectPermission<PermissionPolicyUser>(SecurityOperations.Read, "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                defaultRole.AddNavigationPermission(@"Application/NavigationItems/Items/Mydetails", SecurityPermissionState.Allow);
				defaultRole.AddMemberPermission<PermissionPolicyUser>(SecurityOperations.Write, "ChangePasswordOnFirstLogon", "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
				defaultRole.AddMemberPermission<PermissionPolicyUser>(SecurityOperations.Write, "StoredPassword", "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<PermissionPolicyRole>(SecurityOperations.Read, SecurityPermissionState.Deny);
                defaultRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
				defaultRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.Create, SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.Create, SecurityPermissionState.Allow);                
            }
            return defaultRole;
        }
    }
}
