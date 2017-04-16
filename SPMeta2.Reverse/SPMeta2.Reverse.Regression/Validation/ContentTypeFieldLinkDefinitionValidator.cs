using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Definitions;
using SPMeta2.Reverse.Regression.Base;
using SPMeta2.Syntax.Default;
using SPMeta2.Utils;
using SPMeta2.Reverse.Regression.Consts;

namespace SPMeta2.Reverse.Regression.Validation
{
    public class ContentTypeFieldLinkDefinitionValidator : TypedReverseDefinitionValidatorBase<ContentTypeFieldLinkDefinition>
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var typedModelHost = modelHost.WithAssertAndCast<ReverseValidationModeHost>("modelHost", m => m.RequireNotNull());

            var originalDefinition = typedModelHost.OriginalModel.Value.WithAssertAndCast<ContentTypeFieldLinkDefinition>("value", m => m.RequireNotNull());
            var reversedDefinition = typedModelHost.ReversedModel.Value.WithAssertAndCast<ContentTypeFieldLinkDefinition>("value", m => m.RequireNotNull());

            var assert = ServiceFactory.AssertService.NewAssert(originalDefinition, reversedDefinition);

            assert
                //.ShouldBeEqual(s => s.FieldInternalName, r => r.FieldInternalName)
                .ShouldBeEqual(s => s.FieldId, r => r.FieldId)
                .SkipProperty(s => s.DisplayName, SkipMessages.NotImplemented)
                ;

            // field is is the main one
            // FieldInternalName depends on the test/ case
            if (!string.IsNullOrEmpty(originalDefinition.FieldInternalName))
            {
                assert.ShouldBeEqual(s => s.FieldInternalName, r => r.FieldInternalName);
            }
            else
            {
                assert.SkipProperty(s => s.FieldInternalName, SkipMessages.Skipped);
            }

            if (originalDefinition.Hidden.HasValue)
            {
                assert.ShouldBeEqual(s => s.Hidden, r => r.Hidden);
            }
            else
            {
                assert.SkipProperty(s => s.Hidden, SkipMessages.Skipped);
            }

            if (originalDefinition.Required.HasValue)
            {
                assert.ShouldBeEqual(s => s.Required, r => r.Required);
            }
            else
            {
                assert.SkipProperty(s => s.Required, SkipMessages.Skipped);
            }
        }
    }
}
