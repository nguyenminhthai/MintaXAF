﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.Security;
using DevExpress.Xpo;
using Xpand.ExpressApp.ModelDifference.DataStore.Queries;
using Xpand.Persistent.Base;
using Xpand.Persistent.Base.General;
using Xpand.Persistent.Base.General.Model;
using Xpand.Persistent.Base.ModelDifference;

namespace Xpand.ExpressApp.ModelDifference.DataStore.BaseObjects {
    [HideFromNewMenu, ModelDefault("Caption", "User Settings"), VisibleInReports(false)]
    [CloneView(CloneViewType.DetailView, "UDO_DetailView",true)]
    [CreatableItem(false)]
    public class UserModelDifferenceObject : ModelDifferenceObject {
        private bool _nonPersistent;

        public UserModelDifferenceObject(Session session)
            : base(session) {
        }

        public bool NonPersistent {
            get { return _nonPersistent; }
            set { SetPropertyValue(MethodBase.GetCurrentMethod().Name.Replace("set_", ""), ref _nonPersistent, value); }
        }
        [Browsable(false)]
        public bool IsCurrentUserModel => ReferenceEquals(new QueryUserModelDifferenceObject(Session).GetActiveModelDifference(ApplicationHelper.Instance.Application.GetType().FullName, Name,DeviceCategory), this);

        public override void AfterConstruction() {
            base.AfterConstruction();
            DifferenceType = DifferenceType.User;
        }
        public override ModelDifferenceObject InitializeMembers(string name, string applicationTitle, string uniqueName, DeviceCategory deviceCategory = DeviceCategory.All) {
            var modelDifferenceObject = base.InitializeMembers(name, applicationTitle, uniqueName,deviceCategory);
            modelDifferenceObject.Name =string.Format(
                    ((IModelOptionsModelDifference) ApplicationHelper.Instance.Application.Model.Options)
                    .UserModelDifferenceObjectSubjectTemplate, DateTime.Now, GetUserName(),deviceCategory);
            return modelDifferenceObject;
        }

        private static string GetUserName(){
            return SecuritySystem.UserType!=null ? ((IAuthenticationStandardUser)SecuritySystem.CurrentUser).UserName : null;
        }

        public void AssignToCurrentUser() {
            if (ClassInfo.FindMember("Users")!=null) {
                var list = ((IList)GetMemberValue("Users"));
                object value =
                    ((XPBaseObject)SecuritySystem.CurrentUser).ClassInfo.KeyProperty.GetValue(SecuritySystem.CurrentUser);
                object objectByKey = Session.GetObjectByKey(SecuritySystem.UserType, value);
                list.Add(objectByKey);
            }
        }
        public override IEnumerable<ModelApplicationBase> GetAllLayers(ModelApplicationBase master) {
            var differenceObjects = new QueryRoleModelDifferenceObject(Session).GetActiveModelDifferences(PersistentApplication.UniqueName, null).Cast<ModelDifferenceObject>();
            differenceObjects = differenceObjects.Concat(new QueryModelDifferenceObject(Session).GetActiveModelDifferences(PersistentApplication.UniqueName, null));
            return GetAllLayers(differenceObjects, master);
        }

    }
}