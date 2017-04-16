using SPMeta2.Reverse.CSOM.ReverseHandlers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.Extensions;
using SPMeta2.Definitions;
using SPMeta2.ModelHosts;
using SPMeta2.Models;
using SPMeta2.Reverse.CSOM.ReverseHosts;
using SPMeta2.Reverse.ReverseHosts;
using SPMeta2.Reverse.Services;
using SPMeta2.Syntax.Default;
using SPMeta2.Utils;

namespace SPMeta2.Reverse.CSOM.ReverseHandlers
{
    public class ContentTypeFieldLinkReverseHandler : CSOMReverseHandlerBase
    {
        #region properties
        public override Type ReverseType
        {
            get { return typeof(ContentTypeFieldLinkDefinition); }
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
            var result = new List<ContentTypeFieldLinkReverseHost>();

            var typedHost = parentHost.WithAssertAndCast<ContentTypeReverseHost>("reverseHost", value => value.RequireNotNull());

            var contentType = typedHost.HostContentType;
            var context = typedHost.HostClientContext;

            var items = contentType.FieldLinks;

            context.Load(items);
            context.ExecuteQueryWithTrace();

            result.AddRange(ApplyReverseFilters(items, options).ToArray().Select(i =>
            {
                return ModelHostBase.Inherit<ContentTypeFieldLinkReverseHost>(parentHost, h =>
                {
                    h.HostFieldLink = i;
                });
            }));

            return result;
        }

        public override ModelNode ReverseSingleHost(object reverseHost, ReverseOptions options)
        {
            var item = (reverseHost as ContentTypeFieldLinkReverseHost).HostFieldLink;

            var def = new ContentTypeFieldLinkDefinition();

            def.FieldInternalName = item.Name;
            def.FieldId = item.Id;

            def.Hidden = item.Hidden;
            def.Required = item.Required;

            return new ContentTypeFieldLinkModelNode
            {
                Options = { RequireSelfProcessing = true },
                Value = def
            };
        }

        #endregion
    }
}
