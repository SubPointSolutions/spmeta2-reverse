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
using SPMeta2.Reverse.CSOM.ReverseHandlers;
using SPMeta2.Reverse.CSOM.ReverseHandlers.Base;
using SPMeta2.Reverse.CSOM.ReverseHosts;
using SPMeta2.Reverse.ReverseHandlers;
using SPMeta2.Reverse.ReverseHosts;
using SPMeta2.Services;
using SPMeta2.Utils;

namespace SPMeta2.Reverse.CSOM.Services
{
    public class CSOMReverseService : ReverseServiceBase
    {
        #region constructor

        public CSOMReverseService()
        {
            RegisterReverseHandlers(typeof(FieldReverseHandler).Assembly);
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

        #region events

        public EventHandler<ReverseProgressEventArgs> OnReverseProgress;

        #endregion

        #region properties

        protected int CurrentReverseDepth { get; set; }

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

        protected virtual ReverseResult ReverseInternal(object modelHost, ReverseOptions options)
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


        protected virtual void InvokeOnReverseProgress(ReverseProgressEventArgs args)
        {
            if (OnReverseProgress != null)
                OnReverseProgress.Invoke(this, args);
        }

        protected virtual bool ShouldReverseChildren(ReverseContext context)
        {
            var currentModelNode = context.CurrentModelNode;

            // that's the root model node
            // always continue
            if (currentModelNode == null)
                return true;

            // get a particular depth option for definition
            var definitionType = currentModelNode.Value.GetType();
            var depthOption = context.ReverseOptions
                .Options
                .FirstOrDefault(o => o.DefinitionClassFullName == definitionType.FullName
                                    && o is ReverseDepthOption) as ReverseDepthOption;

            // does it exist? no more that suggested depth
            if (depthOption != null)
            {
                return _localReverseDepth[definitionType] < depthOption.Depth;
            }

            // always full in, by default
            return true;
        }

        private Dictionary<Type, int> _localReverseDepth = new Dictionary<Type, int>();

        protected virtual void ReverseModel(ReverseContext context)
        {
            if (context.RootModelNode == null)
            {
                Type rootTargetType = null;
                ModelNode modelNode = null;
                ReverseHandlerBase rootHandler = null;

                var rootHost = context.ReverseHost;

                // manually reverse a single 'root' model node
                // update RequireSelfProcessing to false 

                if (context.ReverseHost.GetType() == typeof(SiteReverseHost))
                {
                    rootTargetType = typeof(SiteDefinition);
                    rootHandler = Handlers.First(h => h.ReverseType == rootTargetType);

                    modelNode = rootHandler.ReverseSingleHost(rootHost, context.ReverseOptions);
                    modelNode.Options.RequireSelfProcessing = false;
                }
                else if (context.ReverseHost.GetType() == typeof(WebReverseHost))
                {
                    rootTargetType = typeof(WebDefinition);
                    rootHandler = Handlers.First(h => h.ReverseType == rootTargetType);

                    modelNode = rootHandler.ReverseSingleHost(rootHost, context.ReverseOptions);
                    modelNode.Options.RequireSelfProcessing = false;
                }

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

            var defType = context.CurrentModelNode.Value.GetType();

            if (!_localReverseDepth.ContainsKey(defType))
                _localReverseDepth.Add(defType, 0);
            else
                _localReverseDepth[defType] = _localReverseDepth[defType] + 1;

            var shouldReverseSelfChildren = ShouldReverseChildren(context);

            var targetType = context.CurrentModelNode.Value.GetType();
            var childHandlers = Handlers.Where(h => h.ReverseParentTypes.Any(t => t == targetType))
                                        .ToList();

            // prevent furver reverse of the same type
            if (!shouldReverseSelfChildren)
            {
                var selfHandler = childHandlers.FirstOrDefault(h => h.ReverseType == defType);

                if (selfHandler != null)
                {
                    childHandlers.Remove(selfHandler);
                }
            }

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

            _localReverseDepth[defType] = _localReverseDepth[defType] - 1;
        }

        #endregion
    }
}
