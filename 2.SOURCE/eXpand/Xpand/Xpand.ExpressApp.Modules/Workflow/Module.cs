using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Workflow;
using DevExpress.ExpressApp.Xpo.Updating;
using DevExpress.Utils;
using DevExpress.Xpo;
using Xpand.ExpressApp.Workflow.ObjectChangedWorkflows;
using Xpand.ExpressApp.Workflow.ScheduledWorkflows;
using Xpand.Persistent.Base.General;
using Xpand.Xpo.ConnectionProviders;

namespace Xpand.ExpressApp.Workflow {
    [ToolboxItem(true)]
    [ToolboxTabName(XpandAssemblyInfo.TabWinWebModules)]
    [ToolboxBitmap(typeof(WorkflowModule), "Resources.Toolbox_Module_Workflow.ico")]
    public sealed class XpandWorkFlowModule : XpandModuleBase{
        public static Type[] WorkflowTypes = {
            typeof(ScheduledWorkflow), typeof(ObjectChangedWorkflow),
            GetDxBaseImplType("DevExpress.ExpressApp.Workflow.Xpo.XpoWorkflowDefinition")
        };
        public XpandWorkFlowModule() {
            RequiredModuleTypes.Add(typeof(WorkflowModule));
            RequiredModuleTypes.Add(typeof(ConditionalAppearanceModule));
            AdditionalExportedTypes.AddRange(ModuleHelper.CollectExportedTypesFromAssembly(GetType().Assembly, IsExportedType));
        }

        protected override IEnumerable<Type> GetDeclaredExportedTypes() {
            List<Type> declaredExportedTypes = base.GetDeclaredExportedTypes().ToList();
            declaredExportedTypes.Add(typeof(ObjectChangedWorkflow));
            declaredExportedTypes.Add(typeof(ModuleInfo));
            declaredExportedTypes.Add(typeof(ScheduledWorkflow));
            return declaredExportedTypes;
        }

        public override void CustomizeTypesInfo(ITypesInfo typesInfo){
            base.CustomizeTypesInfo(typesInfo);
            var typeInfo = typesInfo.FindTypeInfo<ObjectChangedWorkflow>();
            if (RuntimeMode&&Application != null && Application.ObjectSpaceProviders.FindProvider(typeInfo.Type).GetProviderType() ==
                ConnectionProviderType.Oracle){
                var memberInfo = (XafMemberInfo)typeInfo.FindMember<ObjectChangedWorkflow>(o => o.TargetObjectType);
                memberInfo.RemoveAttributes<SizeAttribute>();
                memberInfo.AddAttribute(new SizeAttribute(255));
            }
        }
    }
}