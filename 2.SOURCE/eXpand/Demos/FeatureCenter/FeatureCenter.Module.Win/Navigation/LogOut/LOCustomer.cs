﻿using DevExpress.Xpo;
using Xpand.Persistent.Base.AdditionalViewControls;
using Xpand.Persistent.Base.General;

namespace FeatureCenter.Module.Win.Navigation.LogOut {
    [AdditionalViewControlsRule(Module.Captions.ViewMessage + " " + Captions.HeaderLogOut, "1=1", "1=1",
        Captions.ViewMessageLogOut, Position.Bottom)]
    [AdditionalViewControlsRule(Module.Captions.Header + " " + Captions.HeaderLogOut, "1=1", "1=1", Captions.HeaderLogOut
        , Position.Top)]
    [NonPersistent]
    [XpandNavigationItem("Navigation/Log Out", "LOCustomer_DetailView")]
    [DisplayFeatureModel("LOCustomer_DetailView", "LogOut")]
    public class LOCustomer  {
        
    }
}