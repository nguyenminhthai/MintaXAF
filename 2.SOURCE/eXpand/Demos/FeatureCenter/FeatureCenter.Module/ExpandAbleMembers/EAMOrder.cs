﻿using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using FeatureCenter.Base;
using Xpand.ExpressApp.Attributes;
using Xpand.Persistent.Base.AdditionalViewControls;
using Xpand.Persistent.Base.General;

namespace FeatureCenter.Module.ExpandAbleMembers {
    [AdditionalViewControlsRule(Captions.ViewMessage + " " + Captions.HeaderExpandAbleMembers, "1=1", "1=1", Captions.ViewMessageExpandAbleMembers, Position.Bottom, ViewType = ViewType.DetailView)]
    [AdditionalViewControlsRule(Captions.Header + " " + Captions.HeaderExpandAbleMembers, "1=1", "1=1", Captions.HeaderExpandAbleMembers, Position.Top, ViewType = ViewType.DetailView)]
    [XpandNavigationItem("Expand Able Members", "EAMOrder_DetailView")]
    public class EAMOrder : OrderBase {
        EAMCustomer _customer;

        public EAMOrder(Session session) : base(session) {
        }
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        [ProvidedAssociation("EAMCustomer-EAMOrders")]
        public EAMCustomer Customer {
            get { return _customer; }
            set { SetPropertyValue("Customer", ref _customer, value); }
        }

        protected override void SetCustomer(ICustomer customer) {
            Customer = (EAMCustomer) customer;
        }

        protected override ICustomer GetCustomer() {
            return Customer;
        }

    }
}