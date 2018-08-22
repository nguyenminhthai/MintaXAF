using System.Windows.Forms;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.XtraGrid;

namespace Xpand.ExpressApp.Win.SystemModule {
    public class NewObjectLookUpViewController : ViewController {
        public NewObjectLookUpViewController() {
            TargetViewType = ViewType.ListView;
        }

        protected override void OnActivated() {
            base.OnActivated();

            if (Frame.Template is ILookupPopupFrameTemplate)
                View.ControlsCreated += (sender, e) => {
                    if (View.Control is GridControl)
                        ((GridControl)View.Control).KeyDown += grid_KeyDown;
                };
        }


        private void grid_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyData == Keys.Space)
                Frame.GetController<NewObjectViewController>().NewObjectAction.DoExecute(null);
        }

        protected override void OnDeactivated() {
            base.OnDeactivated();
            if (Frame.Template is ILookupPopupFrameTemplate && View.Control is GridControl)
                ((GridControl)View.Control).KeyDown -= grid_KeyDown;
        }
    }
}