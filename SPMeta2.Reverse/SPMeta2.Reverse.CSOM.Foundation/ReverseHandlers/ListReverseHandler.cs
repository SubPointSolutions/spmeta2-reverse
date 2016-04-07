using SPMeta2.Definitions;
using SPMeta2.Reverse.CSOM.Foundation.ReverseHandlers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint.Client;
using SPMeta2.Models;
using SPMeta2.Reverse.CSOM.Foundation.ReverseHosts;
using SPMeta2.Reverse.ReverseHosts;
using SPMeta2.Reverse.Services;
using SPMeta2.Utils;
using SPMeta2.Syntax.Default;
using SPMeta2.ModelHosts;
using SPMeta2.Reverse.Exceptions;

namespace SPMeta2.Reverse.CSOM.Foundation.ReverseHandlers
{
    public class ListReverseHandler : CSOMReverseHandlerBase
    {
        #region properties
        public override Type ReverseType
        {
            get { return typeof(ListDefinition); }
        }

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

        public override IEnumerable<ReverseHostBase> ReverseHosts(ReverseHostBase parentHost, ReverseOptions options)
        {
            var result = new List<ListReverseHost>();

            var typedHost = parentHost.WithAssertAndCast<WebReverseHost>("reverseHost", value => value.RequireNotNull());

            var site = typedHost.HostSite;
            var web = typedHost.HostWeb;

            var context = typedHost.HostClientContext;

            var items = web.Lists;

            //context.Load(items, i => i.Include(r => r.RootFolder, r => r.RootFolder.Properties));
            context.Load(items,
                i => i.Include(r => r.RootFolder,
                                    r => r.Title,
                                    r => r.Description,
                                    r => r.Hidden,
                                    r => r.BaseTemplate,
                                    r => r.ContentTypesEnabled)

                );
            context.ExecuteQuery();

            var resultItems = ApplyReverseFilters(items, options);

            result.AddRange(resultItems.ToArray().Select(i =>
            {
                return ModelHostBase.Inherit<ListReverseHost>(parentHost, h =>
                {
                    h.HostList = i;
                });
            }));

            return result;
        }

        protected virtual IEnumerable<List> ApplyReverseFilters(
            IEnumerable<List> items,
            ReverseOptions options)
        {
            //no filters, returning original collection
            if (!HasReverseFileters(options))
                return items;

            // filtering colleciton as per the filters
            var result = new List<List>();
            var reverseFilters = FindReverseFileters(options);

            foreach (var reverseFilter in reverseFilters)
            {
                var filterPropName = reverseFilter.Filter.PropertyName;
                var filterPropValue = reverseFilter.Filter.PropertyValue;

                var filterOperation = reverseFilter.Filter.Operation;

                switch (filterOperation)
                {
                    case "Equal":
                        {
                            var operationObjects = new List<List>();

                            foreach (var list in items)
                            {
                                var tmpPropValue = ReflectionUtils.GetPropertyValue(list, filterPropName);

                                if (tmpPropValue != null)
                                {
                                    if (tmpPropValue.Equals(filterPropValue))
                                    {
                                        operationObjects.Add(list);
                                    }
                                }
                            }

                            //var includedList =
                            //    items.FirstOrDefault(l => l.Title == reverseFilter.Filter.PropertyValue);

                            //if (includedList != null)
                            //{
                            //    result.Add(includedList);
                            //}
                        }

                        break;
                    default:
                        throw new SPMeta2ReverseException(
                            String.Format("Unsupported filter operation:[{0}]", filterOperation));
                }
            }

            // prevent multiple filter match
            result = result.Distinct().ToList();

            return result;
        }

        public override ModelNode ReverseSingleHost(object reverseHost, ReverseOptions options)
        {
            var web = (reverseHost as ListReverseHost).HostWeb;
            var item = (reverseHost as ListReverseHost).HostList;

            var def = new ListDefinition();

            def.Title = item.Title;
            def.Description = item.Description;

            var listServerRelativeUrl = item.RootFolder.ServerRelativeUrl;

            if (!web.IsObjectPropertyInstantiated("ServerRelativeUrl"))
            {
                web.Context.Load(web, w => w.ServerRelativeUrl);
                web.Context.ExecuteQuery();
            }

            var webServerRelativeUrl = web.ServerRelativeUrl;

            var listWebRelativeUrl = listServerRelativeUrl;

            // roort web / ?
            if (webServerRelativeUrl.Length > 1)
            {
                listWebRelativeUrl = listServerRelativeUrl.Replace(webServerRelativeUrl, string.Empty);
            }

            def.CustomUrl = UrlUtility.RemoveStartingSlash(listWebRelativeUrl);

            // TODO, fix for lists based on custom list templates
            def.TemplateType = item.BaseTemplate;

            def.ContentTypesEnabled = item.ContentTypesEnabled;

            def.Hidden = item.Hidden;
            //def.OnQuickLaunch = item.OnQuickLaunch;

            return new ListModelNode
            {
                Options = { RequireSelfProcessing = true },
                Value = def
            };
        }

        #endregion
    }
}
