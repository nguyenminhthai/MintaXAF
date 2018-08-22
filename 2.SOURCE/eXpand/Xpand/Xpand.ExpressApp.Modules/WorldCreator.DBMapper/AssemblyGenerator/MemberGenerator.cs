using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo.DB;
using Xpand.ExpressApp.WorldCreator.BusinessObjects;
using Xpand.Persistent.Base.General;
using Xpand.Persistent.Base.PersistentMetaData;
using Xpand.Persistent.Base.PersistentMetaData.PersistentAttributeInfos;

namespace Xpand.ExpressApp.WorldCreator.DBMapper.AssemblyGenerator {
    public struct MemberGeneratorInfo {
        public MemberGeneratorInfo(IPersistentMemberInfo persistentMemberInfo, DBColumn dbColumn)
            : this() {
            PersistentMemberInfo = persistentMemberInfo;
            DbColumn = dbColumn;
        }

        public IPersistentMemberInfo PersistentMemberInfo { get; }

        public DBColumn DbColumn { get; }
    }
    public class MemberGenerator {
        readonly DBTable _dbTable;
        readonly Dictionary<string, ClassGeneratorInfo> _classInfos;
        readonly IObjectSpace _objectSpace;

        public MemberGenerator(DBTable dbTable, Dictionary<string, ClassGeneratorInfo> classInfos) {
            _dbTable = dbTable;
            _classInfos = classInfos;
            _objectSpace = XPObjectSpace.FindObjectSpaceByObject(classInfos[classInfos.Keys.First()].PersistentClassInfo);
        }

        public IEnumerable<MemberGeneratorInfo> Create() {
            var generatorInfos = CreateMembersCore(GetNonCompoundPKColumns());
            var coumboundPkColumn = GetCoumboundPKColumn();
            if (coumboundPkColumn != null) {
                string tableName = ClassGenerator.GetTableName(_dbTable.Name);
                var persistentClassInfo = _classInfos[tableName + ClassGenerator.KeyStruct].PersistentClassInfo;
                if (persistentClassInfo == null) throw new ArgumentNullException("persistentClassInfo with name " + _dbTable.Name + ClassGenerator.KeyStruct);
                var pkDbColumns = coumboundPkColumn.Columns.OfType<string>().Select(s => _dbTable.GetColumn(s)).ToArray();
                var membersCore = CreateMembersCore(pkDbColumns, persistentClassInfo, TemplateType.ReadWriteMember, TemplateType.ReadWriteMember).ToList();
                var persistentReferenceMemberInfo = CreatePersistentReferenceMemberInfo(persistentClassInfo.Name, _classInfos[tableName].PersistentClassInfo, persistentClassInfo, TemplateType.FieldMember);
                membersCore.Add(new MemberGeneratorInfo(persistentReferenceMemberInfo, _dbTable.GetColumn(coumboundPkColumn.Columns[0])));
                if (pkDbColumns.Where(IsOneToOneOnTheKey).Count() == pkDbColumns.Length) {
                    string refTableName = ClassGenerator.GetTableName(_dbTable.ForeignKeys.First(key => key.Columns.Contains(pkDbColumns.ToList()[0].Name)).PrimaryKeyTable);
                    ClassGeneratorInfo classGeneratorInfo = _classInfos[refTableName];
                    membersCore.Add(new MemberGeneratorInfo(CreatePersistentReferenceMemberInfo(classGeneratorInfo.PersistentClassInfo.Name, _classInfos[tableName].PersistentClassInfo, classGeneratorInfo.PersistentClassInfo, TemplateType.XPOneToOneReadOnlyPropertyMember), null));
                }
                return generatorInfos.Union(membersCore);
            }
            return generatorInfos;
        }


        IEnumerable<DBColumn> GetNonCompoundPKColumns() {
            var columns = _dbTable.PrimaryKey.Columns;
            return columns.Count > 1 ? _dbTable.Columns.Where(column => !columns.Contains(column.Name)) : _dbTable.Columns;
        }

        IEnumerable<MemberGeneratorInfo> CreateMembersCore(IEnumerable<DBColumn> dbColumns, IPersistentClassInfo persistentClassInfo = null, TemplateType coreTemplateType = TemplateType.XPReadWritePropertyMember, TemplateType refTemplateType = TemplateType.XPReadWritePropertyMember) {
            return dbColumns.SelectMany(dbColumn => {
                var memberGeneratorInfos = new List<MemberGeneratorInfo>();
                if (IsOneToOneOnTheKey(dbColumn) && coreTemplateType != TemplateType.ReadWriteMember) {
                    memberGeneratorInfos.Add(CreateFkMember(dbColumn, persistentClassInfo, coreTemplateType, TemplateType.XPOneToOneReadOnlyPropertyMember));
                } else {
                    memberGeneratorInfos.Add(CreateMember(dbColumn, persistentClassInfo, coreTemplateType, refTemplateType));
                }
                return memberGeneratorInfos;
            }).Where(info => info.PersistentMemberInfo != null);
        }

