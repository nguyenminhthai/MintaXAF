﻿using System;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using Xpand.Xpo.MetaData;

namespace Xpand.Persistent.Base.Xpo.MetaData {
    public class XpandCollectionMemberInfo : XpandCustomMemberInfo {
        readonly string _criteria;

        public XpandCollectionMemberInfo(XPClassInfo owner, string propertyName, Type propertyType, string criteria)
            : base(owner, propertyName, propertyType, null, true, false) {
            _criteria = criteria;
        }

        public string Criteria {
            get { return _criteria; }
        }

        public override object GetValue(object theObject) {
            var xpBaseObject = ((XPBaseObject)theObject);
            return base.GetStore(theObject).GetCustomPropertyValue(this) == null
                       ? ReflectionHelper.CreateObject(MemberType,GetArguments(xpBaseObject))
                       : base.GetValue(theObject);
        }

        object[] GetArguments(XPBaseObject xpBaseObject) {
            if (!string.IsNullOrEmpty(_criteria))
                return new object[] {
                                    xpBaseObject.Session,
                                    new CriteriaWrapper(_criteria, xpBaseObject).CriteriaOperator
                                };
            return new object[] { xpBaseObject.Session };
        }

        protected override bool CanPersist {
            get { return false; }
        }
    }
}
