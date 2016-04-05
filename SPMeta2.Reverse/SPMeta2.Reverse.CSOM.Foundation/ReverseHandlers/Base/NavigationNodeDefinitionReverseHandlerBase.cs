using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint.Client;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Exceptions;
using SPMeta2.ModelHosts;
using SPMeta2.Models;
using SPMeta2.Reverse.CSOM.Foundation.ReverseHosts;
using SPMeta2.Reverse.Exceptions;
using SPMeta2.Reverse.ReverseHosts;
using SPMeta2.Reverse.Services;
using SPMeta2.Syntax.Default;
using SPMeta2.Utils;

namespace SPMeta2.Reverse.CSOM.Foundation.ReverseHandlers.Base
{
    public abstract class NavigationNodeDefinitionReverseHandlerBase : CSOMReverseHandlerBase
    {
        #region properties

        //public override Type ReverseType
        //{
        //    get { return typeof(ListDefinition); }
        //}

        public override IEnumerable<Type> ReverseParentTypes
        {
            get
            {
                return new[]
                {
                    typeof(WebDefinition)
                };
            }
        }
        #endregion

        #region methods

        protected abstract NavigationNodeCollection GetNavigationNodes(ReverseHostBase parentHost, ReverseOptions options);

        public override IEnumerable<ReverseHostBase> ReverseHosts(ReverseHostBase parentHost, ReverseOptions options)
        {
            var result = new List<NavigationNodeReverseHost>();

            var typedHost = parentHost.WithAssertAndCast<WebReverseHost>("reverseHost", value => value.RequireNotNull());
            var context = typedHost.HostClientContext;

            var items = GetNavigationNodes(parentHost, options);

            context.Load(items);
            context.ExecuteQuery();

            result.AddRange(items.ToArray().Select(i =>
            {
                return ModelHostBase.Inherit<NavigationNodeReverseHost>(parentHost, h =>
                {
                    h.HostNavigationNode = i;
                });
            }));

            return result;
        }

        protected abstract NavigationNodeDefinitionBase GetNavigationNodeDefinitionInstance();

        public override ModelNode ReverseSingleHost(object reverseHost, ReverseOptions options)
        {
            var item = (reverseHost as NavigationNodeReverseHost).HostNavigationNode;
            var def = GetNavigationNodeDefinitionInstance();

            def.Title = item.Title;
            def.Url = item.Url;
            def.IsExternal = item.IsExternal;

            if (def is QuickLaunchNavigationNodeDefinition)
            {
                return new QuickLaunchNavigationNodeModelNode
                {
                    Options = { RequireSelfProcessing = true },
                    Value = def
                };
            }
            else if (def is TopNavigationNodeDefinition)
            {
                return new TopNavigationNodeModelNode
                {
                    Options = { RequireSelfProcessing = true },
                    Value = def
                };
            }

            throw new SPMeta2ReverseException(string.Format("Navigation node definition of type:[{0}] is not supported",
                item.GetType()));
        }

        #endregion
    }
}
