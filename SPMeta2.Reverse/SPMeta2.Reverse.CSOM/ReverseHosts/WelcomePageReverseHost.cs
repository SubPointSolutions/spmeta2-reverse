using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint.Client;

namespace SPMeta2.Reverse.CSOM.ReverseHosts
{
    public class WelcomePageReverseHost : CSOMReverseHostBase
    {
        #region properties

        public Folder HostFolder { get; set; }

        #endregion
    }
}
