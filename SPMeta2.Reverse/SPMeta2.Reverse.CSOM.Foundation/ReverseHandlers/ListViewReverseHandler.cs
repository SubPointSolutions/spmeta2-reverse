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
using System.Xml.Linq;

namespace SPMeta2.Reverse.CSOM.Foundation.ReverseHandlers
{
    public class ListViewReverseHandler : CSOMReverseHandlerBase
    {
        #region properties
        public override Type ReverseType
        {
            get { return typeof(ListViewDefinition); }
        }

        public override IEnumerable<Type> ReverseParentTypes
        {
            get
            {
                return new[]
                {
                    typeof(ListDefinition)
                };
            }
        }
        #endregion

        #region methods

        public override IEnumerable<ReverseHostBase> ReverseHosts(ReverseHostBase parentHost, ReverseOptions options)
        {
            var result = new List<ListViewReverseHost>();

            var typedHost = parentHost.WithAssertAndCast<ListReverseHost>("reverseHost", value => value.RequireNotNull());

            var site = typedHost.HostSite;
            var web = typedHost.HostWeb;
            var list = typedHost.HostList;

            var context = typedHost.HostClientContext;

            var items = list.Views;

            context.Load(items);
            context.ExecuteQuery();

            result.AddRange(ApplyReverseFilters(items, options).ToArray().Select(i =>
            {
                return ModelHostBase.Inherit<ListViewReverseHost>(parentHost, h =>
                {
                    h.HostListView = i;
                });
            }));

            return result;
        }

        public override ModelNode ReverseSingleHost(object reverseHost, ReverseOptions options)
        {
            var list = (reverseHost as ListViewReverseHost).HostList;
            var item = (reverseHost as ListViewReverseHost).HostListView;

            if (!item.IsPropertyAvailable("Fields"))
            {
                item.Context.Load(item);
                item.Context.Load(item, i => i.ViewFields);

                item.Context.ExecuteQuery();
            }

            var def = new ListViewDefinition();

            var xmlDoc = XDocument.Parse(item.ListViewXml);
            var viewXmlNode = xmlDoc.Descendants("View").First();

            var url = viewXmlNode.Attribute("Url")
                                  .Value
                                  .Split('/')
                                  .LastOrDefault();

            def.Title = item.Title;
            def.Url = url;



            def.Hidden = item.Hidden;

            def.IsDefault = item.DefaultView;
            def.IsPaged = item.Paged;

            def.Scope = item.Scope.ToString();

            def.RowLimit = (int)item.RowLimit;
            def.Query = item.ViewQuery;

            def.Type = viewXmlNode.Attribute("Type").Value;

            def.Fields = new System.Collections.ObjectModel.Collection<string>(
                item.ViewFields.ToArray()
                );

            return new ListViewModelNode
            {
                Options = { RequireSelfProcessing = true },
                Value = def
            };
        }

        #endregion
    }
}
