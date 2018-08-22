﻿using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using DevExpress.DashboardWin;
using DevExpress.DashboardWin.Bars;
using DevExpress.DashboardWin.Native;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using Xpand.ExpressApp.Dashboard.BusinessObjects;
using Xpand.ExpressApp.XtraDashboard.Win.PropertyEditors;
using ListView = DevExpress.ExpressApp.ListView;

namespace Xpand.ExpressApp.XtraDashboard.Win.Controllers {
    public class DashboardExportController : ObjectViewController<ListView,IDashboardDefinition> {
        private readonly SingleChoiceAction _exportDashboardAction;
        private DashboardViewEditor _dashboardViewEditor;
        private const string ExportToPdf = "PDF";
        private const string ExportToImage = "Image";
        private const string PrintPreview = "PrintPreview";
        public DashboardExportController() {
            _exportDashboardAction = new SingleChoiceAction(this, "ExportDashboardTo", PredefinedCategory.Export) { Caption = "Export Dashboard To" };
            _exportDashboardAction.Items.Add(new ChoiceActionItem(PrintPreview, PrintPreview) { ImageName = "Action_Printing_Preview" });
            _exportDashboardAction.Items.Add(new ChoiceActionItem(ExportToPdf, ExportToPdf) { ImageName = "Action_Export_ToPDF" });
            _exportDashboardAction.Items.Add(new ChoiceActionItem(ExportToImage, ExportToImage) { ImageName = "Action_Export_ToImage" });
            _exportDashboardAction.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            _exportDashboardAction.ItemType = SingleChoiceActionItemType.ItemIsOperation;
            _exportDashboardAction.EmptyItemsBehavior = EmptyItemsBehavior.Deactivate;
            _exportDashboardAction.Execute += ExportDashboardActionOnExecute;
            SaveFileDialog = new SaveFileDialog { OverwritePrompt = true };
        }

        public SingleChoiceAction ExportDashboardAction => _exportDashboardAction;

        public SaveFileDialog SaveFileDialog { get; }

        protected override void OnViewControlsCreated() {
            base.OnViewControlsCreated();
            if (View.Id== "DashboardDefinitionViewer_DetailView") {
                var xmlViewItem = View.FindItem("Xml");
                if (xmlViewItem is DashboardViewEditor item) {
                    _dashboardViewEditor = item;
                    _dashboardViewEditor.DashboardViewer.PopupMenuShowing += DashboardViewer_PopupMenuShowing;
                }
            }
        }

        protected override void OnDeactivated() {
            if (_dashboardViewEditor?.DashboardViewer != null) {
                _dashboardViewEditor.DashboardViewer.PopupMenuShowing -= DashboardViewer_PopupMenuShowing;
            }
            base.OnDeactivated();
        }

        private void DashboardViewer_PopupMenuShowing(object sender, DashboardPopupMenuShowingEventArgs e){
            if (!string.IsNullOrEmpty(e.DashboardItemName)){
                var viewer = (DashboardViewer) sender;
                DashboardItemViewer itemViewer = null;

                var fi =
                    typeof(DashboardItemMouseHitTestEventArgs).GetField("dashboardItemViewer",
                        BindingFlags.NonPublic | BindingFlags.Instance);
                var obj = fi?.GetValue(e);
                if (obj is DashboardItemViewer dashboardItemViewer){
                    itemViewer = dashboardItemViewer;
                }

                if (itemViewer == null){
                    return;
                }


                var printPreviewItemBarItem = new PrintPreviewItemBarItem();
                printPreviewItemBarItem.ItemClick += (o, args) =>((IDashboardViewerCommandBarItem) printPreviewItemBarItem).ExecuteCommand(viewer, itemViewer);
                e.Menu.AddItem(printPreviewItemBarItem);

                var exportItemToPdfBarItem = new ExportItemToPdfBarItem();
                exportItemToPdfBarItem.ItemClick += (o, args) =>((IDashboardViewerCommandBarItem) exportItemToPdfBarItem).ExecuteCommand(viewer, itemViewer);
                e.Menu.AddItem(exportItemToPdfBarItem);

                var exportItemToImageBarItem = new ExportItemToImageBarItem();
                exportItemToImageBarItem.ItemClick += (o, args) => ((IDashboardViewerCommandBarItem) exportItemToImageBarItem).ExecuteCommand(viewer, itemViewer);
                e.Menu.AddItem(exportItemToImageBarItem);

                var exportItemToExcelBarItem = new ExportItemToExcelBarItem();
                exportItemToExcelBarItem.ItemClick += (o, args) => ((IDashboardViewerCommandBarItem) exportItemToExcelBarItem).ExecuteCommand(viewer, itemViewer);
                e.Menu.AddItem(exportItemToExcelBarItem);
            }
        }

        private void ExportDashboardActionOnExecute(object sender, SingleChoiceActionExecuteEventArgs e) {
            var dashboardViewer = CreateDashboardViewer();
            if ((string)e.SelectedChoiceActionItem.Data == PrintPreview) {
                dashboardViewer.PrintPreviewType = DashboardPrintPreviewType.RibbonPreview;
                dashboardViewer.ShowRibbonPrintPreview();
                return;
            }
            if ((string)e.SelectedChoiceActionItem.Data == ExportToPdf)
                ConfigureSaveFileDialog("pdf", "Pdf Files (*.pdf)|*.pdf");
            else if ((string)e.SelectedChoiceActionItem.Data == ExportToImage) {
                ConfigureSaveFileDialog("png", "Png Files (*.png)|*.png");
            }
            var dialogResult = SaveFileDialog.ShowDialog();
            if (dialogResult == DialogResult.OK && Directory.Exists(Path.GetDirectoryName(SaveFileDialog.FileName) + "")) {
                if ((string)e.SelectedChoiceActionItem.Data == ExportToPdf) {
                    dashboardViewer.ExportToPdf(SaveFileDialog.FileName);
                }
                else if ((string)e.SelectedChoiceActionItem.Data == ExportToImage) {
                    dashboardViewer.ExportToImage(SaveFileDialog.FileName);
                }
            }
        }

        protected virtual DashboardViewer CreateDashboardViewer() {
            var dashboardViewer = new DashboardViewer {
                Width = ((Form)Application.MainWindow.Template).Width,
                Height = ((Form)Application.MainWindow.Template).Height
            };
            ((Control) Frame.Template).BeginInvoke(new Action(() => dashboardViewer.Show(Application, (IDashboardDefinition) View.CurrentObject)));
            return dashboardViewer;
        }

        protected virtual void ConfigureSaveFileDialog(string defaultExt, string filter) {
            SaveFileDialog.DefaultExt = defaultExt;
            SaveFileDialog.AddExtension = true;
            SaveFileDialog.Filter = filter;
            SaveFileDialog.FileName = ((IDashboardDefinition)View.CurrentObject).Name;
        }
    }
}