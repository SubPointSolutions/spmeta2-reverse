using System;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Reverse.Regression.Base;
using SPMeta2.Utils;

namespace SPMeta2.Reverse.Regression.Validation.Fields
{
    public class OutcomeFieldDefinitionValidator : FieldDefinitionValidator
    {
        public override Type TargetType
        {
            get { return typeof(OutcomeChoiceFieldDefinition); }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            base.DeployModel(modelHost, model);

            var typedModelHost = modelHost.WithAssertAndCast<ReverseValidationModeHost>("modelHost", m => m.RequireNotNull());

            var originalDefinition = typedModelHost.OriginalModel.Value.WithAssertAndCast<OutcomeChoiceFieldDefinition>("value", m => m.RequireNotNull());
            var reversedDefinition = typedModelHost.ReversedModel.Value.WithAssertAndCast<OutcomeChoiceFieldDefinition>("value", m => m.RequireNotNull());

            var assert = ServiceFactory.AssertService.NewAssert(originalDefinition, reversedDefinition);

            //assert
            //    .ShouldBeEqual(s => s.DisplayFormat, r => r.DisplayFormat)

            //    .ShouldBeEqual(s => s.ShowAsPercentage, r => r.ShowAsPercentage)

            //    .ShouldBeEqual(s => s.MinimumValue, r => r.MinimumValue)
            //    .ShouldBeEqual(s => s.MaximumValue, r => r.MaximumValue);
            
        }
    }
}
