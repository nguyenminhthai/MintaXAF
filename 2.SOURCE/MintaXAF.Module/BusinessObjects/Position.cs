using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using MintaXAF.Module.BusinessObjects.Base;

namespace MintaXAF.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("Dictionary")]
    [ImageName("BO_Position")]
    public class Position : MintaBaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Position(Session session)
            : base(session)
        {
        }
        [Association("Departments-Positions")]
        public XPCollection<Department> Departments=> GetCollection<Department>("Departments");
        
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        
    }
}