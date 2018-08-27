using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.XtraGrid.Views.Grid;

namespace MintaXAF.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class CustomizeListViewController : ViewController<ListView>
    {
        public CustomizeListViewController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
            #region Winform
            GridListEditor winListEditor = ((ListView)View).Editor as GridListEditor;
            if (winListEditor != null)
            {
                GridView gridView = winListEditor.GridView;
                #region Enable Filter Row
                gridView.OptionsView.ShowAutoFilterRow = true;
                #endregion

                #region Group Format Display
                gridView.GroupFormat = "{1}";
                #endregion
            }
            #endregion

            #region Webform
            ASPxGridListEditor webListEditor = ((ListView)View).Editor as ASPxGridListEditor;
            if (webListEditor != null)
            {
                #region Enable Filter Row
                webListEditor.Grid.Settings.ShowFilterRow = true;
                #endregion

                #region Group Format Display
                webListEditor.Grid.Settings.GroupFormat = "{1}";
                #endregion
            }
            #endregion
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
