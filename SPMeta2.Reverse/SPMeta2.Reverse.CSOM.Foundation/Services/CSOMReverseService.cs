using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Models;
using SPMeta2.Reverse.Services;
using Microsoft.SharePoint.Client;
using SPMeta2.Definitions;
using SPMeta2.Reverse.CSOM.Foundation.ReverseHandlers;
using SPMeta2.Reverse.CSOM.Foundation.ReverseHandlers.Base;
using SPMeta2.Reverse.CSOM.Foundation.ReverseHosts;
using SPMeta2.Reverse.ReverseHandlers;
using SPMeta2.Reverse.ReverseHosts;
using SPMeta2.Services;
using SPMeta2.Utils;

namespace SPMeta2.Reverse.CSOM.Foundation.Services
{
    public class CSOMReverseService : ReverseServiceBase
    {
        #region constructor

        public CSOMReverseService()
        {
            RegisterReverseHandlers(typeof(FieldReverseHandler).Assembly);

            //Handlers.Add(new SiteReverseHandler());

            //Handlers.Add(new FieldReverseHandler());
            //Handlers.Add(new ContentTypeReverseHandler());

            //Handlers.Add(new WebReverseHandler());
        }

        protected void RegisterReverseHandlers(Assembly assembly)
        {
            var handlerTypes = ReflectionUtils.GetTypesFromAssembly<CSOMReverseHandlerBase>(assembly);

            foreach (var type in handlerTypes)
            {
                if (!Handlers.Any(r => r.GetType() == type))
                {
                    var instance = Activator.CreateInstance(type) as ReverseHandlerBase;

                    Handlers.Add(instance);
                }
            }
        }

        #endregion

        #region classes

        protected class ReverseContext
        {
            public ModelNode RootModelNode { get; set; }
            public ModelNode CurrentModelNode { get; set; }

            public ReverseHostBase ReverseHost { get; set; }
            public ReverseOptions ReverseOptions { get; set; }

        }

        #endregion

        #region methods

        public override ReverseResult Reverse(object modelHost, ReverseOptions options)
        {
            if (!(modelHost is SiteReverseHost)
                && !(modelHost is WebReverseHost))
            {
                throw new Exception("modelHost should be either SiteReverseHost or WebReverseHost");
            }

            return ReverseInternal(modelHost, options);
        }

        private ReverseResult ReverseInternal(object modelHost, ReverseOptions options)
        {
            var result = new ReverseResult();

            var context = new ReverseContext
            {
                ReverseOptions = options,
                ReverseHost = modelHost as ReverseHostBase
            };

            ReverseModel(context);

            result.Model = context.RootModelNode;

            return result;
        }

        //private List<ReverseHandlerBase> LookupHandlers(ReverseContext context)
        //{

        //    var handlers = Handlers.Where(h => h.ReverseParentTypes.Any(t => t == targetType));

        //    return handlers.ToList();
        //}

        public EventHandler<ReverseProgressEventArgs> OnReverseProgress;

        protected virtual void InvokeOnReverseProgress(ReverseProgressEventArgs args)
        {
            if (OnReverseProgress != null)
                OnReverseProgress.Invoke(this, args);
        }

        private void ReverseModel(ReverseContext context)
        {
            if (context.RootModelNode == null)
            {
                Type rootTargetType = null;

                if (context.ReverseHost.GetType() == typeof(SiteReverseHost))
                {
                    rootTargetType = typeof(SiteDefinition);
                }
                else if (context.ReverseHost.GetType() == typeof(WebReverseHost))
                {
                    rootTargetType = typeof(WebDefinition);
                }

                // TODO
                // handle site, root web and web cases

                var rootHandler = Handlers.First(h => h.ReverseType == rootTargetType);

                var rootHost = rootHandler.ReverseHosts(context.ReverseHost, context.ReverseOptions).First();
                var modelNode = rootHandler.ReverseSingleHost(rootHost, context.ReverseOptions);

                context.RootModelNode = modelNode;
                context.CurrentModelNode = modelNode;

                InvokeOnReverseProgress(new ReverseProgressEventArgs
                {
                    CurrentNode = modelNode,
                    TargetType = modelNode.GetType(),
                    ProcessedModelNodeCount = 1,
                    TotalModelNodeCount = 1
                });
            }

            var targetType = context.CurrentModelNode.Value.GetType();
            var childHandlers = Handlers.Where(h => h.ReverseParentTypes.Any(t => t == targetType));

            foreach (var handler in childHandlers)
            {
                var hosts = handler.ReverseHosts(context.ReverseHost, context.ReverseOptions);

                var count = 1;
                var totalCount = hosts.Count();

                foreach (var host in hosts)
                {
                    var modelNode = handler.ReverseSingleHost(host, context.ReverseOptions);

                    context.CurrentModelNode.ChildModels.Add(modelNode);

                    InvokeOnReverseProgress(new ReverseProgressEventArgs
                    {
                        CurrentNode = modelNode,
                        TargetType = modelNode.GetType(),
                        ProcessedModelNodeCount = count,
                        TotalModelNodeCount = totalCount
                    });

                    ReverseModel(new ReverseContext
                    {
                        ReverseOptions = context.ReverseOptions,
                        CurrentModelNode = modelNode,
                        ReverseHost = host,
                        RootModelNode = context.RootModelNode
                    });

                    count++;
                }
            }
        }

        #endregion
    }


}
