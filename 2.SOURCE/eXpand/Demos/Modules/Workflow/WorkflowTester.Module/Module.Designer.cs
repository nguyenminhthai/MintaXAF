namespace WorkflowTester.Module {
    partial class WorkflowTesterModule {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            // 
            // WorkflowTesterModule
            // 
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.SystemModule.SystemModule));
            this.RequiredModuleTypes.Add(typeof(Xpand.ExpressApp.Workflow.XpandWorkFlowModule));
//            this.RequiredModuleTypes.Add(typeof(DynAssembly1.DynamicDynAssembly1Module));
//            this.RequiredModuleTypes.Add(typeof(DynAssembly2.DynamicDynAssembly2Module));
        }

        #endregion
    }
}
