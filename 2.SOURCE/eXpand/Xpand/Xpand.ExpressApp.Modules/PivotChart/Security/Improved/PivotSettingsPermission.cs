﻿using System.Collections.Generic;
using Xpand.ExpressApp.ModelArtifactState.ControllerState.Security.Improved;

namespace Xpand.ExpressApp.PivotChart.Security.Improved {
    public class PivotSettingsPermission : ControllerStateRulePermission {
        public new const string OperationName = "PivotSettings";
        public PivotSettingsPermission(PivotSettingsOperationPermissionData logicRule)
            : base(OperationName, logicRule) {

        }

        public override IList<string> GetSupportedOperations() {
            return new[] { OperationName };
        }
    }
}
