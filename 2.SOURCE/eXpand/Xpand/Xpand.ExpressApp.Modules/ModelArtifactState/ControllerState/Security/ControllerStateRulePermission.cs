﻿using System;
using System.Security;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Xpand.ExpressApp.ModelArtifactState.ArtifactState.Security;
using Xpand.Persistent.Base.ModelArtifact;
using Xpand.Persistent.Base.Validation.AtLeast1PropertyIsRequired;

namespace Xpand.ExpressApp.ModelArtifactState.ControllerState.Security {
    [RuleRequiredForAtLeast1Property(null, DefaultContexts.Save, "Module,ControllerType")]
    [NonPersistent]
    public class ControllerStateRulePermission : ArtifactStateRulePermission, IControllerStateRule {
        #region IControllerStateRule Members
        public Type ControllerType { get; set; }

        public Persistent.Base.ModelArtifact.ControllerState ControllerState { get; set; }

        #endregion
        public override IPermission Copy() {
            return new ControllerStateRulePermission();
        }

        public override string ToString() {
            return string.Format("{2}: {0} {1}", ControllerType, ID, GetType().Name);
        }
    }
}