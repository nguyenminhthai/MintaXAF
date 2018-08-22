﻿using DevExpress.Xpo;
using Xpand.Persistent.Base.AdditionalViewControls;
using Xpand.Persistent.Base.General;

namespace FeatureCenter.Module.Win.Miscellaneous.ApplicationMultipleInstances
{
    [AdditionalViewControlsRule(Module.Captions.ViewMessage + " " + Captions.HeaderApplicationMultipleInstances, "1=1", "1=1",
        Captions.ViewMessageApplicationMultipleInstances, Position.Bottom)]
    [AdditionalViewControlsRule(Module.Captions.Header + " " + Captions.HeaderApplicationMultipleInstances, "1=1", "1=1",
        Captions.HeaderApplicationMultipleInstances, Position.Top)]
    [NonPersistent]
    [XpandNavigationItem(Module.Captions.Miscellaneous + "Application Multiple Instances", "AMIObject_DetailView")]
    [DisplayFeatureModel("AMIObject_DetailView", "ApplicationMultipleInstances")]
    public class AMIObject {
        
    }
}
