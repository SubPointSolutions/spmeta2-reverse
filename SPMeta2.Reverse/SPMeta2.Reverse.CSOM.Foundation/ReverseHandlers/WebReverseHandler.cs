using SPMeta2.Definitions;
using SPMeta2.Reverse.CSOM.Foundation.ReverseHandlers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint.Client;
using SPMeta2.Models;
using SPMeta2.Reverse.CSOM.Foundation.ReverseHosts;
using SPMeta2.Reverse.ReverseHosts;
using SPMeta2.Reverse.Services;
using SPMeta2.Utils;
using SPMeta2.Syntax.Default;
using SPMeta2.ModelHosts;

namespace SPMeta2.Reverse.CSOM.Foundation.ReverseHandlers
{
    public class WebReverseHandler : CSOMReverseHandlerBase
    {
        #region properties
        public override Type ReverseType
        {
            get { return typeof(WebDefinition); }
        }

        public override IEnumerable<Type> ReverseParentTypes
        {
            get
            {
                return new[]
                {
                    typeof(SiteDefinition),
                    typeof(WebDefinition)
                };
            }
        }
        #endregion

        #region methods

        public override IEnumerable<ReverseHostBase> ReverseHosts(ReverseHostBase parentHost, ReverseOptions options)
        {
            var result = new List<WebReverseHost>();

            Web web = null;

            if (parentHost is SiteReverseHost)
                web = (parentHost as SiteReverseHost).HostWeb;

            if (parentHost is WebReverseHost)
                web = (parentHost as WebReverseHost).HostWeb;

            var context = (parentHost as CSOMReverseHostBase).HostClientContext;

            var items = web.Webs;

            context.Load(items);
            context.ExecuteQuery();

            result.AddRange(items.ToArray().Select(i =>
            {
                return ModelHostBase.Inherit<WebReverseHost>(parentHost, h =>
                {
                    h.HostWeb = i;
                });
            }));

            return result;
        }

        public override ModelNode ReverseSingleHost(object reverseHost, ReverseOptions options)
        {
            var item = (reverseHost as WebReverseHost).HostWeb;

            var def = new WebDefinition();

            def.Title = item.Title;
            def.Description = item.Description;

            def.WebTemplate = item.WebTemplate;

            if (item.Configuration > -1)
            {
                def.WebTemplate = string.Format("{0}#{1}",
                    item.WebTemplate, item.Configuration);
            }

            // always web relative
            def.Url = item.Url.Split('/').Last();

            return new WebModelNode
            {
                Options = { RequireSelfProcessing = true },
                Value = def
            };
        }

        #endregion
    }
}
