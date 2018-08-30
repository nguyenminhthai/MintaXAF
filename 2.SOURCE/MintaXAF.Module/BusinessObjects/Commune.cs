using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using MintaXAF.Module.BusinessObjects.Base;
using DevExpress.Persistent.Validation;

namespace MintaXAF.Module.BusinessObjects
{
    [DefaultClassOptions]    
    public class Commune : MintaBaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Commune(Session session)
            : base(session)
        {
        }
        [RuleUniqueValue(DefaultContexts.Save)]
        public string Code { get => GetPropertyValue<string>("Code"); set => SetPropertyValue("Code", value); }

        [PersistentAlias("Concat([Title],', ',[District].[FullNameWithPath])")]
        public string FullNameWithPath
        {
            get => (string)EvaluateAlias("FullNameWithPath");
        }

        public Province Province { get => GetPropertyValue<Province>("Province"); set => SetPropertyValue("Province", value); }

        [Association("District-Communes")]
        [DataSourceProperty("Province.Districts", DataSourcePropertyIsNullMode.SelectAll)]
        public District District { get => GetPropertyValue<District>("District"); set=> SetPropertyValue("District",value); }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        
    }
}