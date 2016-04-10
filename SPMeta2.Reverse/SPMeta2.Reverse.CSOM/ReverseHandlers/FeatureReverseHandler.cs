using SPMeta2.Definitions;
using SPMeta2.Reverse.CSOM.ReverseHandlers.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.SharePoint.Client;
using SPMeta2.Enumerations;
using SPMeta2.Models;
using SPMeta2.Reverse.CSOM.ReverseHosts;
using SPMeta2.Reverse.ReverseHosts;
using SPMeta2.Reverse.Services;
using SPMeta2.Utils;
using SPMeta2.Syntax.Default;
using SPMeta2.ModelHosts;
using FeatureDefinitionScope = SPMeta2.Definitions.FeatureDefinitionScope;

namespace SPMeta2.Reverse.CSOM.ReverseHandlers
{
    public class FeatureReverseHandler : CSOMReverseHandlerBase
    {
        #region properties
        public override Type ReverseType
        {
            get { return typeof(FeatureDefinition); }
        }

        public override IEnumerable<Type> ReverseParentTypes
        {
            get
            {
                return new[]
                {
                    typeof(SiteDefinition),
                    typeof(WebDefinition),
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

            var context = siteHost.HostClientContext;

            FeatureCollection items = null;

            var isSiteLevel = false;
            var isWebLevel = false;

            var knownFeatures = GetKnownSiteFeatures();
            var allKnownFeatures = new List<FeatureDefinition>();

            allKnownFeatures.AddRange(GetKnownSiteFeatures());
            allKnownFeatures.AddRange(GetKnownWebFeatures());

            if (webHost != null)
            {
                items = webHost.HostWeb.Features;

                isWebLevel = true;
                isSiteLevel = false;

                knownFeatures = GetKnownWebFeatures();
            }
            else if (siteHost != null)
            {
                items = siteHost.HostSite.Features;

                isWebLevel = false;
                isSiteLevel = true;

                knownFeatures = GetKnownSiteFeatures();
            }

            context.Load(items);
            context.ExecuteQuery();

            // feature collection will have only activate features
            // we need to add up OOTB, known features to mark deactivated features

            // activated features, from the collection
            result.AddRange(ApplyReverseFilters(items, options).ToArray().Select(i =>
            {
                return ModelHostBase.Inherit<FeatureReverseHost>(parentHost, h =>
                {
                    h.Feature = i;

                    h.IsActivated = true;

                    h.IsSiteLevel = isSiteLevel;
                    h.IsWebLevel = isWebLevel;

                    h.AllKnownFeatures = allKnownFeatures;
                });
            }));

            // disabled features
            var activatedFeatureIds = items.ToArray().Select(f => f.DefinitionId);
            var disabledFeatures = knownFeatures.Where(d => !activatedFeatureIds.Contains(d.Id));

            result.AddRange(disabledFeatures.ToArray().Select(i =>
            {
                return ModelHostBase.Inherit<FeatureReverseHost>(parentHost, h =>
                {
                    h.Feature = null;
                    h.FeatureId = i.Id;

                    h.IsActivated = false;

                    h.IsSiteLevel = isSiteLevel;
                    h.IsWebLevel = isWebLevel;

                    h.AllKnownFeatures = allKnownFeatures;
                });
            }));

            return result;
        }

        protected virtual IEnumerable<FeatureDefinition> GetKnownSiteFeatures()
        {
            return KnownDefinitionService.GetKnownSiteFeatures();
        }

        protected virtual IEnumerable<FeatureDefinition> GetKnownWebFeatures()
        {
            return KnownDefinitionService.GetKnownWebFeatures();
        }

        public override ModelNode ReverseSingleHost(object reverseHost, ReverseOptions options)
        {
            var typedReverseHost = (reverseHost as FeatureReverseHost);
            var item = typedReverseHost.Feature;

            FeatureDefinition knownFeature = null;

            if (item != null)
            {
                knownFeature = typedReverseHost.AllKnownFeatures
                                               .FirstOrDefault(f => f.Id == item.DefinitionId);
            }
            else
            {
                knownFeature = typedReverseHost.AllKnownFeatures
                                               .FirstOrDefault(f => f.Id == typedReverseHost.FeatureId);
            }

            if (knownFeature == null)
            {
                // TODO, warning in logs 
                // better register a known feature
            }

            var def = new FeatureDefinition();

            if (typedReverseHost.IsSiteLevel)
                def.Scope = FeatureDefinitionScope.Site;

            if (typedReverseHost.IsWebLevel)
                def.Scope = FeatureDefinitionScope.Web;

            if (item != null)
            {
                def.Id = item.DefinitionId;
                def.Enable = true;
            }
            else
            {
                def.Id = typedReverseHost.FeatureId;
                def.Enable = false;
            }

            def.Title = knownFeature != null ? knownFeature.Title : def.Id.ToString();

            return new FeatureModelNode
            {
                Options = { RequireSelfProcessing = true },
                Value = def
            };
        }

        #endregion
    }
}
