using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using MintaXAF.Module.BusinessObjects.Base;

namespace MintaXAF.Module.BusinessObjects
{
    [DefaultClassOptions]    
    public class District : MintaBaseObject
    {
        public District(Session session) : base(session)
        {
        }
        [RuleUniqueValue(DefaultContexts.Save)]
        public string Code { get => GetPropertyValue<string>("Code"); set => SetPropertyValue("Code", value); }

        [PersistentAlias("Concat([Title],', ',[Province].[Title])")]
        public string FullNameWithPath
        {
            get => (string)EvaluateAlias("FullNameWithPath");
        }
        [Association("Province-Districts")]
        public Province Province
        {
            get => GetPropertyValue<Province>("Province");
            set => SetPropertyValue("Province", value);
        }
        [Association("District-Communes"), Aggregated]
        public XPCollection<Commune> Communes => GetCollection<Commune>("Communes");
    }   

}