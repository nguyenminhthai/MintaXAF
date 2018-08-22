﻿using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using Xpand.Persistent.Base.General.Model;
using Xpand.Xpo;

namespace Xpand.Persistent.Base.RuntimeMembers.Model {
    [ModelPersistentName("RuntimeMember")]
    [ModelDisplayName("Persistent")]
    public interface IModelMemberPersistent : IModelMemberEx, IModelMemberDataStoreForeignKeyCreated {
        
        [Browsable((false))]
        bool DataStoreColumnCreated { get; set; }
        
    }

    [DomainLogic(typeof(IModelMemberPersistent))]
    public class ModelMemberPersistentDomainLogic : ModelMemberExDomainLogicBase<IModelMemberPersistent> {
        public static IMemberInfo Get_MemberInfo(IModelMemberPersistent modelRuntimeMember){
            return GetMemberInfo(modelRuntimeMember, 
                (persistent, info) => info.CreateCustomMember(persistent.Name, persistent.Type, false),
                persistent => true);
        }
    }
}