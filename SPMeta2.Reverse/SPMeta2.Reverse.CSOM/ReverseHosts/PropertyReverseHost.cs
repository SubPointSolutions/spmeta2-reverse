using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint.Client;

namespace SPMeta2.Reverse.CSOM.ReverseHosts
{
    public class PropertyReverseHost : WebReverseHost
    {
        #region properties

        public string HostPropertyName { get; set; }
        public object HostPropertyValue { get; set; }

        #endregion
    }
}
