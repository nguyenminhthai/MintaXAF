﻿using DevExpress.Xpo;
using Xpand.Persistent.Base.AdditionalViewControls;
using Xpand.Persistent.Base.General;

namespace FeatureCenter.Module.Navigation {
    [AdditionalViewControlsRule(Captions.Header + " " + Captions.HeaderHideFromNewMenu, "1=1", "1=1", Captions.HeaderHideFromNewMenu, Position.Top)]
    [AdditionalViewControlsRule(Captions.ViewMessage + " " + Captions.HeaderHideFromNewMenu, "1=1", "1=1",
        Captions.ViewMessageHideFromNewMenu, Position.Bottom)]
    [NonPersistent]
    [HideFromNewMenu]
//    [XpandNavigationItem("Navigation/Hide From New Menu", "HideFromNewMenuObject_DetailView")]
    public class HideFromNewMenuObject {
    }
}
