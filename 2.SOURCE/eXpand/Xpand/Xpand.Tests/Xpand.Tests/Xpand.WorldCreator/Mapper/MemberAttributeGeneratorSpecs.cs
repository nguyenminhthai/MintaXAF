﻿using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using Machine.Specifications;
using Xpand.ExpressApp.WorldCreator.DBMapper;
using Xpand.Persistent.Base.PersistentMetaData;
using Xpand.Persistent.Base.PersistentMetaData.PersistentAttributeInfos;
using Xpand.Persistent.BaseImpl.PersistentMetaData;
using Xpand.Persistent.BaseImpl.PersistentMetaData.PersistentAttributeInfos;
using Xpand.ExpressApp.WorldCreator.Core;
using System;

namespace Xpand.Tests.Xpand.WorldCreator.Mapper {
    [Ignore("not implemented")]
    public class MemberAttributeGeneratorSpecs {
        public class When_a_column_is_nullable : With_In_Memory_DataStore {
            // ReSharper disable UnassignedField.Local
            static List<IPersistentAttributeInfo> _persistentAttributeInfos;
            // ReSharper restore UnassignedField.Local

            Establish context = () => {
                //                new DBColumn(){}
                //                _column.Nullable = false;
            };

            Because of = () => {
                //                _persistentAttributeInfos = new AttributeGenerator(XPObjectSpace).Create();
            };

            //            It should_create_a_rule_required_attribute =
            //                () => _persistentAttributeInfos.OfType<PersistentRuleRequiredFieldAttribute>().FirstOrDefault().ShouldNotBeNull();
        }

        public class When_column_size_is_100 : With_In_Memory_DataStore {
            static DBTable _dbTable;
            static DBColumn _column;
            static PersistentMemberInfo _persistentMemberInfo;


            Establish context = () => {
                _column = new DBColumn("col", false, "text", 100, DBColumnType.String);
                _persistentMemberInfo = XPObjectSpace.CreateObject<PersistentCoreTypeMemberInfo>();
                var persistentClassInfo = XPObjectSpace.CreateObject<PersistentClassInfo>();
                persistentClassInfo.PersistentAssemblyInfo = XPObjectSpace.CreateObject<PersistentAssemblyInfo>();
                persistentClassInfo.SetDefaultTemplate(TemplateType.Class);
                _persistentMemberInfo.Owner = persistentClassInfo;
                _persistentMemberInfo.SetDefaultTemplate(TemplateType.XPReadWritePropertyMember);
                _dbTable = new DBTable();
                _dbTable.Columns.Add(_column);
            };

            Because of =
                () => new MemberAttributeGenerator(new MemberGeneratorInfo(_persistentMemberInfo, _column), new ClassGeneratorInfo(_persistentMemberInfo.Owner, _dbTable)).Create();

            It should_create_a_size_attribute_lenght_100 =
                () => _persistentMemberInfo.TypeAttributes.OfType<PersistentSizeAttribute>().Single().Size.ShouldEqual(100);
        }
        public class When_column_is_primary_key : With_In_Memory_DataStore {
            static PersistentCoreTypeMemberInfo _persistentCoreTypeMemberInfo;
            static DBTable _dbTable;
            static DBColumn _column;

            Establish context = () => {
                _column = new DBColumn("col", false, "text", 100, DBColumnType.String);
                _persistentCoreTypeMemberInfo = XPObjectSpace.CreateObject<PersistentCoreTypeMemberInfo>();
                var persistentClassInfo = XPObjectSpace.CreateObject<PersistentClassInfo>();
                persistentClassInfo.PersistentAssemblyInfo = XPObjectSpace.CreateObject<PersistentAssemblyInfo>();
                persistentClassInfo.SetDefaultTemplate(TemplateType.Class);
                _persistentCoreTypeMemberInfo.Owner = persistentClassInfo;
                _persistentCoreTypeMemberInfo.SetDefaultTemplate(TemplateType.XPReadWritePropertyMember);
                _dbTable = new DBTable();
                _dbTable.Columns.Add(_column);
                _dbTable.PrimaryKey = new DBPrimaryKey(new[] { _column });
            };

            Because of = () => new MemberAttributeGenerator(new MemberGeneratorInfo(_persistentCoreTypeMemberInfo, _column), new ClassGeneratorInfo(_persistentCoreTypeMemberInfo.Owner, _dbTable)).Create();

            It should_create_persistent_key_attribute =
                () => _persistentCoreTypeMemberInfo.TypeAttributes.OfType<PersistentKeyAttribute>().FirstOrDefault().ShouldNotBeNull();
        }

    }