        bool IsOneToOneOnTheKey(DBColumn dbColumn) {
            IEnumerable<DBForeignKey> foreignPKeys = _dbTable.ForeignKeys.Where(key => _dbTable.PrimaryKey.Columns.Contains(dbColumn.Name) && key.PrimaryKeyTable != _dbTable.Name && key.Columns.Contains(dbColumn.Name));
            var keies = foreignPKeys.Select(key => new { FK = key, PrimaryTable = _classInfos[ClassGenerator.GetTableName(key.PrimaryKeyTable)].DbTable });
            return keies.Any(arg => arg.PrimaryTable.PrimaryKey.Columns.OfType<string>().All(s => arg.FK.PrimaryKeyTableKeyColumns.OfType<string>().Contains(s)));
        }

        MemberGeneratorInfo CreateMember(DBColumn dbColumn, IPersistentClassInfo persistentClassInfo = null, TemplateType coreTemplateType = TemplateType.XPReadWritePropertyMember, TemplateType refTemplateType = TemplateType.XPReadWritePropertyMember) {
            return CreateMemberCore(dbColumn, persistentClassInfo, coreTemplateType, refTemplateType);
        }


        MemberGeneratorInfo CreateMemberCore(DBColumn dbColumn, IPersistentClassInfo persistentClassInfo,
                                               TemplateType coreTemplateType, TemplateType refTemplateType) {
            bool isPrimaryKey = IsPrimaryKey(dbColumn);
            bool isFkColumn = IsFKey(dbColumn);
            var isOneToOneOnTheKey = IsOneToOneOnTheKey(dbColumn);
            var b = ((!isFkColumn) && (IsCoreColumn(dbColumn) || isPrimaryKey)) || (isOneToOneOnTheKey);
            if (b || IsSelfRefOnTheKey(dbColumn, isPrimaryKey)) {
                return new MemberGeneratorInfo(CreatePersistentCoreTypeMemberInfo(dbColumn, persistentClassInfo, coreTemplateType), dbColumn);
            }
            if (isFkColumn) {
                return CreateFkMember(dbColumn, persistentClassInfo, coreTemplateType, refTemplateType);
            }
            throw new NotImplementedException(dbColumn.Name);
        }

        bool IsSelfRefOnTheKey(DBColumn dbColumn, bool isPrimaryKey) {
            if (!isPrimaryKey)
                return false;
            return _dbTable.ForeignKeys.FirstOrDefault(
                key => key.PrimaryKeyTable == _dbTable.Name && key.Columns.Contains(dbColumn.Name)) != null;
        }

        MemberGeneratorInfo CreateFkMember(DBColumn dbColumn, IPersistentClassInfo persistentClassInfo,
                                           TemplateType coreTemplateType, TemplateType refTemplateType) {
            var foreignKey = _dbTable.ForeignKeys.Where(key => key.Columns.Contains(dbColumn.Name)).First(key => !Equals(key.PrimaryKeyTable, "XPObjectType"));

            var tableName = ClassGenerator.GetTableName(foreignKey.PrimaryKeyTable);
            if (_classInfos.ContainsKey(tableName)) {
                var classGeneratorInfo = _classInfos[tableName];
                return new MemberGeneratorInfo(CreatePersistentReferenceMemberInfo(dbColumn.Name, persistentClassInfo,
                                                            classGeneratorInfo.PersistentClassInfo, GetTemplateType(refTemplateType, classGeneratorInfo)), dbColumn);
            }
            return new MemberGeneratorInfo(CreatePersistentCoreTypeMemberInfo(dbColumn, persistentClassInfo, coreTemplateType), dbColumn);
        }


        TemplateType GetTemplateType(TemplateType refTemplateType, ClassGeneratorInfo classGeneratorInfo) {
            bool selfReference = classGeneratorInfo.DbTable.Name == _dbTable.Name;
            if (!selfReference) {
                DBForeignKey oneToOne = classGeneratorInfo.DbTable.ForeignKeys.FirstOrDefault(key => key.PrimaryKeyTable == _dbTable.Name);
                if (oneToOne != null)
                    return TemplateType.XPOneToOnePropertyMember;
            }
            return refTemplateType;
        }

