﻿using System;
using System.Collections.Generic;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using MbUnit.Framework;
using TypeMock.ArrangeActAssert;
using Xpand.Persistent.Base.General;

namespace Xpand.Tests.Xpand.ExpressApp {
    /// <summary>
    /// AN_Association_Collection_Can_Be_Created_To_Any_Persistent_Type
    /// An_Associated_Member_Can_Be_Created_To_Any_Perstent_type
    /// Both_Parts_One_To_Many_Of_Association_Can_Be_Created_To_Any_Persistent_Member
    /// </summary>
    [TestFixture]
    public class TypesInfoFixture {
        [SetUp]
        public void Setup() {
            XafTypesInfo.Reset();
        }
        [Test]
        public void AN_Association_Collection_Can_Be_Created_To_Any_Persistent_Type() {

            ITypesInfo info = XafTypesInfo.Instance;
            Type typeToCreateOn = typeof(User);
            info.RegisterEntity(typeToCreateOn);
            Type typeOfCollection = typeof(Analysis);

            XPMemberInfo collection = info.CreateCollection(typeToCreateOn, typeOfCollection, "association",
                                                                  XpoTypesInfoHelper.GetXpoTypeInfoSource().XPDictionary);

            AssociationAttribute attribute = assertMemberCreation(collection, typeOfCollection.Name + "s", typeToCreateOn);
            Assert.AreEqual(typeOfCollection.FullName, attribute.ElementTypeName);

        }

        private AssociationAttribute assertMemberCreation(object collection, string name, Type type) {
            Assert.IsNotNull(collection);
            ITypeInfo typeInfo = XafTypesInfo.CastTypeToTypeInfo(type);
            IMemberInfo memberInfo = typeInfo.FindMember(name);
            Assert.IsNotNull(memberInfo);
            var attribute = memberInfo.FindAttribute<AssociationAttribute>();
            Assert.IsNotNull(attribute);
            Assert.AreEqual("association", attribute.Name);
            return attribute;
        }

        [Test]
        [Isolated]
        public void An_Associated_Member_Can_Be_Created_To_Any_Perstent_type() {
            ITypesInfo info = XafTypesInfo.Instance;
            Type typeToCreateOn = typeof(User);
            info.RegisterEntity(typeToCreateOn);
            Type typeOfMember = typeof(Analysis);

            XPMemberInfo member = info.CreateMember(typeToCreateOn, typeOfMember, "association", XpoTypesInfoHelper.GetXpoTypeInfoSource().XPDictionary);

            assertMemberCreation(member, typeOfMember.Name, typeToCreateOn);
        }
        [Test]
        [Isolated]

        public void Both_Parts_One_To_Many_Of_Association_Can_Be_Created_To_Any_Persistent_Member() {
            ITypesInfo info = XafTypesInfo.Instance;
            Type typeToCreateOn = typeof(User);
            info.RegisterEntity(typeToCreateOn);
            Type typeOfMember = typeof(Analysis);
            info.RegisterEntity(typeOfMember);

            List<XPMemberInfo> members = info.CreateBothPartMembers(typeToCreateOn, typeOfMember,
                                                                          XpoTypesInfoHelper.GetXpoTypeInfoSource().XPDictionary, false, "association");

            assertMemberCreation(members, typeOfMember.Name, typeToCreateOn);
            assertMemberCreation(members, typeToCreateOn.Name + "s", typeOfMember);
        }
        [Test]
        [Isolated]

        public void Both_Parts_OF_Many_To_Many_Of_Association_Can_Be_Created_To_Any_Persistent_Member() {
            ITypesInfo info = XafTypesInfo.Instance;
            Type typeToCreateOn = typeof(User);
            info.RegisterEntity(typeToCreateOn);
            Type typeOfMember = typeof(Analysis);
            info.RegisterEntity(typeOfMember);

            List<XPMemberInfo> members = info.CreateBothPartMembers(typeToCreateOn, typeOfMember, XpoTypesInfoHelper.GetXpoTypeInfoSource().XPDictionary, true, "association");

            assertMemberCreation(members, typeToCreateOn.Name + "s", typeOfMember);
            assertMemberCreation(members, typeOfMember.Name + "s", typeToCreateOn);
        }
    }
}