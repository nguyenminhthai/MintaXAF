﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;
using DevExpress.Xpo.Metadata.Helpers;
using Xpand.Xpo;
using Xpand.Xpo.DB;

namespace Xpand.Persistent.Base.General {
    public class MultiDataStoreProxy : DataStoreProxy {
        readonly XpoObjectHacker _xpoObjectHacker = new XpoObjectHacker();
        readonly DataStoreManager _dataStoreManager;

        public MultiDataStoreProxy(IDataStore dataStore, string connectionString,XPDictionary dictionary=null) : base(dataStore){
            if (dictionary==null)
                dictionary=XpoTypesInfoHelper.GetXpoTypeInfoSource().XPDictionary;

            _dataStoreManager = new DataStoreManager(connectionString);
            FillDictionaries(dictionary);
        }

        public MultiDataStoreProxy(string connectionString,AutoCreateOption autoCreateOption=AutoCreateOption.None):this(XpoDefault.GetConnectionProvider(connectionString, autoCreateOption),connectionString ){
            
        }

        public MultiDataStoreProxy(string connectionString,XPDictionary dictionary,AutoCreateOption  option=AutoCreateOption.None):this(XpoDefault.GetConnectionProvider(connectionString, option),connectionString,dictionary ){
            
        }

        public override void Init(){
            FillDictionaries(XpoTypesInfoHelper.GetXpoTypeInfoSource().XPDictionary);
        }

        public DataStoreManager DataStoreManager => _dataStoreManager;

        void FillDictionaries(XPDictionary xpDictionary) {
            foreach (XPClassInfo queryClassInfo in xpDictionary.Classes.OfType<XPClassInfo>().Where(info => !(info is IntermediateClassInfo))) {
                ReflectionDictionary reflectionDictionary = _dataStoreManager.GetDictionary(queryClassInfo);
                reflectionDictionary.QueryClassInfo(queryClassInfo.ClassType);
            }
        }

        public override ModificationResult ModifyData(params ModificationStatement[] dmlStatements){
            var dataStoreModifyDataEventArgs = new DataStoreModifyDataEventArgs(dmlStatements);
            OnDataStoreModifyData(dataStoreModifyDataEventArgs);
            var name = typeof(XPObjectType).Name;
            var insertStatement = dataStoreModifyDataEventArgs.ModificationStatements.OfType<InsertStatement>().FirstOrDefault(statement => statement.Table.Name == name);
            var modificationResult = new ModificationResult();
            if (insertStatement != null) {
                modificationResult = ModifyXPObjectTable(dmlStatements, insertStatement, modificationResult);
            } else {
                var key = _dataStoreManager.GetKey(dmlStatements[0].Table.Name);
                modificationResult = _dataStoreManager.GetDataLayer(key,DataStore).ModifyData(dmlStatements);
            }
            if (modificationResult != null) return modificationResult;
            throw new NotImplementedException();
        }

        ModificationResult ModifyXPObjectTable(ModificationStatement[] dmlStatements, InsertStatement insertStatement, ModificationResult modificationResult) {
            foreach (var simpleDataLayer in _dataStoreManager.GetDataLayers(DataStore)) {
                if (!simpleDataLayer.Value.IsLegacy) {
                    var dataLayer = simpleDataLayer.Value;
                    if (!TypeExists(dataLayer, insertStatement)) {
                        if (!dataLayer.IsMainLayer) {
                            _xpoObjectHacker.CreateObjectTypeIndetifier(insertStatement, _dataStoreManager.GetDataLayer(DataStoreManager.DefaultDictionaryKey,DataStore));
                        }
                        var modifyData = dataLayer.ModifyData(dmlStatements);
                        if (modifyData.Identities.Any())
                            modificationResult = modifyData;
                    }
                }
            }
            return modificationResult;
        }


        bool TypeExists(DataStoreManagerSimpleDataLayer dataLayer, InsertStatement stm1) {
            if (dataLayer.IsMainLayer)
                return false;
            var session = new Session(dataLayer) { IdentityMapBehavior = IdentityMapBehavior.Strong };
            var value = stm1.Parameters.ToList()[0].Value as string;
            var xpObjectType = session.FindObject<XPObjectType>(type => type.TypeName == value);
            return xpObjectType != null;
        }

        bool IsMainLayer(IDbConnection connection) {
            return (connection == null || Connection == null) || connection.ConnectionString == Connection.ConnectionString;
        }

        public override SelectedData SelectData(params SelectStatement[] selects) {
            var resultSet = new List<SelectStatementResult>();
            List<SelectedData> selectedDatas = selects.Select(stm => {
                OnDataStoreSelectData(new DataStoreSelectDataEventArgs(new[] { stm }));
                var simpleDataLayer = _dataStoreManager.GetDataLayer(_dataStoreManager.GetKey(stm.Table.Name),DataStore);
                return simpleDataLayer.SelectData(stm);
            }).ToList();
            foreach (SelectedData selectedData in selectedDatas.Where(
                selectedData => selectedData != null)) {
                resultSet.AddRange(selectedData.ResultSet);
            }
            return new SelectedData(resultSet.ToArray());
        }

        public override UpdateSchemaResult UpdateSchema(bool dontCreateIfFirstTableNotExist, params DBTable[] tables) {
            foreach (KeyValuePair<IDataStore, DataStoreInfo> keyValuePair in _dataStoreManager.GetDataStores(tables, DataStore)) {
                var store = keyValuePair.Key as ConnectionProviderSql;
                if (store != null) {
                    var dataStoreInfo = keyValuePair.Value;
                    var storeInfo = dataStoreInfo;
                    var dbTables = storeInfo.DbTables;
                    if (Connection == null)
                        throw new NullReferenceException();
                    if (!storeInfo.IsLegacy && !IsMainLayer(store.Connection))
                        _xpoObjectHacker.EnsureIsNotIdentity(dbTables);
                    if (storeInfo.IsLegacy){
                        var dbTable = dbTables.FirstOrDefault(table => table.Name == typeof(XPObjectType).Name);
                        dbTables.Remove(dbTable);
                    }
                    store.UpdateSchema(false, dbTables.ToArray());
                    RunExtraUpdaters(tables, store, dontCreateIfFirstTableNotExist);
                }
            }
            return UpdateSchemaResult.SchemaExists;
        }

        void RunExtraUpdaters(DBTable[] tables, ConnectionProviderSql store, bool dontCreateIfFirstTableNotExist) {
            foreach (var schemaUpdater in SchemaUpdaters) {
                schemaUpdater.Update(store, new DataStoreUpdateSchemaEventArgs(dontCreateIfFirstTableNotExist, tables));
            }
        }
    }
}