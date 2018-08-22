﻿using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.Utils;
using Xpand.Persistent.Base.General;
using Xpand.Persistent.Base.Security;

namespace Xpand.ExpressApp.Security.AuthenticationProviders {
    [ToolboxTabName(XpandAssemblyInfo.TabSecurity)]
    public class XpandAuthenticationStandard : AuthenticationStandard {
        public XpandAuthenticationStandard() {
        }

        public XpandAuthenticationStandard(Type userType, Type logonParametersType)
            : base(userType, logonParametersType) {
        }

        public override bool AskLogonParametersViaUI {
            get {
                var application = ApplicationHelper.Instance.Application;
                if (application.GetPlatform()==Platform.Win){
                    application.ReadLastLogonParameters();
                    var xpandLogonParameters = LogonParameters as XpandLogonParameters;
                    var ask = xpandLogonParameters == null || (!xpandLogonParameters.RememberMe || !(!(string.IsNullOrEmpty(
                                                                                                         xpandLogonParameters.Password)) && !(string.IsNullOrEmpty(xpandLogonParameters.UserName))));
                    if (!ask){
                        var authenticationStandard =((SecurityStrategyBase) SecuritySystem.Instance).Authentication as XpandAuthenticationStandard;
                        return authenticationStandard != null && !authenticationStandard.CanAuthenticate();
                    }
                    return true;
                }
                return base.AskLogonParametersViaUI;
            }
        }

    }
}