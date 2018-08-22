﻿using Xpand.ExpressApp.Logic.NodeUpdaters;
using Xpand.Persistent.Base.ModelArtifact;

namespace Xpand.ExpressApp.ModelArtifactState.ObjectViews.Model {
    public class ObjectViewRulesNodeUpdater :LogicRulesNodeUpdater<IObjectViewRule, IModelObjectViewRule> {
        protected override void SetAttribute(IModelObjectViewRule rule, IObjectViewRule attribute) {
            rule.Attribute = attribute;
        }

    }
}