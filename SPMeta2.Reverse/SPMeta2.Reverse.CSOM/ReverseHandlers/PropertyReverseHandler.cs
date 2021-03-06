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
                    typeof(SiteDefinition),
                    typeof(WebDefinition),
                    typeof(ListDefinition),
                };
            }
        }
        #endregion

        #region methods

        public override IEnumerable<ReverseHostBase> ReverseHosts(ReverseHostBase parentHost, ReverseOptions options)
        {
            var result = new List<PropertyReverseHost>();

            PropertyValues items = null;

            if (parentHost is ListReverseHost)
                items = (parentHost as ListReverseHost).HostList.RootFolder.Properties;
            else if (parentHost is WebReverseHost)
                items = (parentHost as WebReverseHost).HostWeb.AllProperties;
            else if (parentHost is SiteReverseHost)
                items = (parentHost as SiteReverseHost).HostSite.RootWeb.AllProperties;

            var context = (parentHost as CSOMReverseHostBase).HostClientContext;

            context.Load(items);
            context.ExecuteQueryWithTrace();

            result.AddRange(ApplyReverseFilters(items.FieldValues.Keys, options).ToArray().Select(i =>
            {
                return ModelHostBase.Inherit<PropertyReverseHost>(parentHost, h =>
                {
                    h.HostPropertyName = i;
                    h.HostPropertyValue = items[i];
                });
            }));

            return result;
        }

        public override ModelNode ReverseSingleHost(object reverseHost, ReverseOptions options)
        {
            var item = (reverseHost as PropertyReverseHost).HostPropertyName;
            var itemValue = (reverseHost as PropertyReverseHost).HostPropertyValue;

            var def = new PropertyDefinition();

            def.Key = item;
            def.Value = itemValue;

            def.Overwrite = true;

            return new PropertyModelNode
            {
                Options = { RequireSelfProcessing = true },
                Value = def
            };
        }

        #endregion
    }
}