    public class When_column_is_primary_key_and_autogenerated : With_In_Memory_DataStore {
        static PersistentCoreTypeMemberInfo _persistentCoreTypeMemberInfo;
        static DBTable _dbTable;
        static DBColumn _column;

        Establish context = () => {
            _column = new DBColumn("col", false, "text", 100, DBColumnType.String) { IsIdentity = true };
            _persistentCoreTypeMemberInfo = XPObjectSpace.CreateObject<PersistentCoreTypeMemberInfo>();
            var persistentClassInfo = XPObjectSpace.CreateObject<PersistentClassInfo>();
            persistentClassInfo.PersistentAssemblyInfo = XPObjectSpace.CreateObject<PersistentAssemblyInfo>();
            persistentClassInfo.SetDefaultTemplate(TemplateType.Class);
            _persistentCoreTypeMemberInfo.Owner = persistentClassInfo;
            _persistentCoreTypeMemberInfo.SetDefaultTemplate(TemplateType.XPReadWritePropertyMember);
            _dbTable = new DBTable();
            _dbTable.Columns.Add(_column);
            _dbTable.PrimaryKey = new DBPrimaryKey(new[] { _column });
        };

        Because of = () => new MemberAttributeGenerator(new MemberGeneratorInfo(_persistentCoreTypeMemberInfo, _column), new ClassGeneratorInfo(_persistentCoreTypeMemberInfo.Owner, _dbTable)).Create();

        It should_create_an_autogenerated_persistentkey_attribute =
            () => _persistentCoreTypeMemberInfo.TypeAttributes.OfType<PersistentKeyAttribute>().Single().AutoGenerated.ShouldEqual(true);
    }
    public class When_column_is_foreign : With_In_Memory_DataStore {
        static PersistentAssociationAttribute _persistentAssociationAttribute;
        static DBTable _dbTable;
        static PersistentReferenceMemberInfo _persistentReferenceMemberInfo;
        static DBColumn _column;

        Establish context = () => {
            _column = new DBColumn("col", true, "int", 0, DBColumnType.Int32);
            var persistentAssemblyInfo = XPObjectSpace.CreateObject<PersistentAssemblyInfo>();
            persistentAssemblyInfo.Name = "persistentAssemblyInfo";
            _persistentReferenceMemberInfo = XPObjectSpace.CreateObject<PersistentReferenceMemberInfo>();
            _persistentReferenceMemberInfo.Name = "ReferenceMemberInfo";
            var referenceClassInfo = XPObjectSpace.CreateObject<PersistentClassInfo>();
            referenceClassInfo.Name = "referenceClassInfo";
            referenceClassInfo.PersistentAssemblyInfo = persistentAssemblyInfo;
            referenceClassInfo.SetDefaultTemplate(TemplateType.Class);
            _persistentReferenceMemberInfo.ReferenceClassInfo = referenceClassInfo;
            var persistentClassInfo = XPObjectSpace.CreateObject<PersistentClassInfo>();
            persistentClassInfo.PersistentAssemblyInfo = persistentAssemblyInfo;
            persistentClassInfo.Name = "persistentClassInfo";
            persistentClassInfo.SetDefaultTemplate(TemplateType.Class);
            _persistentReferenceMemberInfo.Owner = persistentClassInfo;
            _persistentReferenceMemberInfo.SetDefaultTemplate(TemplateType.XPReadWritePropertyMember);
            _dbTable = new DBTable();
            _dbTable.Columns.Add(_column);
            _dbTable.ForeignKeys.Add(new DBForeignKey(new[] { _column }, "", new StringCollection()));
        };

        Because of = () => new MemberAttributeGenerator(new MemberGeneratorInfo(_persistentReferenceMemberInfo, _column), new ClassGeneratorInfo(_persistentReferenceMemberInfo.Owner, _dbTable)).Create();

        It should_create_an_association_attribute = () => {
            _persistentAssociationAttribute = _persistentReferenceMemberInfo.TypeAttributes.OfType<PersistentAssociationAttribute>().FirstOrDefault();
            _persistentAssociationAttribute.ShouldNotBeNull();
        };

        It should_have_as_association_the_name_of_the_foreignKey_that_this_column_belongs = () => _persistentAssociationAttribute.AssociationName.ShouldEqual("ReferenceMemberInfo-persistentClassInfoReferenceMemberInfoss");
    }

    public class When_column_is_primarykey_and_table_has_more_privae_keys : With_In_Memory_DataStore {
        static PersistentPersistentAttribute _persistentPersistentAttribute;
        static DBTable _dbTable;
        static DBColumn _column;

