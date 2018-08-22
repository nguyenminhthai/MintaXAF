﻿using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Xpand.Persistent.Base.General;
using Xpand.Persistent.Base.General.Model;

namespace SystemTester.Module.FunctionalTests.RunTimeMembers{
    [DefaultClassOptions]
    [CloneView(CloneViewType.ListView, "RuntimeMembersModelDifferenceObject_ListView",DetailView = "RuntimeMembersModelDifferenceObject_DetailView")]
    [CloneView(CloneViewType.DetailView, "RuntimeMembersModelDifferenceObject_DetailView")]
    [XpandNavigationItem("RuntimeMembers/RunTimeMembers")]
    [XpandNavigationItem("RuntimeMembers/ModelDifference", "RuntimeMembersModelDifferenceObject_ListView")]
    public class RunTimeMembersObject : BaseObject{
        public RunTimeMembersObject(Session session)
            : base(session){
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Browsable(false)]
        public Address HiddenAddress { get; set; }
    }

    [NonPersistent]
    public class RunTimeMembersObjectConfig {
         
    }
}
