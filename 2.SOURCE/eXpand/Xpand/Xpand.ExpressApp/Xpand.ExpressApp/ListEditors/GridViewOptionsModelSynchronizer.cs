﻿using DevExpress.ExpressApp.Model;
using Xpand.ExpressApp.SystemModule;

namespace Xpand.ExpressApp.ListEditors {
    public class GridViewOptionsModelSynchronizer : OptionsModelSynchronizer<object, IModelListView, IModelListViewMainViewOptionsBase> {
        public GridViewOptionsModelSynchronizer(object control, IModelListView modelNode)
            : base(control, modelNode) {
        }

        protected override object GetControl() {
            return Control;
        }
    }
}