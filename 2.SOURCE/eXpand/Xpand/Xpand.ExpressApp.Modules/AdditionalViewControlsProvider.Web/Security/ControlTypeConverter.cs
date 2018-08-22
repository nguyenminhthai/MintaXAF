﻿using System;
using System.Web.UI;
using Xpand.ExpressApp.AdditionalViewControlsProvider.Security;
using Xpand.Persistent.Base.General.Controllers;

namespace Xpand.ExpressApp.AdditionalViewControlsProvider.Web.Security {
    public class ControlTypeConverter:XpandReferenceConverter {
        protected override Type GetTypeInfo() {
            return typeof(Control);
        }
    }
}