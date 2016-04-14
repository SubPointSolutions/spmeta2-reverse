using System;
using System.Collections.Generic;
using System.Linq;
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

namespace SPMeta2.Reverse.CSOM.ReverseHandlers.Fields
{
    public class GuidFieldReverseHandler : FieldReverseHandler
    {
        #region properties
        public override Type ReverseType
        {
            get { return typeof(GuidFieldDefinition); }
        }

        #endregion

        #region methods

        protected override IEnumerable<Field> GetTypedFields(ClientContext context, FieldCollection items)
        {
            var typedFields = context.LoadQuery(items.Where(i => i.FieldTypeKind == FieldType.Guid));
            context.ExecuteQueryWithTrace();

            return typedFields;
        }

        protected override FieldDefinition GetFieldDefinitionInstance()
        {
            return new GuidFieldDefinition();
        }

        protected override ModelNode GetFieldModelNodeInstance()
        {
            return new GuidFieldModelNode();
        }

        protected override void PostProcessFieldDefinitionInstance(FieldDefinition def, FieldReverseHost typedReverseHost, ReverseOptions options)
        {

        }

        #endregion
    }
}
