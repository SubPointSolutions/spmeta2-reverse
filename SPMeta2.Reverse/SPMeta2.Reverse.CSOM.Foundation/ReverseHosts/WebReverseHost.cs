using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;

namespace SPMeta2.Reverse.CSOM.Foundation.ReverseHosts
{
    public class WebReverseHost : CSOMReverseHostBase
    {
        #region constructors

        public WebReverseHost()
        {

        }

        public WebReverseHost(ClientContext clientContext)
        {
            HostClientContext = clientContext;

            HostWeb = clientContext.Web;
            HostSite = clientContext.Site;
        }

        #endregion

        #region static

        public static WebReverseHost FromClientContext(ClientContext clientContext)
        {
            return new WebReverseHost(clientContext);
        }

        #endregion
    }
}
