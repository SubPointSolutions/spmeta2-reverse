using System;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Reverse.Regression.Base;
using SPMeta2.Utils;

namespace SPMeta2.Reverse.Regression.Validation.Fields
{
    public class UserFieldDefinitionValidator : FieldDefinitionValidator
    {
        public override Type TargetType
        {
            get { return typeof(UserFieldDefinition); }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            base.DeployModel(modelHost, model);

            var typedModelHost = modelHost.WithAssertAndCast<ReverseValidationModeHost>("modelHost", m => m.RequireNotNull());

            var originalDefinition = typedModelHost.OriginalModel.Value.WithAssertAndCast<UserFieldDefinition>("value", m => m.RequireNotNull());
            var reversedDefinition = typedModelHost.ReversedModel.Value.WithAssertAndCast<UserFieldDefinition>("value", m => m.RequireNotNull());

            var assert = ServiceFactory.AssertService.NewAssert(originalDefinition, reversedDefinition);

            assert
                .ShouldBeEqual(s => s.AllowDisplay, r => r.AllowDisplay)
                .ShouldBeEqual(s => s.AllowMultipleValues, r => r.AllowMultipleValues)
                .ShouldBeEqual(s => s.Presence, r => r.Presence)
                .ShouldBeEqual(s => s.SelectionGroup, r => r.SelectionGroup)
                .ShouldBeEqual(s => s.SelectionGroupName, r => r.SelectionGroupName)

                .ShouldBeEqual(s => s.LookupField, r => r.LookupField)

                .ShouldBeEqual(s => s.SelectionMode, r => r.SelectionMode);
        }
    }
}
