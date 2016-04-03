using SPMeta2.Reverse.CSOM.Foundation.ReverseHandlers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using SPMeta2.Definitions;
using SPMeta2.ModelHosts;
using SPMeta2.Models;
using SPMeta2.Reverse.CSOM.Foundation.ReverseHosts;
using SPMeta2.Reverse.ReverseHosts;
using SPMeta2.Reverse.Services;
using SPMeta2.Syntax.Default;
using SPMeta2.Utils;

namespace SPMeta2.Reverse.CSOM.Foundation.ReverseHandlers
{
    public class ContentTypeReverseHandler : CSOMReverseHandlerBase
    {
        #region properties
        public override Type ReverseType
        {
            get { return typeof(ContentTypeDefinition); }
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
            var result = new List<ContentTypeReverseHost>();

            var typedHost = parentHost.WithAssertAndCast<SiteReverseHost>("reverseHost", value => value.RequireNotNull());

            var site = typedHost.HostSite;
            var context = typedHost.HostClientContext;

            var items = site.RootWeb.ContentTypes;

            context.Load(items);
            context.ExecuteQuery();

            result.AddRange(items.ToArray().Where(i => ShouldReverse(i)).Select(i =>
            {
                return ModelHostBase.Inherit<ContentTypeReverseHost>(parentHost, h =>
                {
                    h.ContentType = i;
                });
            }));

            return result;
        }

        private bool ShouldReverse(ContentType ct)
        {
            return ExtractContentTypeIdAsGuid(ct).HasValue;
        }

        private Guid? ExtractContentTypeIdAsGuid(ContentType ct)
        {
            var id = ct.StringId.Split(new string[] { "00" }, StringSplitOptions.None);

            if (id.Length > 0)
            {
                var guid = id.Last();
                return ConvertUtils.ToGuid(guid);
            }

            return null;
        }

        public override ModelNode ReverseSingleHost(object reverseHost, ReverseOptions options)
        {
            var item = (reverseHost as ContentTypeReverseHost).ContentType;

            var def = new ContentTypeDefinition();

            def.Name = item.Name;
            def.Description = item.Description;

            var id = ExtractContentTypeIdAsGuid(item);

            if (id.HasValue)
            {
                def.Id = id.Value;
                def.ParentContentTypeId = item.StringId.Substring(item.StringId.Length - 32, 32);
            }

            def.Group = item.Group;

            return new ContentTypeModelNode
            {
                Options = { RequireSelfProcessing = true },
                Value = def
            };
        }

        #endregion
    }
}
