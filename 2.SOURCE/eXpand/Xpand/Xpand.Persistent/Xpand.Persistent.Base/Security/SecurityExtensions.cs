﻿using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.ClientServer;
using Xpand.Persistent.Base.General;

namespace Xpand.Persistent.Base.Security{
    public static class SecurityExtensions{

        public static bool IsRemoteClient(this ISecurityStrategyBase security){
            return security is IMiddleTierClientSecurity || security is IClientInfoProvider;
        }

        public static bool CanAuthenticate(this AuthenticationStandard authenticationStandard) {
            object authenticate;
            try {
                authenticate = authenticationStandard.Authenticate(ApplicationHelper.Instance.Application.CreateObjectSpace(SecuritySystem.LogonParameters.GetType()));
            }
            catch (AuthenticationException) {
                return false;
            }
            return authenticate != null;
        }
 
    }
}