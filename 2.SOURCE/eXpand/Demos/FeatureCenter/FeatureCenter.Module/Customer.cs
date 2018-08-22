﻿using System;
using System.ComponentModel;
using System.Linq;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using FeatureCenter.Base;
using Xpand.ExpressApp.Attributes;
using IQueryable = System.Linq.IQueryable;

namespace FeatureCenter.Module {

    public class Customer : CustomerBase {
        public Customer(Session session)
            : base(session) {
        }

        [Association("Customer-Orders")]
        public XPCollection<Order> Orders {
            get { return GetCollection<Order>("Orders"); }
        }
        private DateTime _birthDate;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public DateTime BirthDate {
            get { return _birthDate; }
            set { SetPropertyValue("BirthDate", ref _birthDate, value); }
        }
        [Browsable(false)]
        public string ConditionalControlAndMessage {
            get { return "Customer " + Name + " has less than " + Orders.Count; }
        }

        [Browsable(false)]
        public string NameWarning {
            get { return (Name + "").Length > 20 ? "Who gave him this name!!! " + Name : null; }
        }

        [Browsable(false)]
        public string CityWarning {
            get { return (City + "").Length < 3 ? "Last week I was staying at " + City : null; }
        }


        [CustomQueryProperty("Name_City", typeof(string))]
        [CustomQueryProperty("Orders_Last_OrderDate", typeof(DateTime))]
        public static IQueryable EmployeesLinq(Session session) {
            return new XPQuery<Customer>(session).Select(customer =>
                                                         new CustomerOrdersProxy {
                                                             Oid=customer.Oid,
                                                             Name_City = customer.Name + " " + customer.City,
                                                             Orders_Last_OrderDate = customer.Orders.Max(order => order.OrderDate)
                                                         });
        }
        [NonPersistent]
        public class CustomerOrdersProxy {
            public Guid Oid { get; set; }

            public string Name_City { get; set; }

            public DateTime Orders_Last_OrderDate { get; set; }
        }
    }
}