using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using Xpand.ExpressApp.AdditionalViewControlsProvider.Model;
using Xpand.ExpressApp.Logic;
using Xpand.ExpressApp.Logic.NodeUpdaters;
using Xpand.Persistent.Base.AdditionalViewControls;
using Xpand.Persistent.Base.General;
using Xpand.Persistent.Base.Logic;

namespace Xpand.ExpressApp.AdditionalViewControlsProvider {
    public class AdditionalViewControlsLogicInstaller:LogicInstaller<IAdditionalViewControlsRule,IModelAdditionalViewControlsRule> {
        public AdditionalViewControlsLogicInstaller(XpandModuleBase xpandModuleBase) : base(xpandModuleBase) {
        }

        public override List<ExecutionContext> ExecutionContexts => new List<ExecutionContext>{ExecutionContext.ViewChanged};

        public override LogicRulesNodeUpdater<IAdditionalViewControlsRule, IModelAdditionalViewControlsRule> LogicRulesNodeUpdater => new AdditionalViewControlsRulesNodeUpdater();

        protected override IModelLogicWrapper GetModelLogicCore(IModelApplication applicationModel) {
            var additionalViewControls = ((IModelApplicationAdditionalViewControls) applicationModel).AdditionalViewControls;
            return new ModelLogicWrapper(additionalViewControls.Rules, additionalViewControls.ExecutionContextsGroup,
                                         additionalViewControls.ViewContextsGroup,
                                         additionalViewControls.FrameTemplateContextsGroup);
        }

    }
}