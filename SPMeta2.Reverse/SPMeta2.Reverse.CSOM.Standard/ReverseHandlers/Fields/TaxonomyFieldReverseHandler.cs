using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint.Client;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Models;
using SPMeta2.Reverse.CSOM.ReverseHandlers.Base;
using SPMeta2.Reverse.CSOM.ReverseHosts;
using SPMeta2.Reverse.ReverseHosts;
using SPMeta2.Reverse.Services;
using SPMeta2.Standard.Definitions.Fields;
using SPMeta2.Standard.Enumerations;
using SPMeta2.Standard.Syntax;
using SPMeta2.Utils;
using SPMeta2.CSOM.Extensions;
using SPMeta2.Reverse.CSOM.ReverseHandlers;

namespace SPMeta2.Reverse.CSOM.Standard.ReverseHandlers.Fields
{
    public class TaxonomyFieldReverseHandler : FieldReverseHandler
    {
        #region properties
        public override Type ReverseType
        {
            get { return typeof(TaxonomyFieldDefinition); }
        }

        #endregion

        #region methods

        protected override IEnumerable<Field> GetTypedFields(ClientContext context, FieldCollection items)
        {
            var typedFields = context.LoadQuery(items.Where(i => i.TypeAsString == BuiltInFieldTypes.TaxonomyFieldType));
            context.ExecuteQueryWithTrace();

            var result = new List<Field>();

            result.AddRange(typedFields);

            typedFields = context.LoadQuery(items.Where(i => i.TypeAsString == BuiltInFieldTypes.TaxonomyFieldTypeMulti));
            context.ExecuteQueryWithTrace();

            result.AddRange(typedFields);

            return result;
        }

        protected override FieldDefinition GetFieldDefinitionInstance()
        {
            return new TaxonomyFieldDefinition();
        }

        protected override ModelNode GetFieldModelNodeInstance()
        {
            return new TaxonomyFieldModelNode();
        }

        protected override void PostProcessFieldDefinitionInstance(FieldDefinition def, FieldReverseHost typedReverseHost, ReverseOptions options)
        {

        }

        #endregion
    }
}
