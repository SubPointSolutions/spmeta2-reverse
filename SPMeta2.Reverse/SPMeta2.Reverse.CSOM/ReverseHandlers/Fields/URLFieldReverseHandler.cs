using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.SharePoint.Client;
using SPMeta2.Definitions;
using SPMeta2.ModelHosts;
using SPMeta2.Models;
using SPMeta2.Reverse.CSOM.ReverseHandlers.Base;
using SPMeta2.Reverse.CSOM.ReverseHosts;
using SPMeta2.Reverse.ReverseHosts;
using SPMeta2.Reverse.Services;
using SPMeta2.Syntax.Default;
using SPMeta2.Definitions.Fields;
using SPMeta2.Enumerations;
using SPMeta2.Utils;

namespace SPMeta2.Reverse.CSOM.ReverseHandlers.Fields
{
    public class URLFieldReverseHandler : FieldReverseHandler
    {
        #region properties
        public override Type ReverseType
        {
            get { return typeof(URLFieldDefinition); }
        }

        #endregion

        #region methods

        protected override IEnumerable<Field> GetTypedFields(ClientContext context, FieldCollection items)
        {
            var typedFields = context.LoadQuery(items.Where(i => i.FieldTypeKind == FieldType.URL));
            context.ExecuteQuery();

            return typedFields;
        }

        protected override FieldDefinition GetFieldDefinitionInstance()
        {
            return new URLFieldDefinition();
        }

        protected override ModelNode GetFieldModelNodeInstance()
        {
            return new URLFieldModelNode();
        }

        protected override void PostProcessFieldDefinitionInstance(FieldDefinition def, FieldReverseHost typedReverseHost, ReverseOptions options)
        {
            var context = typedReverseHost.HostClientContext;

            var typedField = context.CastTo<FieldUrl>(typedReverseHost.Field);
            var typedDef = def.WithAssertAndCast<URLFieldDefinition>("modelHost", m => m.RequireNotNull());

            var xml = XDocument.Parse(typedField.SchemaXml);
            var fieldXml = xml.Root;

            var displayFormat = ConvertUtils.ToString(fieldXml.GetAttributeValue(BuiltInFieldAttributes.Format));
            typedDef.DisplayFormat = displayFormat;
        }

        #endregion
    }
}
