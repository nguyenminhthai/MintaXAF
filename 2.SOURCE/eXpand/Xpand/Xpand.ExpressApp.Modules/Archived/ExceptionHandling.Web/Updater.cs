using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;

namespace Xpand.ExpressApp.ExceptionHandling.Web {
    public class Updater : ModuleUpdater {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion)
            : base(objectSpace, currentDBVersion) {
        }
    }
}
