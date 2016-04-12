using SPMeta2.Reverse.CSOM.ReverseHandlers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public class TaxonomyTermGroupReverseHandler : CSOMReverseHandlerBase
    {
        #region properties
        public override Type ReverseType
        {
            get { return typeof(TaxonomyTermGroupDefinition); }
        }

        public override IEnumerable<Type> ReverseParentTypes
        {
            get
            {
                return new[]
                {
                    typeof(TaxonomyTermStoreDefinition)
                };
            }
        }


        #endregion

        #region methods

        public override IEnumerable<ReverseHostBase> ReverseHosts(ReverseHostBase parentHost, ReverseOptions options)
        {
            var result = new List<TaxonomyTermGroupReverseHost>();

            var typedHost = parentHost.WithAssertAndCast<TaxonomyTermStoreReverseHost>("reverseHost", value => value.RequireNotNull());

            var context = typedHost.HostClientContext;

            var site = typedHost.HostSite;
            var store = typedHost.HostTermStore;

            var items = store.Groups;

            context.Load(items);
            context.ExecuteQuery();

            result.AddRange(ApplyReverseFilters(items, options).ToArray().Select(i =>
            {
                return ModelHostBase.Inherit<TaxonomyTermGroupReverseHost>(parentHost, h =>
                {
                    h.HostTermGroup = i;
                });
            }));

            return result;
        }

        public override ModelNode ReverseSingleHost(object reverseHost, ReverseOptions options)
        {
            var typedHost = (reverseHost as TaxonomyTermGroupReverseHost);
            var item = typedHost.HostTermGroup;

            var def = new TaxonomyTermGroupDefinition();

            def.Id = item.Id;
            def.Name = item.Name;

            // TODO, M2
            // https://github.com/SubPointSolutions/spmeta2/issues/827
            //def.Descrption = item.Description;

            return new TaxonomyTermStoreModelNode
            {
                Options = { RequireSelfProcessing = true },
                Value = def
            };
        }

        #endregion
    }
}
