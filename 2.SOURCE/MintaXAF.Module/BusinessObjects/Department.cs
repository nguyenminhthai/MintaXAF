﻿using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using MintaXAF.Module.BusinessObjects.Base;

namespace MintaXAF.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("Dictionary")]
    [ImageName("BO_Department")]
    public class Department : MintaBaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Department(Session session)
            : base(session)
        {
        }

        [Association("Department-Branch")]
        public XPCollection<Branch> BranchCollection => GetCollection<Branch>("BranchCollection");

        [Association("Department-Employees")]
        public XPCollection<Employee> EmployeeCollection => GetCollection<Employee>("EmployeeCollection");

        [Association("Departments-Positions")]
        public XPCollection<Position> PositionCollection => GetCollection<Position>("PositionCollection");

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
    }
}