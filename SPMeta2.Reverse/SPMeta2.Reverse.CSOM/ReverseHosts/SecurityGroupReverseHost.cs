﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint.Client;

namespace SPMeta2.Reverse.CSOM.ReverseHosts
{
    public class SecurityGroupReverseHost : SiteReverseHost
    {
        #region constructors

        public SecurityGroupReverseHost()
        {

        }

        public SecurityGroupReverseHost(ClientContext clientContext, Group group)
            : base(clientContext)
        {
            HostGroup = group;
        }

        #endregion

        #region properties

        public Group HostGroup { get; set; }

        #endregion

    }
}
