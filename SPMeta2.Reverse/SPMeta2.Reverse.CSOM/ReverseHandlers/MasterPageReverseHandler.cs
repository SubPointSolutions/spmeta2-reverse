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
using SPMeta2.Reverse.Exceptions;
using SPMeta2.Syntax.Default.Utils;

namespace SPMeta2.Reverse.CSOM.ReverseHandlers
{
    public class MasterPageReverseHandler : CSOMReverseHandlerBase
    {
        #region properties
        public override Type ReverseType
        {
            get { return typeof(MasterPageDefinition); }
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
            var result = new List<MasterPageReverseHost>();

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

            // TODO, only master page files
            var items = hostFolder.Files;

            context.Load(items);
            context.ExecuteQueryWithTrace();

            result.AddRange(ApplyReverseFilters(items, options).ToArray().Select(i =>
            {
                return ModelHostBase.Inherit<MasterPageReverseHost>(parentHost, h =>
                {
                    h.HostFolder = hostFolder;
                    h.HostMasterPageFile = i;
                });
            }));

            return result;
        }

        public override ModelNode ReverseSingleHost(object reverseHost, ReverseOptions options)
        {
            var item = (reverseHost as MasterPageReverseHost).HostMasterPageFile;
            var context = (reverseHost as MasterPageReverseHost).HostClientContext;

            var def = new MasterPageDefinition();

            def.FileName = item.Name;
            def.Title = item.Title;

            using (var stream = File.OpenBinaryDirect(
                context,
                item.ServerRelativeUrl).Stream)
            {
                def.Content = ModuleFileUtils.ReadFully(stream);
            }

            return new MasterPageModelNode
            {
                Options = { RequireSelfProcessing = true },
                Value = def
            };
        }

        #endregion
    }
}
