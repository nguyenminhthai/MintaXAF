﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;

namespace Xpand.Xpo.DB {
    public class DataStoreInfo {
        public List<DBTable> DbTables{ get; } = new List<DBTable>();

        public bool IsLegacy{ get; set; } = true;
    }

    public struct KeyInfo {
        public KeyInfo(bool isLegacy, string key) {
            IsLegacy = isLegacy;
            Key = key;
        }

        public bool IsLegacy{ get; }

        public string Key{ get; }
    }
    public class DataStoreManager {
        public const string DefaultDictionaryKey = "Default";
        readonly Dictionary<KeyInfo, ReflectionDictionary> _reflectionDictionaries = new Dictionary<KeyInfo, ReflectionDictionary>();
        readonly Dictionary<string, DataStoreManagerSimpleDataLayer> _simpleDataLayers = new Dictionary<string, DataStoreManagerSimpleDataLayer>();
        readonly Dictionary<string, List<string>> _tables = new Dictionary<string, List<string>>();
        readonly string _connectionString;
        readonly DataStoreAttribute[] _dataStoreAttributes;
        private bool _dataLayersCreated;

        public DataStoreManager(string connectionString) {
            _connectionString = connectionString;
            _dataStoreAttributes = GetDataStoreAttributes().ToArray();
        }

        public KeyInfo GetKeyInfo(Type type) {
            var nameSpace = (type.Namespace + "");
            var dataStoreAttribute = _dataStoreAttributes.FirstOrDefault(attribute => nameSpace.StartsWith(attribute.NameSpace));
            return dataStoreAttribute == null ? new KeyInfo(false, DefaultDictionaryKey) : new KeyInfo(dataStoreAttribute.IsLegacy, (dataStoreAttribute.DataStoreName ?? dataStoreAttribute.ConnectionString));
        }

        KeyInfo GetKeyInfo(XPClassInfo xpClassInfo) {
            return GetKeyInfo(xpClassInfo.ClassType);
        }

        void AddTableNames(XPClassInfo xpClassInfo, string key) {
            List<string> list = _tables[key];
            list.Add(xpClassInfo.TableName);
            foreach (var tableName in GetIntermediateTableNames(xpClassInfo).Where(tableName => !list.Contains(tableName))) {
                list.Add(tableName);
            }
        }

        IEnumerable<string> GetIntermediateTableNames(XPClassInfo classInfo) {
            return classInfo.CollectionProperties.OfType<XPMemberInfo>().Where(info => info.IntermediateClass != null).Select(memberInfo => memberInfo.IntermediateClass.TableName);
        }
        public ReflectionDictionary GetDictionary(Type type) {
            XPClassInfo xpClassInfo = GetXPClassInfo(type);
            return GetDictionary(xpClassInfo);
        }

        public Dictionary<KeyInfo, ReflectionDictionary> ReflectionDictionaries => _reflectionDictionaries;

        XPClassInfo GetXPClassInfo(Type type) {
            var xpClassInfos = _reflectionDictionaries.Select(pair => pair.Value).SelectMany(dictionary => dictionary.Classes.OfType<XPClassInfo>());
            return xpClassInfos.First(info => info.ClassType == type);
        }

        public ReflectionDictionary GetDictionary(XPClassInfo xpClassInfo) {
            KeyInfo keyInfo = GetKeyInfo(xpClassInfo);
            var reflectionDictionary = GetDictionary(keyInfo);
            if (xpClassInfo.IsPersistent)
                AddTableNames(xpClassInfo, keyInfo.Key);
            return reflectionDictionary;
        }

        ReflectionDictionary GetDictionary(KeyInfo keyInfo) {
            if (!_reflectionDictionaries.ContainsKey(keyInfo)) {
                var reflectionDictionary = new ReflectionDictionary();
                _reflectionDictionaries.Add(keyInfo, reflectionDictionary);
                _tables.Add(keyInfo.Key, new List<string>());
            }
            return _reflectionDictionaries[keyInfo];
        }

        public IDataStore GetConnectionProvider(Type type) {
            return GetConnectionProvider(GetKeyInfo(type).Key);
        }

        public string GetConnectionString(Type type) {
            string key = GetKeyInfo(type).Key;
            return GetConnectionString(key);
        }

        public string GetConnectionString(string key) {
            if (key == DefaultDictionaryKey)
                return _connectionString;
            if (key.StartsWith(DataStoreBase.XpoProviderTypeParameterName))
                return key;
            ConnectionStringSettings connectionStringSettings =
                ConfigurationManager.ConnectionStrings[$"{key}ConnectionString"];
            if (connectionStringSettings != null) {
                return connectionStringSettings.ConnectionString;
            }
            IDataStore connectionProvider = XpoDefault.GetConnectionProvider(_connectionString, AutoCreateOption.DatabaseAndSchema);
            var sql = connectionProvider as ConnectionProviderSql;
            if (sql != null) {
                IDbConnection dbConnection = sql.Connection;
                return _connectionString?.Replace(dbConnection.Database, $"{dbConnection.Database}{key}.mdb") ?? AccessConnectionProvider.GetConnectionString(key);

            }
            throw new NoNullAllowedException($"{key}ConnectionString not found ");
        }

