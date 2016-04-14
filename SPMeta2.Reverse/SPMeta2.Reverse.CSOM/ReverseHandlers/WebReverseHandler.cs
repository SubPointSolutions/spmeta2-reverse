using SPMeta2.Definitions;
using SPMeta2.Reverse.CSOM.ReverseHandlers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.Extensions;
using SPMeta2.Models;
using SPMeta2.Reverse.CSOM.ReverseHosts;
using SPMeta2.Reverse.ReverseHosts;
using SPMeta2.Reverse.Services;
using SPMeta2.Utils;
using SPMeta2.Syntax.Default;
using SPMeta2.ModelHosts;

namespace SPMeta2.Reverse.CSOM.ReverseHandlers
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
            TraceService.Information((int)ReverseLogEventId.ReverseHostsStart, "Processing web reverse host start");

            var result = new List<WebReverseHost>();

            Web web = null;

            if (parentHost is WebReverseHost)
                web = (parentHost as WebReverseHost).HostWeb;
            else if (parentHost is SiteReverseHost)
                web = (parentHost as SiteReverseHost).HostWeb;

            var rootWeb = (parentHost as SiteReverseHost).HostSite.RootWeb;
            var context = (parentHost as CSOMReverseHostBase).HostClientContext;


            //if (!web.IsObjectPropertyInstantiated("ServerRelativeUrl"))
            //{
            //    context.Load(web, w => w.ServerRelativeUrl);
            //    context.Load(rootWeb, w => w.ServerRelativeUrl);

            //    context.ExecuteQueryWithTrace();
            //}

            //var isRootWeb = web.ServerRelativeUrl == rootWeb.ServerRelativeUrl;

            //if (UseRootWebOnly && isRootWeb)
            //{
            //    return new[] { parentHost };
            //}

            var items = web.Webs;

            context.Load(items);
            context.ExecuteQueryWithTrace();

            result.AddRange(ApplyReverseFilters(items, options).ToArray().Select(i =>
            {
                return ModelHostBase.Inherit<WebReverseHost>(parentHost, h =>
                {
                    h.HostWeb = i;
                });
            }));

            TraceService.Information((int)ReverseLogEventId.ReverseHostsStart, "Processing web reverse host end");

            return result;
        }

        public override ModelNode ReverseSingleHost(object reverseHost, ReverseOptions options)
        {
            TraceService.Information((int)ReverseLogEventId.ReverseSingleHostStart, "Processing single web host start");

            var item = (reverseHost as WebReverseHost).HostWeb;

            var def = new WebDefinition();

            if (!item.IsPropertyAvailable("Title"))
            {
                item.Context.Load(item);
                item.Context.ExecuteQueryWithTrace();
            }

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

            TraceService.Information((int)ReverseLogEventId.ReverseSingleHostStart, "Processing single web host end");

            return new WebModelNode
            {
                Options = { RequireSelfProcessing = true },
                Value = def
            };
        }

        #endregion
    }
}
