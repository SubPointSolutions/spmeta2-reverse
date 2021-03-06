using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.Extensions;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Enumerations;
using SPMeta2.Models;
using SPMeta2.Reverse.CSOM.ReverseHandlers.Base;
using SPMeta2.Reverse.CSOM.ReverseHosts;
using SPMeta2.Reverse.ReverseHosts;
using SPMeta2.Reverse.Services;
using SPMeta2.Syntax.Default;
using SPMeta2.Utils;

namespace SPMeta2.Reverse.CSOM.ReverseHandlers.Fields
{
    public class LookupFieldReverseHandler : FieldReverseHandler
    {
        #region properties
        public override Type ReverseType
        {
            get { return typeof(LookupFieldDefinition); }
        }

        #endregion

        #region methods

        protected override IEnumerable<Field> GetTypedFields(ClientContext context, FieldCollection items)
        {
            var typedFields = context.LoadQuery(
                items.Where(i => i.FieldTypeKind == FieldType.Lookup)
                     .IncludeWithDefaultProperties());

            context.ExecuteQueryWithTrace();

            return typedFields;
        }

        protected override FieldDefinition GetFieldDefinitionInstance()
        {
            return new LookupFieldDefinition();
        }

        protected override ModelNode GetFieldModelNodeInstance()
        {
            return new LookupFieldModelNode();
        }

        protected override void PostProcessFieldDefinitionInstance(FieldDefinition def, FieldReverseHost typedReverseHost, ReverseOptions options)
        {
            var context = typedReverseHost.HostClientContext;

            var typedField = context.CastTo<FieldLookup>(typedReverseHost.Field);
            var typedDef = def.WithAssertAndCast<LookupFieldDefinition>("modelHost", m => m.RequireNotNull());

            typedDef.AllowMultipleValues = typedField.AllowMultipleValues;

            if (typedDef.AllowMultipleValues)
                typedDef.FieldType = BuiltInFieldTypes.LookupMulti;
            else
                typedDef.FieldType = BuiltInFieldTypes.Lookup;

            //typedDef.AppendOnly = typedField.AppendOnly;
            //typedDef.RichText = typedField.RichText;

            //typedDef.NumberOfLines = typedField.NumberOfLines;

            //var xml = XDocument.Parse(typedField.SchemaXml);
            //var fieldXml = xml.Root;

            //var unlimValue = ConvertUtils.ToBool(fieldXml.GetAttributeValue("UnlimitedLengthInDocumentLibrary"));
            //typedDef.UnlimitedLengthInDocumentLibrary = unlimValue.HasValue ? unlimValue.Value : false;

            //var richTextMode = ConvertUtils.ToString(fieldXml.GetAttributeValue("RichTextMode"));
            //typedDef.RichTextMode = richTextMode;
        }

        #endregion
    }
}
