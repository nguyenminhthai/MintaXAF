﻿using DevExpress.Xpo;
using eXpand.ExpressApp.Attributes;
using FeatureCenter.Base;

namespace FeatureCenter.Module.HideToolBar {
    public class HTBOrderLine : OrderLineBase {
        HTBOrder _order;

        public HTBOrderLine(Session session) : base(session) {
        }

        [ProvidedAssociation("HTBOrder-HTBOrderLines")]
        public HTBOrder Order {
            get { return _order; }
            set { SetPropertyValue("Order", ref _order, value); }
        }

        protected override void SetOrder(IOrder order) {
            Order = (HTBOrder) order;
        }

        protected override IOrder GetOrder() {
            return Order;
        }
    }
}