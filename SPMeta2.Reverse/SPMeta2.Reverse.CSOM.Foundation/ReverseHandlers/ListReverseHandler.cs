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
    public class ListReverseHandler : CSOMReverseHandlerBase
    {
        #region properties
        public override Type ReverseType
        {
            get { return typeof(ListDefinition); }
        }

        public override IEnumerable<Type> ReverseParentTypes
        {
            get
            {
                return new[]
                {
                    typeof(WebDefinition)
                };
            }
        }
        #endregion

        #region methods

        public override IEnumerable<ReverseHostBase> ReverseHosts(ReverseHostBase parentHost, ReverseOptions options)
        {
            var result = new List<ListReverseHost>();

            var typedHost = parentHost.WithAssertAndCast<WebReverseHost>("reverseHost", value => value.RequireNotNull());

            var site = typedHost.HostSite;
            var web = typedHost.HostWeb;

            var context = typedHost.HostClientContext;

            var items = web.Lists;

            context.Load(items);
            context.ExecuteQuery();

            result.AddRange(items.ToArray().Select(i =>
            {
                return ModelHostBase.Inherit<ListReverseHost>(parentHost, h =>
                {
                    h.HostList = i;
                });
            }));

            return result;
        }

        public override ModelNode ReverseSingleHost(object reverseHost, ReverseOptions options)
        {
            var item = (reverseHost as ListReverseHost).HostList;

            var def = new ListDefinition();

            def.Title = item.Title;
            def.Description = item.Description;

            // TODO, fix up custom URL
            //def.CustomUrl = 

            // TODO, fix for lists based on custom list templates
            def.TemplateType = item.BaseTemplate;

            def.ContentTypesEnabled = item.ContentTypesEnabled;

            def.Hidden = item.Hidden;
            def.OnQuickLaunch = item.OnQuickLaunch;

            return new ListModelNode
            {
                Options = { RequireSelfProcessing = true },
                Value = def
            };
        }

        #endregion
    }
}
