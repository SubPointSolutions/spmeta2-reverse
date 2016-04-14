using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.Extensions;
using SPMeta2.Definitions;
using SPMeta2.ModelHosts;
using SPMeta2.Models;
using SPMeta2.Reverse.CSOM.ReverseHandlers.Base;
using SPMeta2.Reverse.CSOM.ReverseHosts;
using SPMeta2.Reverse.ReverseHosts;
using SPMeta2.Reverse.Services;
using SPMeta2.Syntax.Default;
using SPMeta2.Definitions.Fields;
using SPMeta2.Utils;

namespace SPMeta2.Reverse.CSOM.ReverseHandlers.Fields
{
    public class MultiChoiceFieldReverseHandler : FieldReverseHandler
    {
        #region properties
        public override Type ReverseType
        {
            get { return typeof(MultiChoiceFieldDefinition); }
        }

        #endregion

        #region methods

        protected override IEnumerable<Field> GetTypedFields(ClientContext context, FieldCollection items)
        {
            var typedFields = context.LoadQuery(
                items.Where(i => i.FieldTypeKind == FieldType.MultiChoice)
                     .IncludeWithDefaultProperties());

            context.ExecuteQueryWithTrace();

            return typedFields;
        }

        protected override FieldDefinition GetFieldDefinitionInstance()
        {
            return new MultiChoiceFieldDefinition();
        }

        protected override ModelNode GetFieldModelNodeInstance()
        {
            return new MultiChoiceFieldModelNode();
        }

        protected override void PostProcessFieldDefinitionInstance(FieldDefinition def, FieldReverseHost typedReverseHost, ReverseOptions options)
        {
            var context = typedReverseHost.HostClientContext;

            var typedField = context.CastTo<FieldMultiChoice>(typedReverseHost.Field);
            var typedDef = def.WithAssertAndCast<MultiChoiceFieldDefinition>("modelHost", m => m.RequireNotNull());

            var xml = XDocument.Parse(typedField.SchemaXml);
            var fieldXml = xml.Root;

            var choices = fieldXml.Descendants("CHOICE")
                                  .Select(v => v.Value)
                                  .ToList();

            if (choices.Any())
                typedDef.Choices = new Collection<string>(choices);

            var mappings = fieldXml.Descendants("MAPPING")
                                  .Select(v => v.Value)
                                  .ToList();

            if (mappings.Any())
                typedDef.Mappings = new Collection<string>(mappings);
        }

        #endregion
    }
}
