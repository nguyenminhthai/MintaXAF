using System.Collections.Generic;
using Xpand.ExpressApp.ModelArtifactState.ControllerState.Security.Improved;

namespace Xpand.ExpressApp.PivotChart.Security.Improved {
    public class ShowInAnalysisPermission : ControllerStateRulePermission {
        public new const string OperationName = "ShowInAnalysis";
        public ShowInAnalysisPermission(ShowInAnalysisOperationPermissionData logicRule)
            : base(OperationName, logicRule) {
        }
        public override IList<string> GetSupportedOperations() {
            return new[] { OperationName };
        }

    }
}