using System.Security;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Localization;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.SystemModule;
using Xpand.ExpressApp.ModelDifference.DataStore.BaseObjects;
using Xpand.ExpressApp.Security.Core;
using Xpand.Persistent.Base.General;

namespace Xpand.ExpressApp.ModelDifference.Security.Controllers {
    public class NavigationItemsController : WindowController {
        [CoverageExclude]
        protected override void OnActivated() {
            base.OnActivated();
            Frame.GetController<ShowNavigationItemController>(controller => controller.CustomShowNavigationItem += ControllerOnCustomShowNavigationItem);
        }

        [CoverageExclude]
        protected override void OnDeactivated() {
            base.OnDeactivated();
            Frame.GetController<ShowNavigationItemController>(controller => controller.CustomShowNavigationItem -= ControllerOnCustomShowNavigationItem);
        }

        protected internal virtual void ControllerOnCustomShowNavigationItem(object sender, CustomShowNavigationItemEventArgs args) {
            if (args.FitToObjectType(Application, typeof(ModelDifferenceObject))&&SecuritySystem.Instance is ISecurityComplex) {
                SecuritySystem.ReloadPermissions();
                if (!SecuritySystemExtensions.IsGranted(new EditModelPermission(ModelAccessModifier.Allow), false)) {
                    args.Handled = true;
                    throw new SecurityException(ExceptionLocalizerTemplate<SystemExceptionResourceLocalizer, ExceptionId>.GetExceptionMessage(ExceptionId.PermissionIsDenied));
                }
            }
        }
    }
}