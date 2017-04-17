using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint.Client;

namespace SPMeta2.Reverse.CSOM.ReverseHosts
{
    public class SiteReverseHost : CSOMReverseHostBase
    {
        #region constructors

        public SiteReverseHost()
        {

        }

        public SiteReverseHost(ClientContext clientContext)
        {
            HostClientContext = clientContext;

            HostSite = clientContext.Site;
            HostWeb = clientContext.Web;
        }

        #endregion

        #region static

        public static SiteReverseHost FromClientContext(ClientContext clientContext)
        {
            return new SiteReverseHost(clientContext);
        }

        #endregion
    }
}
