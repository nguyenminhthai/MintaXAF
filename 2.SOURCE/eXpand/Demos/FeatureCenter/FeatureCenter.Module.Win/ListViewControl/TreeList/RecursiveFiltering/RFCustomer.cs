﻿using DevExpress.Persistent.Base.General;
using DevExpress.Xpo;
using FeatureCenter.Base;
using Xpand.Persistent.Base.AdditionalViewControls;
using Xpand.Persistent.Base.General;

namespace FeatureCenter.Module.Win.ListViewControl.TreeList.RecursiveFiltering {
    [AdditionalViewControlsRule(Module.Captions.ViewMessage + " " + Captions.HeaderRecursiveFiltering, "1=1", "1=1",
        Captions.ViewMessageRecursiveFiltering, Position.Bottom)]
    [AdditionalViewControlsRule(Module.Captions.Header + " " + Captions.HeaderRecursiveFiltering, "1=1", "1=1",
        Captions.HeaderRecursiveFiltering, Position.Top)]
    [XpandNavigationItem(Module.Captions.ListViewCotrol + Module.Captions.TreeListView + "RecursiveFiltering", "RFCustomer_ListView")]
    [DisplayFeatureModel("RFCustomer_ListView", "RecursiveFiltering")]
    public class RFCustomer : CustomerBase, ICategorizedItem {
        public RFCustomer(Session session)
            : base(session) {
        }

        ITreeNode ICategorizedItem.Category {
            get { return Category; }
            set { Category = value as RFCategory; }
        }
        private RFCategory _rfCategory;

        [Association("RFCategory-RFCustomers")]
        public RFCategory Category {
            get { return _rfCategory; }
            set { SetPropertyValue("Category", ref _rfCategory, value); }
        }
    }
}
