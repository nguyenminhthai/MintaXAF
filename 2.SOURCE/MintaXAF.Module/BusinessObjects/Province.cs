using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using MintaXAF.Module.BusinessObjects.Base;
using DevExpress.Persistent.Validation;

namespace MintaXAF.Module.BusinessObjects
{
    [DefaultClassOptions]    
    public class Province : MintaBaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Province(Session session)
            : base(session)
        {
        }
        [RuleUniqueValue(DefaultContexts.Save)]
        public string Code { get => GetPropertyValue<string>("Code"); set => SetPropertyValue("Code", value); }

        [Association("Province-Districts"),Aggregated]
        public XPCollection<District> Districts
        {
            get => GetCollection<District>("Districts");
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }       
    }
}