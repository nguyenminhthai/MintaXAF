﻿using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using Xpand.Persistent.Base;
using Xpand.Persistent.Base.PersistentMetaData;
using DevExpress.ExpressApp.Model;

namespace Xpand.Persistent.BaseImpl.PersistentMetaData {
    [InterfaceRegistrator(typeof(IDataStoreLogonObject))]
    [Appearance("DataStoreLogonObject_UserName", AppearanceItemType.ViewItem, "Authentication=0", Enabled = false, TargetItems = "UserName,PassWord")]
    [NonPersistent]
    public class DataStoreLogonObject : XpandBaseCustomObject, IDataStoreLogonObject {
        public DataStoreLogonObject(Session session)
            : base(session) {
        }

        public DataStoreLogonObject(Session session, DataStoreLogonObject sqlMapperInfo)
            : base(session) {
            foreach (XPMemberInfo memberInfo in ClassInfo.OwnMembers) {
                memberInfo.SetValue(this, memberInfo.GetValue(sqlMapperInfo));
            }
        }


        private string _serverName;
        [Index(0)]
        public string ServerName {
            get { return _serverName; }
            set { SetPropertyValue("ServerName", ref _serverName, value); }
        }
        private DataStoreAuthentication _authentication;
        [Index(1)]
        public DataStoreAuthentication Authentication {
            get { return _authentication; }
            set { SetPropertyValue("Authentication", ref _authentication, value); }
        }

        public override string ToString() {
            return this.GetConnectionString();
        }
        private string _userName;
        [Index(2)]
        public string UserName {
            get { return _userName; }
            set { SetPropertyValue("UserName", ref _userName, value); }
        }
        [Index(4)]
        [DataSourceProperty("DataBases")]
        public DataBase DataBase {
            get { return _dataBase; }
            set { SetPropertyValue("DataStore", ref _dataBase, value); }
        }
        IDataBase IDataStoreLogonObject.DataBase {
            get { return DataBase; }
            set { DataBase = value as DataBase; }
        }

        private DataBase _dataBase;
        readonly IList<IDataBase> _dataBases = new List<IDataBase>();


        private string _passWord;
        [Index(3)]
        [ModelDefault("IsPassword", "True")]
        public string PassWord {
            get { return _passWord; }
            set { SetPropertyValue("PassWord", ref _passWord, value); }
        }
        [Browsable(false)]
        public IList<IDataBase> DataBases {
            get { return _dataBases; }
        }
    }
}
