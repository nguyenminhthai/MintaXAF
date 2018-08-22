﻿using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using Xpand.ExpressApp.Logic;
using Xpand.ExpressApp.Logic.NodeUpdaters;
using Xpand.ExpressApp.ModelArtifactState.ObjectViews.Model;
using Xpand.Persistent.Base.General;
using Xpand.Persistent.Base.Logic;
using Xpand.Persistent.Base.ModelArtifact;

namespace Xpand.ExpressApp.ModelArtifactState.ObjectViews {
    public class ObjectViewsLogicInstaller : LogicInstaller<IObjectViewRule, IModelObjectViewRule> {
        public ObjectViewsLogicInstaller(XpandModuleBase xpandModuleBase)
            : base(xpandModuleBase) {

        }

        public override List<ExecutionContext> ExecutionContexts => new List<ExecutionContext>{
            ExecutionContext.CurrentObjectChanged ,
            ExecutionContext.CustomProcessSelectedItem
        };

        public override LogicRulesNodeUpdater<IObjectViewRule, IModelObjectViewRule> LogicRulesNodeUpdater => new ObjectViewRulesNodeUpdater();

        protected override IModelLogicWrapper GetModelLogicCore(IModelApplication applicationModel) {
            var conditionalObjectView = ((IModelApplicationConditionalObjectView) applicationModel).ConditionalObjectView;
            return new ModelLogicWrapper(conditionalObjectView.Rules, conditionalObjectView);
        }
    }
}