using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Xpand.ExpressApp.WorldCreator.CodeProvider;
using Xpand.Persistent.Base.General;
using Xpand.Persistent.Base.PersistentMetaData;

namespace Xpand.Persistent.BaseImpl.PersistentMetaData {
    [RuleCombinationOfPropertiesIsUnique(null, DefaultContexts.Save, "Owner,Name")]
    public abstract class PersistentMemberInfo : PersistentTemplatedTypeInfo, IPersistentMemberInfo {
        PersistentClassInfo _owner;


        protected PersistentMemberInfo(Session session)
            : base(session) {
        }
        [VisibleInDetailView(false)]
        [VisibleInListView(true)]
        [ModelDefault("GroupIndex", "0")]
        public string TypeInfoName => GetType().Name.Replace("Persistent", "");

        [VisibleInListView(false)]
        [ModelDefault("AllowEdit", "false")]
        [Size(SizeAttribute.Unlimited)]
        [EditorAlias(EditorAliases.CSCodePropertyEditor)]
        public string GeneratedCode => this.GenerateCode();

        [Association("PersistentClassInfo-OwnMembers")]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public PersistentClassInfo Owner {
            get { return _owner; }
            set { SetPropertyValue("Owner", ref _owner, value); }
        }
        #region IPersistentMemberInfo Members
        IPersistentClassInfo IPersistentMemberInfo.Owner {
            get { return _owner; }
            set { _owner = value as PersistentClassInfo; }
        }
        #endregion
    }
}