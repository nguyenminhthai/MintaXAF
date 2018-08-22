﻿using Xpand.ExpressApp.Logic.NodeUpdaters;
using Xpand.Persistent.Base.ModelArtifact;

namespace Xpand.ExpressApp.ModelArtifactState.ControllerState.Model {
    public class ControllerStateRulesNodeUpdater :LogicRulesNodeUpdater<IControllerStateRule, IModelControllerStateRule> {
        protected override void SetAttribute(IModelControllerStateRule rule,IControllerStateRule attribute) {
            rule.Attribute = attribute;
        }

    }
}