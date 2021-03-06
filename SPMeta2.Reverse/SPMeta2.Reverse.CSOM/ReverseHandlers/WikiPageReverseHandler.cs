using SPMeta2.Definitions;
using SPMeta2.Reverse.CSOM.ReverseHandlers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
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
using SPMeta2.Enumerations;
using SPMeta2.Reverse.Exceptions;

namespace SPMeta2.Reverse.CSOM.ReverseHandlers
{
    public class WikiPageReverseHandler : CSOMReverseHandlerBase
    {
        #region properties
        public override Type ReverseType
        {
            get { return typeof(WikiPageDefinition); }
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
            var result = new List<WikiPageReverseHost>();

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

            // TODO, query only wiki pages        
            var items = hostFolder.Files;

            context.Load(items, i => i.Include(t => t.ListItemAllFields, t => t.Name));
            context.ExecuteQueryWithTrace();

            result.AddRange(ApplyReverseFilters(items, options).ToArray().Select(i =>
            {
                return ModelHostBase.Inherit<WikiPageReverseHost>(parentHost, h =>
                {
                    h.HostWikiPageFile = i;
                });
            }));

            return result;
        }

        public override ModelNode ReverseSingleHost(object reverseHost, ReverseOptions options)
        {
            var item = (reverseHost as WikiPageReverseHost).HostWikiPageFile;

            var def = new WikiPageDefinition();

            def.FileName = item.Name;
            def.Content = ConvertUtils.ToString(item.ListItemAllFields[BuiltInInternalFieldNames.WikiField]);

            def.NeedOverride = true;

            return new WikiPageModelNode
            {
                Options = { RequireSelfProcessing = true },
                Value = def
            };
        }

        #endregion
    }
}
