using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Win.SystemModule;
using DevExpress.ExpressApp.Win.Templates;
using DevExpress.ExpressApp.Win.Templates.ActionContainers;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Docking;

namespace FeatureCenter.Module.Win {
    public partial class MainForm : MainFormTemplateBase, IDockManagerHolder, ISupportClassicToRibbonTransform, DevExpress.ExpressApp.Demos.IHintTemplate, IInfoPanelTemplate, ICaptionPanelHolder {
        public override void SetSettings(IModelTemplate modelTemplate) {
            base.SetSettings(modelTemplate);
            navigation.Model = TemplatesHelper.GetNavBarCustomizationNode();
            formStateModelSynchronizerComponent.Model = GetFormStateNode();
        }
        protected virtual void InitializeImages() {
            barMdiChildrenListItem.Glyph = ImageLoader.Instance.GetImageInfo("Action_WindowList").Image;
            barMdiChildrenListItem.LargeGlyph = ImageLoader.Instance.GetLargeImageInfo("Action_WindowList").Image;
            barSubItemPanels.Glyph = ImageLoader.Instance.GetImageInfo("Action_Navigation").Image;
            barSubItemPanels.LargeGlyph = ImageLoader.Instance.GetLargeImageInfo("Action_Navigation").Image;
        }
        public MainForm() {
            InitializeComponent();
            InitializeImages();
            modelSynchronizationManager.ModelSynchronizableComponents.Add(navigation);

            viewSiteManager.ViewSiteControl = viewSitePanel;
            BarManager = mainBarManager;
            UpdateMdiModeDependentProperties();
            documentManager.BarAndDockingController = mainBarAndDockingController;
            documentManager.MenuManager = mainBarManager;
            BarManager.ForceLinkCreate();
        }
        public Bar ClassicStatusBar {
            get { return _statusBar; }
        }
        public DockPanel DockPanelNavigation {
            get { return dockPanelNavigation; }
        }
        public DockManager DockManager {
            get { return mainDockManager; }
        }
        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);
            if(ModelTemplate != null && !string.IsNullOrEmpty(ModelTemplate.DockManagerSettings)) {
                MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(ModelTemplate.DockManagerSettings));
                DockManager.RestoreLayoutFromStream(stream);
            }
        }
        protected override void OnClosing(CancelEventArgs e) {
            if(ModelTemplate != null) {
                MemoryStream stream = new MemoryStream();
                DockManager.SaveLayoutToStream(stream);
                ModelTemplate.DockManagerSettings = Encoding.UTF8.GetString(stream.ToArray());
            }
            base.OnClosing(e);
        }
        protected override void UpdateMdiModeDependentProperties() {
            base.UpdateMdiModeDependentProperties();
            bool isMdi = UIType == UIType.StandardMDI || UIType == UIType.TabbedMDI;
            viewSiteControlPanel.Visible = !isMdi;
            barSubItemWindow.Visibility = isMdi ? BarItemVisibility.Always : BarItemVisibility.Never;
            barMdiChildrenListItem.Visibility = isMdi ? BarItemVisibility.Always : BarItemVisibility.Never;
        }

        #region IHintTemplate Members

        public string Hint {
            get {
                return hintPanel.Text;
            }
            set {
                hintPanel.Text = value;
                hintPanel.Visible = !string.IsNullOrEmpty(value);
            }
        }

        #endregion

        #region IInfoPanelTemplate Members
        DevExpress.XtraEditors.SplitContainerControl IInfoPanelTemplate.SplitContainer {
            get { return splitContainerControl; }
        }
        #endregion

        #region ICaptionPanelHolder Members

        public DevExpress.Utils.Frames.ApplicationCaption8_1 CaptionPanel {
            get { return captionPanel; }
        }

        #endregion
    }
    public interface IInfoPanelTemplate : IFrameTemplate {
        DevExpress.XtraEditors.SplitContainerControl SplitContainer { get; }
    }
    public interface ICaptionPanelHolder {
        DevExpress.Utils.Frames.ApplicationCaption8_1 CaptionPanel { get; }
    }
    [System.ComponentModel.DisplayName("FeatureCenter MainForm Template")]
    public class FeatureCenterMainFormTemplateLocalizer : FrameTemplateLocalizer<MainForm> { }
}
