﻿using System;
using System.Diagnostics;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Xpo;
using Xpand.Persistent.Base.MiddleTier;
using Xpand.Persistent.Base.ModelAdapter;

namespace Xpand.ExpressApp.Workflow {
    public class XpandWorkflowApplication:XpandServerApplication {
        public XpandWorkflowApplication(ISecurityStrategyBase securityStrategy)
            : base(securityStrategy) {
            if (Debugger.IsAttached)
                InterfaceBuilder.SkipAssemblyCleanup = true;
        }

        protected override LayoutManager CreateLayoutManagerCore(bool simple) {
            throw new NotImplementedException();
        }
        public new void Logon() {
            base.Logon(null);
        }
        protected override void CreateDefaultObjectSpaceProvider(CreateCustomObjectSpaceProviderEventArgs args) {
            args.ObjectSpaceProvider = new XPObjectSpaceProvider(args.ConnectionString, args.Connection);
        }

    }
}
