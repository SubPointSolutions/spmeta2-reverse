using SPMeta2.Reverse.CSOM.ReverseHandlers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client.Taxonomy;
using SPMeta2.CSOM.Extensions;
using SPMeta2.Definitions;
using SPMeta2.ModelHosts;
using SPMeta2.Models;
using SPMeta2.Reverse.CSOM.ReverseHosts;
using SPMeta2.Reverse.CSOM.Standard.ReverseHosts;
using SPMeta2.Reverse.ReverseHosts;
using SPMeta2.Reverse.Services;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Standard.Syntax;
using SPMeta2.Syntax.Default;
using SPMeta2.Utils;

namespace SPMeta2.Reverse.CSOM.Standard.ReverseHandlers
{
    public class TaxonomyTermStoreReverseHandler : CSOMReverseHandlerBase
    {
        #region properties
        public override Type ReverseType
        {
            get { return typeof(TaxonomyTermStoreDefinition); }
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
            var result = new List<TaxonomyTermStoreReverseHost>();

            var typedHost = parentHost.WithAssertAndCast<SiteReverseHost>("reverseHost", value => value.RequireNotNull());

            var context = typedHost.HostClientContext;
            var site = typedHost.HostSite;

            var session = TaxonomySession.GetTaxonomySession(context);
            var termStore = session.GetDefaultSiteCollectionTermStore();

            context.Load(site);
            context.Load(termStore);

            context.ExecuteQueryWithTrace();

            result.AddRange(ApplyReverseFilters(new[] { site }, options).ToArray().Select(i =>
            {
                return ModelHostBase.Inherit<TaxonomyTermStoreReverseHost>(parentHost, h =>
                {
                    h.HostTermStore = termStore;
                });
            }));

            return result;
        }

        public override ModelNode ReverseSingleHost(object reverseHost, ReverseOptions options)
        {
            var typedHost = (reverseHost as TaxonomyTermStoreReverseHost);
            var item = typedHost.HostSite;

            var def = new TaxonomyTermStoreDefinition();

            def.UseDefaultSiteCollectionTermStore = true;

            return new TaxonomyTermStoreModelNode
            {
                Options = { RequireSelfProcessing = true },
                Value = def
            };
        }

        #endregion
    }
}
