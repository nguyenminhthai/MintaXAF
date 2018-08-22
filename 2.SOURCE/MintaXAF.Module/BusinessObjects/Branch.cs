using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Validation;
using MintaXAF.Module.BusinessObjects.Base;

namespace MintaXAF.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("Dictionary")]
    public class Branch : MintaBaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Branch(Session session)
            : base(session)
        {
        }
        public string Address { get=>GetPropertyValue<string>("Address"); set=>SetPropertyValue("Address",value); }

        [ModelDefault("EditMask", "(000)0000-0000")]
        public string Telephone { get => GetPropertyValue<string>("Telephone"); set => SetPropertyValue("Telephone", value); }

        [ModelDefault("EditMask", "(000)0000-0000")]
        public string Fax { get => GetPropertyValue<string>("Fax"); set => SetPropertyValue("Fax", value); }

        [RuleRegularExpression(DefaultContexts.Save, EmailRegularExpression, CustomMessageTemplate = "Invalid email format!")]
        public string Email { get => GetPropertyValue<string>("Email"); set => SetPropertyValue("Email", value); }

        [Association("Department-Branch")]
        public XPCollection<Department> DepartmentCollection => GetCollection<Department>("DepartmentCollection");

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
       
    }
}