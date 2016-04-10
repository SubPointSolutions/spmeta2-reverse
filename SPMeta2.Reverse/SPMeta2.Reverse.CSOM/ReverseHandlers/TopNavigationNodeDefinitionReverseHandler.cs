using SPMeta2.Definitions;
using SPMeta2.Reverse.CSOM.ReverseHandlers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint.Client;
using SPMeta2.Models;
using SPMeta2.Reverse.CSOM.ReverseHosts;
using SPMeta2.Reverse.ReverseHosts;
using SPMeta2.Reverse.Services;
using SPMeta2.Utils;
using SPMeta2.Syntax.Default;
using SPMeta2.ModelHosts;
using SPMeta2.Definitions.Base;

namespace SPMeta2.Reverse.CSOM.ReverseHandlers
{
    public class TopNavigationNodeDefinitionReverseHandler : NavigationNodeDefinitionReverseHandlerBase
    {

        protected override NavigationNodeCollection GetNavigationNodes(ReverseHostBase parentHost, ReverseOptions options)
        {
            var typedHost = parentHost.WithAssertAndCast<WebReverseHost>("reverseHost", value => value.RequireNotNull());

            return typedHost.HostWeb.Navigation.TopNavigationBar;
        }

        protected override NavigationNodeDefinitionBase GetNavigationNodeDefinitionInstance()
        {
            return new TopNavigationNodeDefinition();
        }

        public override Type ReverseType
        {
            get { return typeof(TopNavigationNodeDefinition); }
        }
    }
}
