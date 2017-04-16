using SPMeta2.Reverse.CSOM.ReverseHandlers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    public class UniqueContentTypeFieldOrderReverseHandler : CSOMReverseHandlerBase
    {
        #region properties
        public override Type ReverseType
        {
            get { return typeof(UniqueContentTypeFieldsOrderDefinition); }
        }

        public override IEnumerable<Type> ReverseParentTypes
        {
            get
            {
                return new[]
                {
                    typeof(ContentTypeDefinition)
                };
            }
        }


        #endregion

        #region methods

        public override IEnumerable<ReverseHostBase> ReverseHosts(ReverseHostBase parentHost, ReverseOptions options)
        {
            var result = new List<ContentTypeReverseHost>();

            var typedHost = parentHost.WithAssertAndCast<ContentTypeReverseHost>("reverseHost", value => value.RequireNotNull());

            var contentType = typedHost.HostContentType;
            var context = typedHost.HostClientContext;

            var item = contentType;

            context.Load(item);

            context.Load(item, i => i.Fields);
            context.Load(item, i => i.FieldLinks);

            context.ExecuteQueryWithTrace();

            result.AddRange(ApplyReverseFilters(new[] { item }, options).ToArray().Select(i =>
            {
                return ModelHostBase.Inherit<ContentTypeReverseHost>(parentHost, h =>
                {
                    h.HostContentType = i;
                });
            }));

            return result;
        }

        public override ModelNode ReverseSingleHost(object reverseHost, ReverseOptions options)
        {
            var item = (reverseHost as ContentTypeReverseHost).HostContentType;

            var def = new UniqueContentTypeFieldsOrderDefinition();

            foreach (var fieldLink in item.FieldLinks)
            {
                def.Fields.Add(new FieldLinkValue
                {
                    Id = fieldLink.Id
                });
            }

            return new UniqueContentTypeFieldsOrderModelNode
            {
                Options = { RequireSelfProcessing = true },
                Value = def
            };
        }

        #endregion
    }
}