        static PersistentReferenceMemberInfo _persistentReferenceMemberInfo;

        Establish context = () => {
            _persistentReferenceMemberInfo = XPObjectSpace.CreateObject<PersistentReferenceMemberInfo>();
            var persistentClassInfo = XPObjectSpace.CreateObject<PersistentClassInfo>();
            persistentClassInfo.PersistentAssemblyInfo = XPObjectSpace.CreateObject<PersistentAssemblyInfo>();
            persistentClassInfo.SetDefaultTemplate(TemplateType.Class);
            _persistentReferenceMemberInfo.Owner = persistentClassInfo;
            var referenceClassInfo = XPObjectSpace.CreateObject<PersistentClassInfo>();
            referenceClassInfo.PersistentAssemblyInfo = persistentClassInfo.PersistentAssemblyInfo;
            referenceClassInfo.SetDefaultTemplate(TemplateType.Struct);
            _persistentReferenceMemberInfo.ReferenceClassInfo = referenceClassInfo;
            _persistentReferenceMemberInfo.SetDefaultTemplate(TemplateType.XPReadWritePropertyMember);
            _column = new DBColumn("col", false, "int", 0, DBColumnType.Int32);
            _dbTable = new DBTable();
            _dbTable.Columns.Add(_column);
            _dbTable.PrimaryKey = new DBPrimaryKey(new[] { _column, new DBColumn("col2", false, "int", 0, DBColumnType.Int32) });
        };

        Because of = () => new MemberAttributeGenerator(new MemberGeneratorInfo(_persistentReferenceMemberInfo, _column), new ClassGeneratorInfo(_persistentReferenceMemberInfo.Owner, _dbTable)).Create();

        It should_return_a_keyAttribute =
            () => _persistentReferenceMemberInfo.TypeAttributes.OfType<PersistentKeyAttribute>().FirstOrDefault().ShouldNotBeNull();

        It should_return_a_persistent_attribute =
            () => {
                _persistentPersistentAttribute = _persistentReferenceMemberInfo.TypeAttributes.OfType<PersistentPersistentAttribute>().FirstOrDefault();
                _persistentPersistentAttribute.ShouldNotBeNull();
            };

        It should_have_an_empty_mapto_field = () => _persistentPersistentAttribute.MapTo.ShouldBeEmpty();
    }


    public class When_column_is_combound_foreign_key : With_In_Memory_DataStore {
        static DBColumn _column;
        static DBTable _dbTable;
        static IPersistentPersistentAttribute _persistentPersistentAttribute;
        static PersistentReferenceMemberInfo _persistentReferenceMemberInfo;

        Establish context = () => {
            _column = new DBColumn("col", true, "int", 0, DBColumnType.Int32);
            _dbTable = new DBTable();
            var dbColumn = new DBColumn("col2", true, "int", 0, DBColumnType.Int32);
            _dbTable.Columns.AddRange(new[] { _column, dbColumn });
            _dbTable.ForeignKeys.Add(new DBForeignKey(new[] { _column, dbColumn }, "", new StringCollection()));


            _persistentReferenceMemberInfo = XPObjectSpace.CreateObject<PersistentReferenceMemberInfo>();
            var persistentClassInfo = XPObjectSpace.CreateObject<PersistentClassInfo>();
            persistentClassInfo.PersistentAssemblyInfo = XPObjectSpace.CreateObject<PersistentAssemblyInfo>();
            persistentClassInfo.SetDefaultTemplate(TemplateType.Class);
            _persistentReferenceMemberInfo.Owner = persistentClassInfo;
            _persistentReferenceMemberInfo.Owner.CodeTemplateInfo.CodeTemplate.TemplateType = TemplateType.Struct;
            _persistentReferenceMemberInfo.SetDefaultTemplate(TemplateType.XPReadWritePropertyMember);
            var referenceClassInfo = XPObjectSpace.CreateObject<PersistentClassInfo>();
            referenceClassInfo.PersistentAssemblyInfo = persistentClassInfo.PersistentAssemblyInfo;
            referenceClassInfo.SetDefaultTemplate(TemplateType.Class);
            _persistentReferenceMemberInfo.ReferenceClassInfo = referenceClassInfo;
        };

        Because of = () => new MemberAttributeGenerator(new MemberGeneratorInfo(_persistentReferenceMemberInfo, _column), new ClassGeneratorInfo(_persistentReferenceMemberInfo.Owner, _dbTable)).Create();

