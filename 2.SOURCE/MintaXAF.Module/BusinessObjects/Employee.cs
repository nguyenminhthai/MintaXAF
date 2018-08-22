using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using MintaXAF.Module.BusinessObjects.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.ExpressApp.Filtering;

namespace MintaXAF.Module.BusinessObjects
{
    [DefaultClassOptions]    
    [NavigationItem("Dictionary")]
    [ImageName("BO_Employee")]
    public class Employee : MintaBaseObject,IPerson
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Employee(Session session)
            : base(session)
        {
        }
        public override string Title { get => FullName; set => base.Title = FullName; }
        public string FirstName { get => GetPropertyValue<string>("FirstName"); set => SetPropertyValue("FirstName", value); }
        public string LastName { get => GetPropertyValue<string>("LastName"); set => SetPropertyValue("LastName", value); }
        public string MiddleName { get => GetPropertyValue<string>("MiddleName"); set => SetPropertyValue("MiddleName", value);  }
        public DateTime Birthday { get => GetPropertyValue<DateTime>("Birthday"); set => SetPropertyValue("Birthday", value); }

        [PersistentAlias("Concat([FirstName],' ',[MiddleName],' ',[LastName])")]
        [SearchMemberOptions(SearchMemberMode.Include)]
        public string FullName
        {
            get => (string)EvaluateAlias("FullName");
        }

        [RuleRegularExpression(DefaultContexts.Save, EmailRegularExpression, CustomMessageTemplate = "Invalid email format!")]
        public string Email { get => GetPropertyValue<string>("Email"); set => SetPropertyValue("Email", value); }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        public void SetFullName(string fullName)
        {
            Title = fullName;
        }
    }
}