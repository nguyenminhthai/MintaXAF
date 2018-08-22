using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Xpand.Persistent.Base;
using Xpand.Persistent.Base.General;
using Xpand.Persistent.Base.PersistentMetaData;

namespace Xpand.Persistent.BaseImpl.PersistentMetaData {
    [InterfaceRegistrator(typeof(ITemplateInfo))]
    public class TemplateInfo : XpandBaseCustomObject, ITemplateInfo {
        string _name;
        string _templateCode;

        public TemplateInfo(Session session)
            : base(session) {
        }
        private PersistentTypeInfo _persistentTypeInfo;
        [Browsable(false)]
        [Association("PersistentTypeInfo-TemplateInfos")]
        public PersistentTypeInfo PersistentTypeInfo {
            get { return _persistentTypeInfo; }
            set { SetPropertyValue("PersistentTypeInfo", ref _persistentTypeInfo, value); }
        }


        [Association("TemplateInfo-CodeTemplateInfos")]
        public XPCollection<CodeTemplateInfo> CodeTemplateInfos => GetCollection<CodeTemplateInfo>("CodeTemplateInfos");

        #region ITemplateInfo Members
        [RuleRequiredField(null, DefaultContexts.Save)]
        public string Name {
            get { return _name; }
            set { SetPropertyValue("Name", ref _name, value); }
        }

        [Size(SizeAttribute.Unlimited)]
        [EditorAlias(EditorAliases.CSCodePropertyEditor)]
        public string TemplateCode {
            get { return _templateCode; }
            set { SetPropertyValue("TemplateCode", ref _templateCode, value); }
        }
        #endregion
    }


}