        It should_create_a_persitent_attrbiute = () => {
            _persistentPersistentAttribute =
                _persistentReferenceMemberInfo.TypeAttributes.OfType<IPersistentPersistentAttribute>().FirstOrDefault();
            _persistentPersistentAttribute.ShouldNotBeNull();
        };

        It should_map_it_to_an_empty_string = () => _persistentPersistentAttribute.MapTo.ShouldEqual(String.Empty);
    }

    public class When_owner_is_one_to_one : With_In_Memory_DataStore {
        static XPCollection<PersistentAttributeInfo> _persistentAttributeInfos;
        static PersistentReferenceMemberInfo _persistentMemberInfo;

        Establish context = () => {
            _persistentMemberInfo = XPObjectSpace.CreateObject<PersistentReferenceMemberInfo>();
            var persistentClassInfo = XPObjectSpace.CreateObject<PersistentClassInfo>();
            persistentClassInfo.PersistentAssemblyInfo = XPObjectSpace.CreateObject<PersistentAssemblyInfo>();
            persistentClassInfo.SetDefaultTemplate(TemplateType.Class);
            _persistentMemberInfo.Owner = persistentClassInfo;
            _persistentMemberInfo.ReferenceClassInfo = persistentClassInfo;
            _persistentMemberInfo.SetDefaultTemplate(TemplateType.XPOneToOnePropertyMember);
            _persistentAttributeInfos = _persistentMemberInfo.TypeAttributes;

            var dbColumn = new DBColumn();
            var dbTable = new DBTable();
            dbTable.ForeignKeys.Add(new DBForeignKey(new[] { dbColumn }, "RefTable", new StringCollection()));
        };

        Because of = () => new MemberAttributeGenerator(new MemberGeneratorInfo(_persistentMemberInfo, new DBColumn()), new ClassGeneratorInfo(_persistentMemberInfo.Owner, new DBTable())).Create();

        It should_not_create_any_attribute = () => _persistentAttributeInfos.OfType<IPersistentAssociationAttribute>().Count().ShouldEqual(0);
    }

    public class When_column_is_mapped : With_In_Memory_DataStore {
        static IPersistentPersistentAttribute _persistentPersistentAttribute;
        static DBColumn _column;
        static DBTable _dbTable;
        static PersistentCoreTypeMemberInfo _persistentCoreTypeMemberInfo;

        Establish context = () => {
            _persistentCoreTypeMemberInfo = XPObjectSpace.CreateObject<PersistentCoreTypeMemberInfo>();
            var persistentClassInfo = XPObjectSpace.CreateObject<PersistentClassInfo>();
            persistentClassInfo.PersistentAssemblyInfo = XPObjectSpace.CreateObject<PersistentAssemblyInfo>();
            persistentClassInfo.SetDefaultTemplate(TemplateType.Class);
            _persistentCoreTypeMemberInfo.Owner = persistentClassInfo;
            _persistentCoreTypeMemberInfo.SetDefaultTemplate(TemplateType.XPReadWritePropertyMember);
            _column = new DBColumn("col", false, "int", 0, DBColumnType.Int32);
            _dbTable = new DBTable();
            _dbTable.Columns.Add(_column);
        };

        Because of = () => new MemberAttributeGenerator(new MemberGeneratorInfo(_persistentCoreTypeMemberInfo, _column), new ClassGeneratorInfo(_persistentCoreTypeMemberInfo.Owner, _dbTable)).Create();

        It should_create_a_persistent_attribute = () => {
            _persistentPersistentAttribute =
                _persistentCoreTypeMemberInfo.TypeAttributes.OfType<IPersistentPersistentAttribute>().FirstOrDefault();
            _persistentPersistentAttribute.ShouldNotBeNull();
        };

        It should_map_it_to_column_name = () => _persistentPersistentAttribute.MapTo.ShouldEqual(_column.Name);
    }

    public class When__column_is_key_and_self_ref : With_Self_Ref_on_the_key {
        static MemberGeneratorInfo _memberGeneratorInfo;

        Establish context = () => {
            _memberGeneratorInfo = new MemberGenerator(ClassGeneratorHelper.DbTable, ClassGeneratorHelper.ClassGeneratorInfos).Create().FirstOrDefault();
        };

        Because of = () => new MemberAttributeGenerator(_memberGeneratorInfo, new ClassGeneratorInfo(_memberGeneratorInfo.PersistentMemberInfo.Owner, ClassGeneratorHelper.DbTable)).Create();

        It should_not_create_an_association_attribute =
            () =>
            _memberGeneratorInfo.PersistentMemberInfo.TypeAttributes.OfType<IPersistentAssociationAttribute>().
                FirstOrDefault().ShouldBeNull();
    }
}
