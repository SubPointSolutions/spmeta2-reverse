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
using SPMeta2.Reverse.Exceptions;
using SPMeta2.Reverse.ReverseHosts;
using SPMeta2.Reverse.Services;
using SPMeta2.Syntax.Default;
using SPMeta2.Utils;

namespace SPMeta2.Reverse.CSOM.ReverseHandlers.ContentTypes
{
    public class UniqueContentTypeOrderReverseHandler : CSOMReverseHandlerBase
    {
        #region properties
        public override Type ReverseType
        {
            get { return typeof(UniqueContentTypeOrderDefinition); }
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

            var folder = list.RootFolder;

            context.Load(list);
            context.Load(list, l => l.ContentTypes);
            context.Load(folder);
            context.Load(folder, f => f.UniqueContentTypeOrder);

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

            var listContentTypes = item.ContentTypes.ToArray();

            var def = new UniqueContentTypeOrderDefinition();

            if (item.RootFolder.UniqueContentTypeOrder != null)
            {
                foreach (var ct in item.RootFolder.UniqueContentTypeOrder.ToArray())
                {
                    var sourceContentType = listContentTypes.FirstOrDefault(c => c.StringId == ct.StringValue);

                    if (sourceContentType == null)
                    {
                        throw new SPMeta2ReverseException(
                            string.Format("Cannot find list content type by ID:[{0}]", ct.StringValue));
                    }

                    def.ContentTypes.Add(new ContentTypeLinkValue
                    {
                        ContentTypeName = sourceContentType.Name
                    });
                }
            }

            return new UniqueContentTypeOrderModelNode
            {
                Options = { RequireSelfProcessing = true },
                Value = def
            };
        }

        #endregion
    }
}
