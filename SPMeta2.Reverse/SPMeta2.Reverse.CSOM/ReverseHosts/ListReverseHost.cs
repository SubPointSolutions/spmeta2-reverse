using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint.Client;

namespace SPMeta2.Reverse.CSOM.ReverseHosts
{
    public class ListReverseHost : WebReverseHost
    {
        #region constructors

        public ListReverseHost()
        {

        }

        public ListReverseHost(ClientContext clientContext, List list)
            : base(clientContext)
        {
            HostClientContext = clientContext;
        }

        #endregion

        #region properties

        public List HostList { get; set; }

        #endregion

        #region static

        public static WebReverseHost FromClientContext(ClientContext clientContext)
        {
            return new WebReverseHost(clientContext);
        }

        #endregion
    }
}
