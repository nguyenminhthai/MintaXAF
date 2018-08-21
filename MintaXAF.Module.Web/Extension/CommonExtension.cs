using DevExpress.ExpressApp.Web;
using MintaXAF.Module.Extension.FriendlyUrl;
using MintaXAF.Module.Web.Extension.FriendlyUrl;

namespace MintaXAF.Module.Web.Extension
{
    public static class CommonExtension
    {
        public static DefaultHttpRequestManager NewHttpRequestManager(this WebApplication application)
        {
            return (application.SupportsFriendlyUrl())
                       ? new FriendlyUrlHttpRequestManager()
                       : new DefaultHttpRequestManager();
        }

        public static bool SupportsFriendlyUrl(this WebApplication application)
        {
            if (application.Model == null)
                return false;
            var modelOptionsFriendlyUrl = application.Model.Options as IModelOptionsFriendlyUrl;
            return modelOptionsFriendlyUrl != null && modelOptionsFriendlyUrl.EnableFriendlyUrl;
        }
    }
}
