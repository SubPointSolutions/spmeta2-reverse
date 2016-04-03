using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.ModelHosts;
using Microsoft.SharePoint.Client;
using SPMeta2.Reverse.ReverseHosts;

namespace SPMeta2.Reverse.CSOM.Foundation.ReverseHosts
{
    public abstract class CSOMReverseHostBase : ReverseHostBase
    {
        #region properties
        public ClientContext HostClientContext { get; set; }

        public Site HostSite { get; set; }
        public Web HostWeb { get; set; } 
        #endregion
    }
}
