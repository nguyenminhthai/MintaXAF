using System;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using MintaXAF.Module.BusinessObjects.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.ExpressApp.Filtering;
using System.ComponentModel;

namespace MintaXAF.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("Dictionary")]
    [ImageName("BO_Employee")]
    public class Employee : MintaBaseObject, IPerson
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Employee(Session session)
            : base(session)
        {
        }
        public override string Title { get => FullName; set => base.Title = FullName; }

        #region Person
        public string FirstName { get => GetPropertyValue<string>("FirstName"); set => SetPropertyValue("FirstName", value); }
        public string LastName { get => GetPropertyValue<string>("LastName"); set => SetPropertyValue("LastName", value); }
        public string MiddleName { get => GetPropertyValue<string>("MiddleName"); set => SetPropertyValue("MiddleName", value); }
        public DateTime Birthday { get => GetPropertyValue<DateTime>("Birthday"); set => SetPropertyValue("Birthday", value); }
        [PersistentAlias("Concat([FirstName],' ',[MiddleName],' ',[LastName])")]
        [SearchMemberOptions(SearchMemberMode.Include)]
        public string FullName
        {
            get => (string)EvaluateAlias("FullName");
        }
        public void SetFullName(string fullName)
        {
            Title = fullName;
        }
        #endregion

        public Sex Sex { get => GetPropertyValue<Sex>("Sex"); set => SetPropertyValue("Sex", value); }

        [ImageEditor(ListViewImageEditorCustomHeight = 75, DetailViewImageEditorFixedHeight = 150)]
        public byte[] Photo { get; set; }


        [RuleRegularExpression(DefaultContexts.Save, EmailRegularExpression, CustomMessageTemplate = "Invalid email format!")]
        public string Email { get => GetPropertyValue<string>("Email"); set => SetPropertyValue("Email", value); }

        [Association("Department-Employees"), ImmediatePostData]
        [RuleRequiredField(DefaultContexts.Save)]
        public Department Department { get => GetPropertyValue<Department>("Department"); set => SetPropertyValue("Department", value); }

        [DataSourceProperty("Department.PositionCollection")]
        public Position Position { get => GetPropertyValue<Position>("Position"); set => SetPropertyValue("Position", value); }

        [Size(4096)]
        public string Notes { get => GetPropertyValue<string>("Notes"); set => SetPropertyValue("Notes", value); }

        [DataSourceProperty("Department.EmployeeCollection")]
        [DataSourceCriteria("Oid != '@This.Oid'")]
        [Association("Manager-Employee")]        
        public Employee Manager { get => GetPropertyValue<Employee>("Manager"); set => SetPropertyValue("Manager", value); }

        [Association("Manager-Employee")]        
        public XPCollection<Employee> JuniorEmployees
        {
            get { return GetCollection<Employee>("JuniorEmployees"); }
        }
        
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
    }
}