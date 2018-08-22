using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.Base.Security;

namespace Xpand.ExpressApp.Security.Controllers {
    public partial class DefaultRolePermissionsController : ViewController {
        public DefaultRolePermissionsController() {
            InitializeComponent();
            RegisterActions(components);
            TargetObjectType = typeof(ICustomizableRole);
            TargetViewType = ViewType.DetailView;
        }
        protected override void OnActivated() {
            base.OnActivated();
            if (View.ObjectSpace.IsNewObject(View.CurrentObject)) {
                var permission = new ObjectAccessPermission(typeof(object), ObjectAccess.AllAccess);
                ((ICustomizableRole)View.CurrentObject).AddPermission(permission);
            }
        }
    }
}
