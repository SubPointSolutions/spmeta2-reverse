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
    public class SandboxSolutionReverseHandler : CSOMReverseHandlerBase
    {
        #region properties
        public override Type ReverseType
        {
            get { return typeof(SandboxSolutionDefinition); }
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
            var result = new List<SandboxSolutionReverseHost>();

            var typedHost = parentHost.WithAssertAndCast<SiteReverseHost>("reverseHost", value => value.RequireNotNull());

            var site = typedHost.HostSite;
            var context = typedHost.HostClientContext;

            var solutionList = site.RootWeb.GetCatalog((int)ListTemplateType.SolutionCatalog);
            var items = solutionList.RootFolder.Files;

            context.Load(items, i => i.Include(f => f.Name,
                                               f => f.ServerRelativeUrl,
                                               f => f.ListItemAllFields));
            context.ExecuteQueryWithTrace();

            result.AddRange(ApplyReverseFilters(items, options).ToArray().Select(i =>
            {
                return ModelHostBase.Inherit<SandboxSolutionReverseHost>(parentHost, h =>
                {
                    h.HostSandboxSolutionFile = i;
                });
            }));

            return result;
        }

        public override ModelNode ReverseSingleHost(object reverseHost, ReverseOptions options)
        {
            var item = (reverseHost as SandboxSolutionReverseHost).HostSandboxSolutionFile;
            var context = (reverseHost as SandboxSolutionReverseHost).HostClientContext;

            var def = new SandboxSolutionDefinition();

            def.FileName = item.Name;
            def.SolutionId = ConvertUtils.ToGuid(item.ListItemAllFields["SolutionId"]).Value;

            def.Activate = true;

            using (var stream = File.OpenBinaryDirect(
                context,
                item.ServerRelativeUrl).Stream)
            {
                def.Content = ModuleFileUtils.ReadFully(stream);
            }

            return new SandboxSolutionModelNode
            {
                Options = { RequireSelfProcessing = true },
                Value = def
            };
        }

        #endregion
    }
}
