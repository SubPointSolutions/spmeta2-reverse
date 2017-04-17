using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint.Client;

namespace SPMeta2.Reverse.CSOM.ReverseHosts
{
    public class WebReverseHost : SiteReverseHost
    {
        #region constructors

        public WebReverseHost()
        {

        }

        public WebReverseHost(ClientContext clientContext)
            : base(clientContext)
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
