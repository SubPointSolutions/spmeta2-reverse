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
using System.Xml.Linq;
using SPMeta2.CSOM.Extensions;
using SPMeta2.Reverse.Exceptions;

namespace SPMeta2.Reverse.CSOM.ReverseHandlers
{
    public class WelcomePageReverseHandler : CSOMReverseHandlerBase
    {
        #region properties
        public override Type ReverseType
        {
            get { return typeof(WelcomePageDefinition); }
        }

        public override IEnumerable<Type> ReverseParentTypes
        {
            get
            {
                return new[]
                {
                    typeof(WebDefinition),
                    typeof(ListDefinition),
                    typeof(FolderDefinition)
                };
            }
        }
        #endregion

        #region methods

        public override IEnumerable<ReverseHostBase> ReverseHosts(ReverseHostBase parentHost, ReverseOptions options)
        {
            var result = new List<WelcomePageReverseHost>();

            Folder hostFolder = null;

            if (parentHost is FolderReverseHost)
                hostFolder = (parentHost as FolderReverseHost).HostFolder;
            else if (parentHost is ListReverseHost)
                hostFolder = (parentHost as ListReverseHost).HostList.RootFolder;
            else if (parentHost is WebReverseHost)
                hostFolder = (parentHost as WebReverseHost).HostWeb.RootFolder;

            var context = (parentHost as CSOMReverseHostBase).HostClientContext;

            context.Load(hostFolder);
            context.ExecuteQueryWithTrace();

            result.AddRange(ApplyReverseFilters(new[] { hostFolder }, options).ToArray().Select(i =>
            {
                return ModelHostBase.Inherit<WelcomePageReverseHost>(parentHost, h =>
                {
                    h.HostFolder = i;
                });
            }));

            return result;
        }

        public override ModelNode ReverseSingleHost(object reverseHost, ReverseOptions options)
        {
            var item = (reverseHost as WelcomePageReverseHost).HostFolder;

            var def = new WelcomePageDefinition();

            def.Url = item.WelcomePage;

            return new WelcomePageModelNode
            {
                Options = { RequireSelfProcessing = true },
                Value = def
            };
        }

        #endregion
    }
}
