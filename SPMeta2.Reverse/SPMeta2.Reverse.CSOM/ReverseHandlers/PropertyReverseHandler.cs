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
using SPMeta2.Reverse.Exceptions;

namespace SPMeta2.Reverse.CSOM.ReverseHandlers
{
    public class PropertyReverseHandler : CSOMReverseHandlerBase
    {
        #region properties
        public override Type ReverseType
        {
            get { return typeof(PropertyDefinition); }
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
            var result = new List<PropertyReverseHost>();

            var typedHost = parentHost.WithAssertAndCast<SiteReverseHost>("reverseHost", value => value.RequireNotNull());

            var site = typedHost.HostSite;
            var context = typedHost.HostClientContext;

            // TODO        

            //var items = site.RootWeb.SiteGroups;

            //context.Load(items);
            //context.ExecuteQuery();

            //result.AddRange(ApplyReverseFilters(items, options).ToArray().Select(i =>
            //{
            //    return ModelHostBase.Inherit<SecurityGroupReverseHost>(parentHost, h =>
            //    {
            //        h.HostGroup = i;
            //    });
            //}));

            return result;
        }

        public override ModelNode ReverseSingleHost(object reverseHost, ReverseOptions options)
        {
            var item = (reverseHost as PropertyReverseHost).HostProperty;

            var def = new PropertyDefinition();

            // TODO
            //def.Name = item.Name;

            return new PropertyModelNode
            {
                Options = { RequireSelfProcessing = true },
                Value = def
            };
        }

        #endregion
    }
}
