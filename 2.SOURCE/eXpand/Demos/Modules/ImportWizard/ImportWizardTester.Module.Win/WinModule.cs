using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;

namespace ImportWizardTester.Module.Win {
    [ToolboxItemFilter("Xaf.Platform.Win")]
    public sealed partial class ImportWizardTesterWindowsFormsModule : ModuleBase {
        public ImportWizardTesterWindowsFormsModule() {
            InitializeComponent();
        }
        public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB) {
            return ModuleUpdater.EmptyModuleUpdaters;
        }
    }
}
