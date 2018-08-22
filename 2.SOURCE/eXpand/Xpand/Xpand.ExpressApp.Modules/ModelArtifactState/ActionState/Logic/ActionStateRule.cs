﻿using System.ComponentModel;
using Xpand.ExpressApp.ModelArtifactState.ArtifactState.Logic;
using Xpand.Persistent.Base.ModelArtifact;

namespace Xpand.ExpressApp.ModelArtifactState.ActionState.Logic {
    public class ActionStateRule : ArtifactStateRule, IActionStateRule {
        public ActionStateRule(IContextActionStateRule actionStateRule)
            : base(actionStateRule) {
            ActionId = actionStateRule.ActionId;
            ActionState = actionStateRule.ActionState;
            ActionContext = actionStateRule.ActionContext;
        }

        public string ActionContext { get; set; }

        [Category("Data")]
        public string ActionId { get; set; }

        [Category("Behavior")]
        public Persistent.Base.ModelArtifact.ActionState ActionState { get; set; }
    }
}
