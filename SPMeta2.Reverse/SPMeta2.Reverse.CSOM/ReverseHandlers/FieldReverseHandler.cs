using SPMeta2.Definitions;
using SPMeta2.Reverse.CSOM.ReverseHandlers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.Extensions;
using SPMeta2.Models;
using SPMeta2.Reverse.CSOM.ReverseHosts;
using SPMeta2.Reverse.ReverseHosts;
using SPMeta2.Reverse.Services;
using SPMeta2.Utils;
using SPMeta2.Syntax.Default;
using SPMeta2.ModelHosts;

namespace SPMeta2.Reverse.CSOM.ReverseHandlers
{
    public class FieldReverseHandler : CSOMReverseHandlerBase
    {
        #region properties
        public override Type ReverseType
        {
            get { return typeof(FieldDefinition); }
        }

        public override IEnumerable<Type> ReverseParentTypes
        {
            get
            {
                return new[]
                {
                    typeof(SiteDefinition),
                    typeof(WebDefinition)
                };
            }
        }
        #endregion

        #region methods

        public override IEnumerable<ReverseHostBase> ReverseHosts(ReverseHostBase parentHost, ReverseOptions options)
        {
            var result = new List<FieldReverseHost>();

            var siteHost = parentHost as SiteReverseHost;
            var webHost = parentHost as WebReverseHost;

            var site = siteHost.HostSite;
            var context = siteHost.HostClientContext;

            FieldCollection items = null;

            if (webHost != null)
            {
                items = webHost.HostWeb.Fields;
            }
            else
            {
                items = siteHost.HostSite.RootWeb.Fields;
            }

            var typedItems = GetTypedFields(context, items);

            result.AddRange(ApplyReverseFilters(typedItems, options).ToArray().Select(i =>
            {
                return ModelHostBase.Inherit<FieldReverseHost>(parentHost, h =>
                {
                    h.Field = i;
                });
            }));

            return result;
        }

        protected virtual IEnumerable<Field> GetTypedFields(ClientContext context, FieldCollection items)
        {
            context.Load(items);
            context.ExecuteQueryWithTrace();

            return items.ToArray();
        }

        protected virtual FieldDefinition GetFieldDefinitionInstance()
        {
            return new FieldDefinition();
        }

        protected virtual ModelNode GetFieldModelNodeInstance()
        {
            return new FieldModelNode();
        }

        protected virtual void PostProcessFieldDefinitionInstance(FieldDefinition def, FieldReverseHost typedReverseHost, ReverseOptions options)
        {
            // implement typed field mapping here
        }

        public override ModelNode ReverseSingleHost(object reverseHost, ReverseOptions options)
        {
            var typedReverseHost = (reverseHost as FieldReverseHost);
            var item = typedReverseHost.Field;

            var def = GetFieldDefinitionInstance();

            def.Title = item.Title;
            def.Description = item.Description;

            def.InternalName = item.InternalName;
            def.Id = item.Id;

            def.FieldType = item.TypeAsString;

            def.DefaultValue = item.DefaultValue;

            def.Required = item.Required;
            def.Hidden = item.Hidden;

            def.Group = item.Group;

            PostProcessFieldDefinitionInstance(def, typedReverseHost, options);

            var modelNode = GetFieldModelNodeInstance();

            modelNode.Options.RequireSelfProcessing = true;
            modelNode.Value = def;

            return modelNode;
        }

        #endregion
    }
}