        public IDataStore GetConnectionProvider(string key) {
            string connectionString = GetConnectionString(key);
            return XpoDefault.GetConnectionProvider(connectionString, AutoCreateOption.DatabaseAndSchema);
        }

        public static IEnumerable<DataStoreAttribute> GetDataStoreAttributes(string dataStoreName) {
            var dataStoreAttributes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assmb => assmb.GetCustomAttributes(typeof(Attribute), false).OfType<DataStoreAttribute>());
            return dataStoreAttributes.Where(attribute => attribute.DataStoreName.Equals(dataStoreName, StringComparison.Ordinal));
        }

        public IEnumerable<DataStoreAttribute> GetDataStoreAttributes(){
            var selectMany = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly =>
                assembly.GetCustomAttributes(typeof (DataStoreAttribute), false).Cast<DataStoreAttribute>(),
                (assembly, dataStoreAttribute) => new{assembly, dataStoreAttribute});
            return selectMany.Where(t =>t.dataStoreAttribute.ConnectionString != null ||
                            ConfigurationManager.ConnectionStrings[$"{t.dataStoreAttribute.DataStoreName}ConnectionString"] != null)
                    .Select(t => t.dataStoreAttribute);
        }

        public Dictionary<string, DataStoreManagerSimpleDataLayer> GetDataLayers(IDataStore defaultStore) {
            if (!_dataLayersCreated) {
                _dataLayersCreated = true;
                foreach (var dictionary in _reflectionDictionaries) {
                    var keyInfo = dictionary.Key;
                    GetDataLayer(keyInfo.Key,defaultStore);
                }
            }
            return _simpleDataLayers;

        }

        public DataStoreManagerSimpleDataLayer GetDataLayer(string key,IDataStore defaultStore){
            if (!_simpleDataLayers.ContainsKey(key)){
                var keyValuePair = _reflectionDictionaries.First(info => info.Key.Key==key);
                var connectionProvider = keyValuePair.Key.Key==DefaultDictionaryKey?defaultStore:GetConnectionProvider(keyValuePair.Key.Key);
                var simpleDataLayer = new DataStoreManagerSimpleDataLayer(keyValuePair.Value, connectionProvider,
                    keyValuePair.Key.Key == DefaultDictionaryKey, keyValuePair.Key.IsLegacy);
                _simpleDataLayers.Add(keyValuePair.Key.Key, simpleDataLayer);
            }
            return _simpleDataLayers[key];
        }

        public Dictionary<IDataStore, DataStoreInfo> GetDataStores(DBTable[] dbTables, IDataStore dataStore) {
            Dictionary<IDataStore, DataStoreInfo> dataStoreInfos =
                GetDataLayers(dataStore).ToDictionary(keyValuePair => keyValuePair.Value.ConnectionProvider, keyValuePair =>
                                               new DataStoreInfo { IsLegacy = keyValuePair.Value.IsLegacy });
            foreach (var dbTable in dbTables) {
                if (dbTable.Name == "XPObjectType") {
                    foreach (var simpleDataLayer in GetDataLayers(dataStore)) {
                        var dataStoreInfo = dataStoreInfos[simpleDataLayer.Value.ConnectionProvider];
                        dataStoreInfo.DbTables.Add(dbTable);
                    }
                } else {
                    var key = GetKey(dbTable.Name);
                    var dataStoreManagerSimpleDataLayer = GetDataLayer(key,dataStore);
                    var dataStoreInfo = dataStoreInfos[dataStoreManagerSimpleDataLayer.ConnectionProvider];
                    dataStoreInfo.DbTables.Add(dbTable);
                }
            }
            return dataStoreInfos;
        }

        public string GetKey(string tableName) {
            if (tableName == typeof(XPObjectType).Name)
                return DefaultDictionaryKey;
            var keyValuePairs = _tables.Where(valuePair => valuePair.Value.Contains(tableName)).ToList();
            string key = DefaultDictionaryKey;
            if (keyValuePairs.Any())
                key = keyValuePairs[0].Key;
            return key;
        }

        public Type GetType(string typeName) {
            var types = _reflectionDictionaries.Select(pair => pair.Value).SelectMany(dictionary => dictionary.Classes.OfType<XPClassInfo>()).Select(classInfo => classInfo.ClassType);
            return types.FirstOrDefault(type => type.Name == typeName);
        }

    }

    public class DataStoreManagerSimpleDataLayer : SimpleDataLayer {
        public DataStoreManagerSimpleDataLayer(XPDictionary dictionary, IDataStore provider, bool isMainLayer, bool isLegacy)
            : base(dictionary, provider) {
            IsMainLayer = isMainLayer;
            IsLegacy = isLegacy;
        }

        public bool IsLegacy{ get; }

        public bool IsMainLayer{ get; }
    }
}