using System;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Reverse.Regression.Base;
using SPMeta2.Utils;

namespace SPMeta2.Reverse.Regression.Validation.Fields
{
    public class TextFieldDefinitionValidator : FieldDefinitionValidator
    {
        public override Type TargetType
        {
            get { return typeof(TextFieldDefinition); }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            base.DeployModel(modelHost, model);

            var typedModelHost = modelHost.WithAssertAndCast<ReverseValidationModeHost>("modelHost", m => m.RequireNotNull());

            var originalDefinition = typedModelHost.OriginalModel.Value.WithAssertAndCast<TextFieldDefinition>("value", m => m.RequireNotNull());
            var reversedDefinition = typedModelHost.ReversedModel.Value.WithAssertAndCast<TextFieldDefinition>("value", m => m.RequireNotNull());

            var assert = ServiceFactory.AssertService.NewAssert(originalDefinition, reversedDefinition);

            assert
                .ShouldBeEqual(s => s.MaxLength, r => r.MaxLength);
        }
    }
}
