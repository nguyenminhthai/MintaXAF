﻿using System;
using System.ComponentModel;
using System.Linq;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using Xpand.Persistent.Base.Logic.Model;
using Xpand.Persistent.Base.MasterDetail;

namespace Xpand.ExpressApp.MasterDetail.Model {
    [ModelInterfaceImplementor(typeof(IContextMasterDetailRule), "Attribute")]
    public interface IModelMasterDetailRule : IContextMasterDetailRule, IModelConditionalLogicRule<IMasterDetailRule> {
        [Browsable(false)]
        IModelList<IModelListView> ChildListViews { get; }
        [Browsable(false)]
        IModelList<IModelMember> CollectionMembers { get; }
    }
    [DomainLogic(typeof(IModelMasterDetailRule))]
    public class MasterDetailRuleDomainLogic {

        public static IModelList<IModelMember> Get_CollectionMembers(IModelMasterDetailRule masterDetailRule) {
            var calculatedModelNodeList = new CalculatedModelNodeList<IModelMember>();
            if (masterDetailRule.ModelClass != null) {
                var modelMembers = masterDetailRule.Application.BOModel.GetClass(masterDetailRule.ModelClass.TypeInfo.Type).AllMembers.
                        Where(member => member.MemberInfo.IsList);
                calculatedModelNodeList.AddRange(modelMembers);
            }
            return calculatedModelNodeList;
        }
        public static IModelList<IModelListView> Get_ChildListViews(IModelMasterDetailRule masterDetailRule) {
            var calculatedModelNodeList = new CalculatedModelNodeList<IModelListView>();
            IModelMember collectionMember = masterDetailRule.CollectionMember;
            if (collectionMember == null)
                return calculatedModelNodeList;
            Type listType = masterDetailRule.CollectionMember.MemberInfo.ListElementTypeInfo.Type;
            calculatedModelNodeList.AddRange(masterDetailRule.Application.Views.OfType<IModelListView>().Where(view => view.ModelClass.TypeInfo.Type == listType));
            return calculatedModelNodeList;
        }
    }

}