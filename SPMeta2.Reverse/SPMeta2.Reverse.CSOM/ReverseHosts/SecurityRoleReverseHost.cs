using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint.Client;

namespace SPMeta2.Reverse.CSOM.ReverseHosts
{
    public class SecurityRoleReverseHost : SiteReverseHost
    {
        #region constructors

        public SecurityRoleReverseHost()
        {

        }

        public SecurityRoleReverseHost(ClientContext clientContext, RoleDefinition group)
            : base(clientContext)
        {
            HostRole = group;
        }

        #endregion

        #region properties

        public RoleDefinition HostRole { get; set; }

        #endregion

    }
}
