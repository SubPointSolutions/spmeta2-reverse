using SPMeta2.Reverse.CSOM.ReverseHandlers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    public class TaxonomyTermSetReverseHandler : CSOMReverseHandlerBase
    {
        #region properties
        public override Type ReverseType
        {
            get { return typeof(TaxonomyTermSetDefinition); }
        }

        public override IEnumerable<Type> ReverseParentTypes
        {
            get
            {
                return new[]
                {
                    typeof(TaxonomyTermGroupDefinition)
                };
            }
        }


        #endregion

        #region methods

        public override IEnumerable<ReverseHostBase> ReverseHosts(ReverseHostBase parentHost, ReverseOptions options)
        {
            var result = new List<TaxonomyTermSetReverseHost>();

            var typedHost = parentHost.WithAssertAndCast<TaxonomyTermGroupReverseHost>("reverseHost", value => value.RequireNotNull());

            var context = typedHost.HostClientContext;

            var site = typedHost.HostSite;
            var termGroup = typedHost.HostTermGroup;

            var items = termGroup.TermSets;

            context.Load(items);
            context.ExecuteQueryWithTrace();

            result.AddRange(ApplyReverseFilters(items, options).ToArray().Select(i =>
            {
                return ModelHostBase.Inherit<TaxonomyTermSetReverseHost>(parentHost, h =>
                {
                    h.HostTermSet = i;
                });
            }));

            return result;
        }

        public override ModelNode ReverseSingleHost(object reverseHost, ReverseOptions options)
        {
            var typedHost = (reverseHost as TaxonomyTermSetReverseHost);
            var item = typedHost.HostTermSet;

            var def = new TaxonomyTermSetDefinition();

            def.Id = item.Id;

            def.Name = item.Name;
            def.Description = item.Description;

            def.Contact = item.Contact;

            def.IsAvailableForTagging = item.IsAvailableForTagging;
            def.IsOpenForTermCreation = item.IsOpenForTermCreation;

            return new TaxonomyTermSetModelNode
            {
                Options = { RequireSelfProcessing = true },
                Value = def
            };
        }

        #endregion
    }
}
