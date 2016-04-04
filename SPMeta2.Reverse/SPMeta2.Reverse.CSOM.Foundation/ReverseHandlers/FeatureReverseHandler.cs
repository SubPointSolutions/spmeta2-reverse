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
using FeatureDefinitionScope = SPMeta2.Definitions.FeatureDefinitionScope;

namespace SPMeta2.Reverse.CSOM.Foundation.ReverseHandlers
{
    public class FeatureReverseHandler : CSOMReverseHandlerBase
    {
        #region properties
        public override Type ReverseType
        {
            get { return typeof(FieldDefinition); }
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
            var result = new List<FeatureReverseHost>();

            var siteHost = parentHost as SiteReverseHost;
            var webHost = parentHost as WebReverseHost;

            var site = siteHost.HostSite;
            var web = siteHost.HostSite;

            var context = siteHost.HostClientContext;

            FeatureCollection items = null;

            var isSiteLevel = false;
            var isWebLevel = false;

            if (webHost != null)
            {
                items = web.Features;

                isWebLevel = true;
                isSiteLevel = false;
            }
            else if (siteHost != null)
            {
                items = site.Features;

                isWebLevel = false;
                isSiteLevel = true;
            }

            context.Load(items);
            context.ExecuteQuery();

            result.AddRange(items.ToArray().Select(i =>
            {
                return ModelHostBase.Inherit<FeatureReverseHost>(parentHost, h =>
                {
                    h.Feature = i;

                    h.IsSiteLevel = isSiteLevel;
                    h.IsWebLevel = isWebLevel;
                });
            }));

            return result;
        }

        public override ModelNode ReverseSingleHost(object reverseHost, ReverseOptions options)
        {
            var typedReverseHost = (reverseHost as FeatureReverseHost);
            var item = typedReverseHost.Feature;

            var def = new FeatureDefinition();

            //def.Title = item.DefinitionId;
            def.Id = item.DefinitionId;

            if (typedReverseHost.IsSiteLevel)
                def.Scope = FeatureDefinitionScope.Site;

            if (typedReverseHost.IsWebLevel)
                def.Scope = FeatureDefinitionScope.Web;

            // TODO, hm... how to check if feature was enabled / disabled?
            // seems there should be a list of 'known' features
            // only by comparing it to the original collection we can make decision on enable / disable feature

            return new FeatureModelNode
            {
                Options = { RequireSelfProcessing = true },
                Value = def
            };
        }

        #endregion
    }
}
