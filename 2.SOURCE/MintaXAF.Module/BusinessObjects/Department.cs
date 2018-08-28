using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using MintaXAF.Module.BusinessObjects.Base;
using DevExpress.Persistent.Base.General;
using System.ComponentModel;

namespace MintaXAF.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("Dictionary")]
    [ImageName("BO_Department")]
    public class Department : MintaBaseObject,ITreeNode
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

        [Association("DepartmentParent-DepartmentChildren")]
        public Department DepartmentParent { get => GetPropertyValue<Department>("DepartmentParent"); set => SetPropertyValue("DepartmentParent", value); }

        [Association("DepartmentParent-DepartmentChildren")]
        public XPCollection<Department> DepartmentChildren => GetCollection<Department>("DepartmentChildren");

        [Browsable(false)]
        public string Name => Title;
        [Browsable(false)]
        ITreeNode ITreeNode.Parent => DepartmentParent as ITreeNode;
        [Browsable(false)]
        IBindingList ITreeNode.Children  =>DepartmentChildren as IBindingList;
        #region ListManagerDepartmentOid - string dùng trong Criteria để tìm các phòng cấp trên cho nhanh
        [Browsable(false)]
        [Size(1000)]
        public string ListManagerDepartmentOid
        {
            get; set;
        }

        public void updateListManagerDepartmentOid()
        {
            ListManagerDepartmentOid = getListManagerDepartmentOid();
        }
        //Đệ quy
        private string getListManagerDepartmentOid()
        {
            //string isUpdated = false;
            if (DepartmentParent == null) return Oid.ToString();
            return string.Format("{0},{1}", Oid.ToString(), DepartmentParent.getListManagerDepartmentOid());
        }
        #endregion

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        protected override void OnSaving()
        {
            // Cập nhật lại d/s các phòng ban trực thuộc
            this.updateListManagerDepartmentOid();
            base.OnSaving();
        }
    }
}