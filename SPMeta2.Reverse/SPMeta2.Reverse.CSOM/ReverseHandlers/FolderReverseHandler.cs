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
using SPMeta2.Reverse.Exceptions;

namespace SPMeta2.Reverse.CSOM.ReverseHandlers
{
    public class FolderReverseHandler : CSOMReverseHandlerBase
    {
        #region properties
        public override Type ReverseType
        {
            get { return typeof(FolderDefinition); }
        }

        public override IEnumerable<Type> ReverseParentTypes
        {
            get
            {
                return new[]
                {
                    typeof(ListDefinition),
                    typeof(FolderDefinition),
                };
            }
        }
        #endregion

        #region methods

        public override IEnumerable<ReverseHostBase> ReverseHosts(ReverseHostBase parentHost, ReverseOptions options)
        {
            var result = new List<FolderReverseHost>();

            var context = (parentHost as ListReverseHost).HostClientContext;
            Folder hostFolder = null;

            if (parentHost is FolderReverseHost)
            {
                hostFolder = (parentHost as FolderReverseHost).HostFolder;
            }
            else if (parentHost is ListReverseHost)
            {
                hostFolder = (parentHost as ListReverseHost).HostList.RootFolder;
            }
            else
            {
                throw new SPMeta2ReverseException(
                    string.Format("parentHost should be either FolderReverseHost or ListReverseHost"));
            }

            var items = hostFolder.Folders;

            context.Load(items);
            context.ExecuteQuery();

            result.AddRange(ApplyReverseFilters(items, options).ToArray().Select(i =>
            {
                return ModelHostBase.Inherit<FolderReverseHost>(parentHost, h =>
                {
                    h.HostFolder = i;
                });
            }));

            return result;
        }

        public override ModelNode ReverseSingleHost(object reverseHost, ReverseOptions options)
        {
            var item = (reverseHost as FolderReverseHost).HostFolder;

            var def = new FolderDefinition();

            def.Name = item.Name;

            return new FolderModelNode
            {
                Options = { RequireSelfProcessing = true },
                Value = def
            };
        }

        #endregion
    }
}
