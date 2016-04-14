using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint.Client.Taxonomy;
using SPMeta2.CSOM.Extensions;
using SPMeta2.ModelHosts;
using SPMeta2.Models;
using SPMeta2.Reverse.CSOM.ReverseHandlers.Base;
using SPMeta2.Reverse.CSOM.Standard.ReverseHosts;
using SPMeta2.Reverse.Exceptions;
using SPMeta2.Reverse.ReverseHosts;
using SPMeta2.Reverse.Services;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Standard.Syntax;
using SPMeta2.Utils;

namespace SPMeta2.Reverse.CSOM.Standard.ReverseHandlers.Taxonomy
{
    public class TaxonomyTermReverseHandler : CSOMReverseHandlerBase
    {
        #region properties
        public override Type ReverseType
        {
            get { return typeof(TaxonomyTermDefinition); }
        }

        public override IEnumerable<Type> ReverseParentTypes
        {
            get
            {
                return new[]
                {
                    typeof(TaxonomyTermSetDefinition),
                    typeof(TaxonomyTermDefinition),
                };
            }
        }


        #endregion

        #region methods

        public override IEnumerable<ReverseHostBase> ReverseHosts(
            ReverseHostBase parentHost, ReverseOptions options)
        {
            var result = new List<TaxonomyTermSetReverseHost>();

            var typedHost = parentHost.WithAssertAndCast<TaxonomyTermGroupReverseHost>("reverseHost", value => value.RequireNotNull());

            var context = typedHost.HostClientContext;

            var site = typedHost.HostSite;

            TermCollection items = null;

            if (parentHost is TaxonomyTermReverseHost)
                items = (parentHost as TaxonomyTermReverseHost).HostTerm.Terms;
            else if (parentHost is TaxonomyTermSetReverseHost)
                items = (parentHost as TaxonomyTermSetReverseHost).HostTermSet.Terms;
            else
            {
                throw new SPMeta2ReverseException(
                    string.Format("Unsupported host:[{0}]", parentHost.GetType()));
            }

            context.Load(items);
            context.ExecuteQueryWithTrace();

            result.AddRange(ApplyReverseFilters(items, options).ToArray().Select(i =>
            {
                return ModelHostBase.Inherit<TaxonomyTermReverseHost>(parentHost, h =>
                {
                    h.HostTerm = i;
                });
            }));

            return result;
        }

        public override ModelNode ReverseSingleHost(object reverseHost, ReverseOptions options)
        {
            var typedHost = (reverseHost as TaxonomyTermReverseHost);
            var item = typedHost.HostTerm;

            var def = new TaxonomyTermDefinition();

            def.Id = item.Id;

            def.Name = item.Name;
            def.Description = item.Description;


            def.IsAvailableForTagging = item.IsAvailableForTagging;

            return new TaxonomyTermModelNode
            {
                Options = { RequireSelfProcessing = true },
                Value = def
            };
        }

        #endregion
    }
}
