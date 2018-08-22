﻿using System;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using Xpand.Persistent.Base.General;
using Xpand.Xpo;

namespace FeatureCenter.Module.RuntimeFields.CalculatedWithCode {
    public class CreateRuntimeCalculatedFieldController : ViewController {
        public override void CustomizeTypesInfo(DevExpress.ExpressApp.DC.ITypesInfo typesInfo) {
            base.CustomizeTypesInfo(typesInfo);
            var classInfo = typeof(Customer).GetTypeInfo().QueryXPClassInfo();

            if (classInfo.FindMember("SumOfOrderTotals") == null) {
                var xpandCalcMemberInfo = classInfo.CreateCalculabeMember("SumOfOrderTotals", typeof(float), "Orders.Sum(Total)");
                var attributes = new Attribute[] {new VisibleInListViewAttribute(false),new VisibleInLookupListViewAttribute(false),
                                                  new VisibleInDetailViewAttribute(false)};
                foreach (var attribute in attributes) {
                    xpandCalcMemberInfo.AddAttribute(attribute);
                }

                typesInfo.RefreshInfo(typeof(Customer));
            }
        }
    }
}
