﻿using System.ComponentModel;
using System.Drawing;
using DevExpress.Utils;
using Xpand.Persistent.Base.General;

namespace Xpand.ExpressApp.ExcelImporter.Web {
    [ToolboxBitmap(typeof(ExcelImporterWebModule))]
    [ToolboxItem(true)]
    [ToolboxTabName(XpandAssemblyInfo.TabAspNetModules)]
    public sealed partial class ExcelImporterWebModule : XpandModuleBase {
        public ExcelImporterWebModule() {
            InitializeComponent();
        }
    }
}
