using SPMeta2.Reverse.CSOM.ReverseHandlers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.Extensions;
using SPMeta2.Definitions;
using SPMeta2.Definitions.ContentTypes;
using SPMeta2.ModelHosts;
using SPMeta2.Models;
using SPMeta2.Reverse.CSOM.ReverseHosts;
using SPMeta2.Reverse.ReverseHosts;
using SPMeta2.Reverse.Services;
using SPMeta2.Syntax.Default;
using SPMeta2.Utils;

namespace SPMeta2.Reverse.CSOM.ReverseHandlers.ContentTypes
{
    public class HideContentTypeLinksReverseHandler : ContentTypeLinkReverseHandler
    {
        #region properties
        public override Type ReverseType
        {
            get { return typeof(HideContentTypeLinksDefinition); }
        }

        public override IEnumerable<Type> ReverseParentTypes
        {
            get
            {
                return new[]
                {
                    typeof(ListDefinition)
                };
            }
        }


        #endregion

        #region methods

        public override IEnumerable<ReverseHostBase> ReverseHosts(ReverseHostBase parentHost, ReverseOptions options)
        {
            var result = new List<ListReverseHost>();

            var typedHost = parentHost.WithAssertAndCast<ListReverseHost>("reverseHost", value => value.RequireNotNull());

            var list = typedHost.HostList;
            var context = typedHost.HostClientContext;

            context.Load(list);
            context.Load(list, l => l.ContentTypes);

            context.ExecuteQueryWithTrace();

            result.AddRange(ApplyReverseFilters(new[] { list }, options).ToArray().Select(i =>
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
            var typedHost = (reverseHost as ListReverseHost);
            var item = typedHost.HostList;

            var def = new HideContentTypeLinksDefinition();

            foreach (var ct in item.ContentTypes.ToArray()
                                                .Where(ct => ct.Hidden))
            {
                def.ContentTypes.Add(new ContentTypeLinkValue
                {
                    ContentTypeName = ct.Name
                });
            }

            return new HideContentTypeLinksModelNode
            {
                Options = { RequireSelfProcessing = true },
                Value = def
            };
        }

        #endregion
    }
}
