using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.StateMachine;
using DevExpress.ExpressApp.Validation;
using DevExpress.Utils;
using Xpand.ExpressApp.Security;
using Xpand.ExpressApp.Security.Core;
using Xpand.ExpressApp.Security.Permissions;
using Xpand.ExpressApp.StateMachine.Security.Improved;
using Xpand.Persistent.Base.General;
using Xpand.Persistent.Base.Security;

namespace Xpand.ExpressApp.StateMachine {

    [ToolboxBitmap(typeof(XpandStateMachineModule))]
    [ToolboxItem(true)]
    [ToolboxTabName(XpandAssemblyInfo.TabWinWebModules)]
    public sealed class XpandStateMachineModule : XpandModuleBase, ISecurityModuleUser {
        public const string AdminRoles = "AdminRoles";
        public const string EnableFilteredProperty = "EnableFilteredProperty";
        public XpandStateMachineModule() {
            RequiredModuleTypes.Add(typeof(ValidationModule));
            RequiredModuleTypes.Add(typeof(StateMachineModule));
            RequiredModuleTypes.Add(typeof(XpandSecurityModule));
            RequiredModuleTypes.Add(typeof(ConditionalAppearanceModule));
        }
        public override void Setup(ApplicationModulesManager moduleManager) {
            base.Setup(moduleManager);
            this.AddSecurityObjectsToAdditionalExportedTypes("Xpand.Persistent.BaseImpl.StateMachine");
            if (RuntimeMode)
                Application.SetupComplete += ApplicationOnSetupComplete;
        }

        public override void Setup(XafApplication application) {
            base.Setup(application);
            if (application != null && !DesignMode) {
                application.SettingUp += ApplicationOnSettingUp;
            }
        }

        public override void CustomizeTypesInfo(ITypesInfo typesInfo) {
            base.CustomizeTypesInfo(typesInfo);
	        var stateMachineType = StateMachineType;
            var typeInfo = typesInfo.FindTypeInfo(stateMachineType);
            if (typeInfo.FindMember(EnableFilteredProperty) == null) {
                typeInfo.CreateMember(EnableFilteredProperty, typeof(bool));
            }

            if (!RuntimeMode) {
                CreateWeaklyTypedCollection(typesInfo, stateMachineType, AdminRoles);
            }
            else if (Application.CanBuildSecurityObjects()) {
                BuildSecuritySystemObjects();
            }
        }

        void ApplicationOnSettingUp(object sender, EventArgs eventArgs) {
            BuildSecuritySystemObjects();
        }

        private Type StateMachineType => ModuleManager.Modules.FindModule<StateMachineModule>().StateMachineStorageType;

        void BuildSecuritySystemObjects() {
            var dynamicSecuritySystemObjects = new DynamicSecuritySystemObjects(Application);
            var xpMemberInfos = dynamicSecuritySystemObjects.BuildRole(StateMachineType, "StateMachineRoles", "XpoStateMachines", AdminRoles);
            dynamicSecuritySystemObjects.HideInDetailView(xpMemberInfos, "XpoStateMachines");
        }

        void ApplicationOnSetupComplete(object sender, EventArgs eventArgs) {
            var securityStrategy = ((XafApplication)sender).Security as SecurityStrategy;
            if (securityStrategy != null) (securityStrategy).CustomizeRequestProcessors += OnCustomizeRequestProcessors;
        }

        void OnCustomizeRequestProcessors(object sender, CustomizeRequestProcessorsEventArgs e){
            var keyValuePair = new KeyValuePair<Type, IPermissionRequestProcessor>(typeof(StateMachineTransitionOperationRequest), e.Permissions.WithCustomPermissions().GetProcessor<StateMachineTransitionRequestProcessor>());
            e.Processors.Add(keyValuePair);
        }

    }
}