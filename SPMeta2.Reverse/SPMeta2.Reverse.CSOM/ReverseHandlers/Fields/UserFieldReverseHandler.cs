using System;
using System.Collections.Generic;
using System.Linq;
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
using SPMeta2.Utils;

namespace SPMeta2.Reverse.CSOM.ReverseHandlers.Fields
{
    public class UserFieldReverseHandler : FieldReverseHandler
    {
        #region properties
        public override Type ReverseType
        {
            get { return typeof(UserFieldDefinition); }
        }

        #endregion

        #region methods

        protected override IEnumerable<Field> GetTypedFields(ClientContext context, FieldCollection items)
        {
            var typedFields = context.LoadQuery(items.Where(i => i.FieldTypeKind == FieldType.User));
            context.ExecuteQuery();

            return typedFields;
        }

        protected override FieldDefinition GetFieldDefinitionInstance()
        {
            return new UserFieldDefinition();
        }

        protected override ModelNode GetFieldModelNodeInstance()
        {
            return new UserFieldModelNode();
        }

        protected override void PostProcessFieldDefinitionInstance(FieldDefinition def, FieldReverseHost typedReverseHost, ReverseOptions options)
        {
            var context = typedReverseHost.HostClientContext;

            var typedField = context.CastTo<FieldUser>(typedReverseHost.Field);
            var typedDef = def.WithAssertAndCast<UserFieldDefinition>("modelHost", m => m.RequireNotNull());

            typedDef.Presence = typedField.Presence;

            if (typedField.SelectionGroup > 0)
            {
                typedDef.SelectionGroup = typedField.SelectionGroup;
            }

            typedDef.AllowDisplay = typedField.AllowDisplay;
            typedDef.SelectionMode = typedField.SelectionMode.ToString();
        }

        #endregion
    }
}
