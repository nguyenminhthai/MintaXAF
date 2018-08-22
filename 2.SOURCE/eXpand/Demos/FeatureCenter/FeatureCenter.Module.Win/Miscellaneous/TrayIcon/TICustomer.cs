﻿using DevExpress.Xpo;
using Xpand.Persistent.Base.AdditionalViewControls;
using Xpand.Persistent.Base.General;

namespace FeatureCenter.Module.Win.Miscellaneous.TrayIcon {
    [AdditionalViewControlsRule(Module.Captions.ViewMessage + " " + Captions.HeaderTrayIcon, "1=1", "1=1",
        Captions.ViewMessageTrayIcon, Position.Bottom)]
    [AdditionalViewControlsRule(Module.Captions.Header + " " + Captions.HeaderTrayIcon, "1=1", "1=1",
        Captions.HeaderTrayIcon, Position.Top)]
    [NonPersistent]
    [XpandNavigationItem(Module.Captions.Miscellaneous + "Tray Icon", "TICustomer_DetailView")]
    [DisplayFeatureModel("TICustomer_DetailView", "TrayIcon")]
    public class TICustomer {
    }
}
