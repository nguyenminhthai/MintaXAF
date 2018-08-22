namespace MintaXAF.Module.Web.Controllers
{
    partial class SwitchLanguageLogonController
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.sLanguageVietnamese = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.sLanguageEnglish = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // sLanguageVietnamese
            // 
            this.sLanguageVietnamese.Caption = "Vietnamese";
            this.sLanguageVietnamese.ConfirmationMessage = null;
            this.sLanguageVietnamese.Id = "sLanguageVietnamese";
            this.sLanguageVietnamese.ToolTip = null;
            this.sLanguageVietnamese.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.sLanguageVietnamese_Execute);
            // 
            // sLanguageEnglish
            // 
            this.sLanguageEnglish.Caption = "English";
            this.sLanguageEnglish.ConfirmationMessage = null;
            this.sLanguageEnglish.Id = "sLanguageEnglish";
            this.sLanguageEnglish.ToolTip = null;
            this.sLanguageEnglish.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.sLanguageEnglish_Execute);
            // 
            // SwitchLanguageLogonController
            // 
            this.Actions.Add(this.sLanguageVietnamese);
            this.Actions.Add(this.sLanguageEnglish);
            this.ViewControlsCreated += new System.EventHandler(this.SwitchLanguageLogonController_ViewControlsCreated);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction sLanguageVietnamese;
        private DevExpress.ExpressApp.Actions.SimpleAction sLanguageEnglish;
    }
}
