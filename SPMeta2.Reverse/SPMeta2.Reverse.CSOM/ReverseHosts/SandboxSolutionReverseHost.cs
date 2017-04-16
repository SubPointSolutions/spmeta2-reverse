using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint.Client;

namespace SPMeta2.Reverse.CSOM.ReverseHosts
{
    public class SandboxSolutionReverseHost : WebReverseHost
    {
        #region properties

        public File HostSandboxSolutionFile { get; set; }

        #endregion
    }
}
