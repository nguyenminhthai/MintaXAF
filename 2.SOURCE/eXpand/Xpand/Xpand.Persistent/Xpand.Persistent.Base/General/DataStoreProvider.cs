﻿using System;
using System.Configuration;
using System.Web;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;
using Xpand.Xpo.DB;

namespace Xpand.Persistent.Base.General {
    public class DataStoreProvider : IXpoDataStoreProxy {
        private DataStoreProxy _proxy;
        private string _connectionString;
        public DataStoreProvider(string connectionString) {
            _connectionString = connectionString;
        }

        public DataStoreProvider(IDataStore dataStore) {
            _proxy = new DataStoreProxy(dataStore);
            var connectionProviderSql = dataStore as ConnectionProviderSql;
            if (connectionProviderSql!=null)
                _connectionString = connectionProviderSql.ConnectionString;
        }

        public IDataStore CreateSchemaCheckingStore(out IDisposable[] disposableObjects){
            disposableObjects = null;
            return XpoDefault.GetConnectionProvider(_connectionString, AutoCreateOption.None);
        }

        public string ConnectionString {
            get { return _connectionString; }
            set { _connectionString = value; }
        }

        public IDataStore CreateWorkingStore(out IDisposable[] disposableObjects) {
            disposableObjects = null;
            if ((ConfigurationManager.AppSettings["DataCache"] + "").Contains("Client")) {
                var cacheNode = HttpContext.Current.Application["DataStore"] as DataCacheNode;
                if (cacheNode == null) {
                    var cacheRoot = new DataCacheRoot(Proxy.DataStore);
                    cacheNode = new DataCacheNode(cacheRoot);
                }
                return cacheNode;
            }
            return Proxy;
        }

        public virtual IDataStore CreateUpdatingStore(bool allowUpdateSchema, out IDisposable[] disposableObjects){
            disposableObjects = null;
            return new DataStoreProxy(XpoDefault.GetConnectionProvider(_connectionString, AutoCreateOption.DatabaseAndSchema));
        }

        public XPDictionary XPDictionary => null;

        public virtual DataStoreProxy Proxy => _proxy ??(_proxy =new DataStoreProxy(XpoDefault.GetConnectionProvider(_connectionString, AutoCreateOption.None)));
    }
}