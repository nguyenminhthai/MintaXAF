﻿using System;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo.DB;
using Xpand.Persistent.Base.General;

namespace Xpand.Persistent.Base.Xpo{
	public class CachedDataStoreProvider : ConnectionStringDataStoreProvider, IXpoDataStoreProvider{
	    private static readonly Lazy<CachedDataStoreProvider> Lazy =
	        new Lazy<CachedDataStoreProvider>(() => Factory()??new CachedDataStoreProvider(XpandModuleBase.ConnectionString));

	    public static Func<CachedDataStoreProvider> Factory{ get; set; }

	    private static IDisposable[] _rootDisposableObjects;
		private static DataCacheRoot _root;

	    public static CachedDataStoreProvider Instance => Lazy.Value;

	    public CachedDataStoreProvider(string connectionString) : base(connectionString){
		}

		IDataStore IXpoDataStoreProvider.CreateWorkingStore(out IDisposable[] disposableObjects){
			if (_root == null){
				var baseDataStore = base.CreateWorkingStore(out _rootDisposableObjects);
				_root = new DataCacheRoot(baseDataStore);
			}

			disposableObjects = new IDisposable[0];
			return new DataCacheNode(_root);
		}

		public static void ResetDataCacheRoot(){
			_root = null;
			if (_rootDisposableObjects != null){
				foreach (var disposableObject in _rootDisposableObjects) disposableObject.Dispose();
				_rootDisposableObjects = null;
			}
		}
	}
}