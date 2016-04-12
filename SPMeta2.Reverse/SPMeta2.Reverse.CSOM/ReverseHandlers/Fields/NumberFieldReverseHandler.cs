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
using SPMeta2.Validation.Extensions;

namespace SPMeta2.Reverse.CSOM.ReverseHandlers.Fields
{
    public class NumberFieldReverseHandler : FieldReverseHandler
    {
        #region properties
        public override Type ReverseType
        {
            get { return typeof(NumberFieldDefinition); }
        }

        #endregion

        #region methods

        protected override IEnumerable<Field> GetTypedFields(ClientContext context, FieldCollection items)
        {
            var typedFields = context.LoadQuery(
                items.Where(i => i.FieldTypeKind == FieldType.Number)
                     .IncludeWithDefaultProperties());

            context.ExecuteQuery();

            return typedFields;
        }

        protected override FieldDefinition GetFieldDefinitionInstance()
        {
            return new NumberFieldDefinition();
        }

        protected override ModelNode GetFieldModelNodeInstance()
        {
            return new NumberFieldModelNode();
        }

        protected override void PostProcessFieldDefinitionInstance(FieldDefinition def, FieldReverseHost typedReverseHost, ReverseOptions options)
        {
            var context = typedReverseHost.HostClientContext;

            var typedField = context.CastTo<FieldNumber>(typedReverseHost.Field);
            var typedDef = def.WithAssertAndCast<NumberFieldDefinition>("modelHost", m => m.RequireNotNull());

            if (double.MinValue != typedField.MinimumValue)
                typedDef.MinimumValue = typedField.MinimumValue;

            if (double.MaxValue != typedField.MaximumValue)
                typedDef.MaximumValue = typedField.MaximumValue;

            var xml = XDocument.Parse(typedField.SchemaXml);
            var fieldXml = xml.Root;

            var showAsPercentage = ConvertUtils.ToBool(fieldXml.GetAttributeValue("Percentage"));
            typedDef.ShowAsPercentage = showAsPercentage.HasValue ? showAsPercentage.Value : false;

            var decimals = ConvertUtils.ToString(fieldXml.GetAttributeValue("Decimals"));
            if (!string.IsNullOrEmpty(decimals))
            {
                var decimalsIntValue = ConvertUtils.ToInt(decimals);

                if (decimalsIntValue.HasValue)
                {
                    switch (decimalsIntValue)
                    {
                        case -1:
                            {
                                typedDef.DisplayFormat = BuiltInNumberFormatTypes.Automatic;
                            }
                            break;

                        case 0:
                            {
                                typedDef.DisplayFormat = BuiltInNumberFormatTypes.NoDecimal;
                            }
                            break;

                        case 1:
                            {
                                typedDef.DisplayFormat = BuiltInNumberFormatTypes.OneDecimal;
                            }
                            break;

                        case 2:
                            {
                                typedDef.DisplayFormat = BuiltInNumberFormatTypes.TwoDecimals;
                            }
                            break;

                        case 3:
                            {
                                typedDef.DisplayFormat = BuiltInNumberFormatTypes.ThreeDecimals;
                            }
                            break;

                        case 4:
                            {
                                typedDef.DisplayFormat = BuiltInNumberFormatTypes.FourDecimals;
                            }
                            break;
                        case 5:
                            {
                                typedDef.DisplayFormat = BuiltInNumberFormatTypes.FiveDecimals;
                            }
                            break;
                    }
                    
                }
            }
        }

        #endregion
    }
}
