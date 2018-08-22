﻿using DevExpress.ExpressApp;
using Xpand.ExpressApp.WorldCreator.BusinessObjects;
using Xpand.Persistent.Base.General;
using Xpand.Persistent.Base.PersistentMetaData;
using Xpand.Utils.Helpers;

namespace Xpand.ExpressApp.WorldCreator.System.Observers {
    public class CodeTemplateInfoObserver : ObjectObserver<ICodeTemplateInfo> {
        public CodeTemplateInfoObserver(IObjectSpace objectSpace)
            : base(objectSpace) {
        }

        protected override void OnChanged(ObjectChangedEventArgs<ICodeTemplateInfo> objectChangedEventArgs) {
            base.OnChanged(objectChangedEventArgs);
            if (objectChangedEventArgs.NewValue != null && objectChangedEventArgs.Object.GetPropertyName(x => x.CodeTemplate) == objectChangedEventArgs.PropertyName) {
                objectChangedEventArgs.Object.CloneProperties();
            }

        }

    }
}