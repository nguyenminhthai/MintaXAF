// <autogenerated />
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
namespace Xpand.ExpressApp.WizardUI.Win.Templates
{
    partial class WizardDetailViewForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.WizardControl = new DevExpress.XtraWizard.WizardControl();
            this.completionWizardPage1 = new DevExpress.XtraWizard.CompletionWizardPage();
            this.showRecordAfterCompletion = new DevExpress.XtraEditors.CheckEdit();
            this._ViewSitePanel = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.WizardControl)).BeginInit();
            this.WizardControl.SuspendLayout();
            this.completionWizardPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.showRecordAfterCompletion.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._ViewSitePanel)).BeginInit();
            this.SuspendLayout();
            // 
            // viewSiteManager
            // 
            this.viewSiteManager.ViewSiteControl = this._ViewSitePanel;
            // 
            // wizardControl
            // 
            this.WizardControl.Controls.Add(this.completionWizardPage1);
            this.WizardControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WizardControl.Image = global::Xpand.ExpressApp.WizardUI.Win.Properties.Resources.wizard_image;
            this.WizardControl.Location = new System.Drawing.Point(0, 0);
            this.WizardControl.Name = "WizardControl";
            this.WizardControl.Pages.AddRange(new DevExpress.XtraWizard.BaseWizardPage[] {
            this.completionWizardPage1});
            this.WizardControl.Size = new System.Drawing.Size(792, 566);
            this.WizardControl.UseAcceptButton = false;
            // 
            // completionWizardPage1
            // 
            this.completionWizardPage1.Controls.Add(this.showRecordAfterCompletion);
            this.completionWizardPage1.Name = "completionWizardPage1";
            this.completionWizardPage1.Size = new System.Drawing.Size(575, 433);
            // 
            // showRecordAfterCompletion
            // 
            this.showRecordAfterCompletion.Location = new System.Drawing.Point(3, 95);
            this.showRecordAfterCompletion.Name = "showRecordAfterCompletion";
            this.showRecordAfterCompletion.Properties.Caption = "Show record after completing the wizard";
            this.showRecordAfterCompletion.Size = new System.Drawing.Size(219, 19);
            this.showRecordAfterCompletion.TabIndex = 7;
            // 
            // _ViewSitePanel
            // 
            this._ViewSitePanel.Location = new System.Drawing.Point(0, 0);
            this._ViewSitePanel.Name = "_ViewSitePanel";
            this._ViewSitePanel.Size = new System.Drawing.Size(200, 100);
            this._ViewSitePanel.TabIndex = 0;
            this._ViewSitePanel.Dock = DockStyle.Fill;
            this._ViewSitePanel.BorderStyle = BorderStyles.NoBorder;
            // 
            // WizardDetailViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 566);
            this.Controls.Add(this.WizardControl);
            this.Name = "WizardDetailViewForm";
            this.Text = "SimpleForm";
            ((System.ComponentModel.ISupportInitialize)(this.WizardControl)).EndInit();
            this.WizardControl.ResumeLayout(false);
            this.completionWizardPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.showRecordAfterCompletion.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._ViewSitePanel)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private DevExpress.XtraEditors.CheckEdit showRecordAfterCompletion;
        private DevExpress.XtraWizard.CompletionWizardPage completionWizardPage1;
        private PanelControl _ViewSitePanel;
    }
}