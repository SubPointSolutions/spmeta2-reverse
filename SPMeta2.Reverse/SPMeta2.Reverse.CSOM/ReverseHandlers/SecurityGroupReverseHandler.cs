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

namespace SPMeta2.Reverse.CSOM.ReverseHandlers
{
    public class SecurityGroupReverseHandler : CSOMReverseHandlerBase
    {
        #region properties
        public override Type ReverseType
        {
            get { return typeof(SecurityGroupDefinition); }
        }

        public override IEnumerable<Type> ReverseParentTypes
        {
            get
            {
                return new[]
                {
                    typeof(SiteDefinition)
                };
            }
        }
        #endregion

        #region methods

        public override IEnumerable<ReverseHostBase> ReverseHosts(ReverseHostBase parentHost, ReverseOptions options)
        {
            var result = new List<SecurityGroupReverseHost>();

            var typedHost = parentHost.WithAssertAndCast<SiteReverseHost>("reverseHost", value => value.RequireNotNull());

            var site = typedHost.HostSite;
            var context = typedHost.HostClientContext;

            var items = site.RootWeb.SiteGroups;

            context.Load(items);
            context.ExecuteQuery();

            result.AddRange(ApplyReverseFilters(items, options).ToArray().Select(i =>
            {
                return ModelHostBase.Inherit<SecurityGroupReverseHost>(parentHost, h =>
                {
                    h.HostGroup = i;
                });
            }));

            return result;
        }

        public override ModelNode ReverseSingleHost(object reverseHost, ReverseOptions options)
        {
            var item = (reverseHost as SecurityGroupReverseHost).HostGroup;

            var def = new SecurityGroupDefinition();

            def.Name = item.Title;
            def.Description = item.Description;

            return new SecurityGroupModelNode
            {
                Options = { RequireSelfProcessing = true },
                Value = def
            };
        }

        #endregion
    }
}
