using SPMeta2.Definitions;
using SPMeta2.Reverse.Regression.Base;
using SPMeta2.Standard.Definitions.Fields;
using SPMeta2.Utils;
using System;

namespace SPMeta2.Reverse.Regression.Validation.Fields
{
    public class ImageFieldDefinitionValidator : FieldDefinitionValidator
    {
        public override Type TargetType
        {
            get { return typeof(ImageFieldDefinition); }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            base.DeployModel(modelHost, model);

            var typedModelHost = modelHost.WithAssertAndCast<ReverseValidationModeHost>("modelHost", m => m.RequireNotNull());

            var originalDefinition = typedModelHost.OriginalModel.Value.WithAssertAndCast<ImageFieldDefinition>("value", m => m.RequireNotNull());
            var reversedDefinition = typedModelHost.ReversedModel.Value.WithAssertAndCast<ImageFieldDefinition>("value", m => m.RequireNotNull());

            var assert = ServiceFactory.AssertService.NewAssert(originalDefinition, reversedDefinition);

            // TODO, typed field validation

        }
    }
}
