﻿using DevExpress.Xpo;
using Xpand.Persistent.Base.AdditionalViewControls;
using Xpand.Persistent.Base.General;

namespace FeatureCenter.Module.Welcome {

    [AdditionalViewControlsRule(Captions.Header + " " + Captions.HeaderWelcome, "1=1", "1=1", Captions.HeaderWelcome, Position.Top)]
    [NonPersistent]
    [XpandNavigationItem("Welcome", "WelcomeObject_DetailView")]
    [DisplayFeatureModel("WelcomeObject_DetailView", "Welcome")]
    public class WelcomeObject {
    }
}