        bool IsPrimaryKey(DBColumn dbColumn) {
            return _dbTable.PrimaryKey.Columns.Contains(dbColumn.Name);
        }

        bool IsFKey(DBColumn dbColumn) {
            return _dbTable.ForeignKeys.FirstOrDefault(key => key.Columns.Contains(dbColumn.Name)) != null;
        }

        static bool IsCoreColumn(DBColumn dbColumn) {
            return !dbColumn.IsIdentity && !dbColumn.IsKey;
        }

        DBPrimaryKey GetCoumboundPKColumn() {
            return _dbTable.PrimaryKey.Columns.Count > 1 ? _dbTable.PrimaryKey : null;
        }

        IPersistentMemberInfo CreatePersistentReferenceMemberInfo(string name, IPersistentClassInfo persistentClassInfo, IPersistentClassInfo persistentReferenceClassInfo, TemplateType templateType) {
            var persistentReferenceMemberInfo = _objectSpace.Create<IPersistentReferenceMemberInfo>();
            persistentReferenceMemberInfo.Name = name;
            persistentReferenceMemberInfo.ReferenceClassInfo = persistentReferenceClassInfo;
            if (persistentClassInfo == null)
                persistentClassInfo = _classInfos[ClassGenerator.GetTableName(_dbTable.Name)].PersistentClassInfo;
            persistentClassInfo.OwnMembers.Add(persistentReferenceMemberInfo);
            persistentReferenceMemberInfo.SetDefaultTemplate(templateType);
            if (templateType != TemplateType.XPOneToOnePropertyMember && templateType != TemplateType.XPOneToOneReadOnlyPropertyMember && persistentClassInfo.CodeTemplateInfo.CodeTemplate.TemplateType == TemplateType.Class && persistentReferenceClassInfo.CodeTemplateInfo.CodeTemplate.TemplateType == TemplateType.Class)
                CreateCollection(persistentReferenceMemberInfo, persistentClassInfo);
            return persistentReferenceMemberInfo;
        }

        void CreateCollection(IPersistentReferenceMemberInfo persistentReferenceMemberInfo, IPersistentClassInfo owner) {
            var persistentCollectionMemberInfo = _objectSpace.Create<IPersistentCollectionMemberInfo>();
            persistentReferenceMemberInfo.ReferenceClassInfo.OwnMembers.Add(persistentCollectionMemberInfo);
            persistentCollectionMemberInfo.Name = persistentReferenceMemberInfo.Owner.Name + persistentReferenceMemberInfo.Name + "s";
            persistentCollectionMemberInfo.Owner = persistentReferenceMemberInfo.ReferenceClassInfo;
            var tableName = ClassGenerator.GetTableName(Regex.Replace(owner.Name, ClassGenerator.KeyStruct, "", RegexOptions.Singleline | RegexOptions.IgnoreCase));
            persistentCollectionMemberInfo.CollectionClassInfo = owner.CodeTemplateInfo.CodeTemplate.TemplateType == TemplateType.Struct ? _classInfos[tableName].PersistentClassInfo : owner;
            persistentCollectionMemberInfo.SetDefaultTemplate(TemplateType.XPCollectionMember);
            var persistentAssociationAttribute = _objectSpace.Create<IPersistentAssociationAttribute>();
            persistentCollectionMemberInfo.TypeAttributes.Add(persistentAssociationAttribute);
            persistentAssociationAttribute.AssociationName =$"{persistentReferenceMemberInfo.Name}-{persistentCollectionMemberInfo.Name}s";
        }

        IPersistentCoreTypeMemberInfo CreatePersistentCoreTypeMemberInfo(DBColumn column, IPersistentClassInfo persistentClassInfo, TemplateType templateType) {
            var persistentCoreTypeMemberInfo = _objectSpace.Create<IPersistentCoreTypeMemberInfo>();
            persistentCoreTypeMemberInfo.Name = column.Name;
            persistentCoreTypeMemberInfo.DataType = column.ColumnType;
            if (persistentClassInfo == null) {
                var tableName = ClassGenerator.GetTableName(_dbTable.Name);
                persistentClassInfo = _classInfos[tableName].PersistentClassInfo;
            }
            persistentClassInfo.OwnMembers.Add(persistentCoreTypeMemberInfo);
            persistentCoreTypeMemberInfo.SetDefaultTemplate(templateType);
            return persistentCoreTypeMemberInfo;
        }

    }
}