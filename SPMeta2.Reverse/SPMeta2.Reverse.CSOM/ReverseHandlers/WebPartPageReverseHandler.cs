using SPMeta2.Definitions;
using SPMeta2.Reverse.CSOM.ReverseHandlers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Client;
using SPMeta2.Models;
using SPMeta2.Reverse.CSOM.ReverseHosts;
using SPMeta2.Reverse.ReverseHosts;
using SPMeta2.Reverse.Services;
using SPMeta2.Utils;
using SPMeta2.Syntax.Default;
using SPMeta2.ModelHosts;
using System.Xml.Linq;
using SPMeta2.CSOM.Extensions;
using SPMeta2.Reverse.Exceptions;
using SPMeta2.Syntax.Default.Utils;

namespace SPMeta2.Reverse.CSOM.ReverseHandlers
{
    public class WebPartPageReverseHandler : CSOMReverseHandlerBase
    {
        #region properties
        public override Type ReverseType
        {
            get { return typeof(WebPartPageDefinition); }
        }

        public override IEnumerable<Type> ReverseParentTypes
        {
            get
            {
                return new[]
                {
                    typeof(ListDefinition),
                    typeof(FolderDefinition)
                };
            }
        }
        #endregion

        #region methods

        public override IEnumerable<ReverseHostBase> ReverseHosts(ReverseHostBase parentHost, ReverseOptions options)
        {
            var result = new List<WebPartPageReverseHost>();

            var typedHost = parentHost.WithAssertAndCast<SiteReverseHost>("reverseHost", value => value.RequireNotNull());

            var site = typedHost.HostSite;
            var context = typedHost.HostClientContext;

            Folder hostFolder = null;

            if (parentHost is FolderReverseHost)
                hostFolder = (parentHost as FolderReverseHost).HostFolder;
            else if (parentHost is ListReverseHost)
                hostFolder = (parentHost as ListReverseHost).HostList.RootFolder;
            else
            {
                throw new SPMeta2ReverseException(
                    string.Format("Unsupported host:[{0}]", parentHost.GetType()));
            }

            // TODO, query only web part pages
            var items = hostFolder.Files;

            context.Load(items, i => i.Include(
                                            t => t.ListItemAllFields,
                                            t => t.Name,
                                            t => t.ServerRelativeUrl,
                                            t => t.Title));
            context.ExecuteQueryWithTrace();

            result.AddRange(ApplyReverseFilters(items, options).ToArray().Select(i =>
            {
                return ModelHostBase.Inherit<WebPartPageReverseHost>(parentHost, h =>
                {
                    h.HostWebPartPageFile = i;
                });
            }));

            return result;
        }

        public override ModelNode ReverseSingleHost(object reverseHost, ReverseOptions options)
        {
            var item = (reverseHost as WebPartPageReverseHost).HostWebPartPageFile;
            var context = (reverseHost as WebPartPageReverseHost).HostClientContext;

            var def = new WebPartPageDefinition();

            def.FileName = item.Name;
            def.Title = item.Title;

            // always reverse to CustomPageLayout
            // we don't know what is the content of the web part page
            using (var stream = File.OpenBinaryDirect(
                context,
                item.ServerRelativeUrl).Stream)
            {
                def.CustomPageLayout = Encoding.UTF8.GetString(ModuleFileUtils.ReadFully(stream));
            }

            def.NeedOverride = true;

            return new WebPartPageModelNode
            {
                Options = { RequireSelfProcessing = true },
                Value = def
            };
        }

        #endregion
    }
}
