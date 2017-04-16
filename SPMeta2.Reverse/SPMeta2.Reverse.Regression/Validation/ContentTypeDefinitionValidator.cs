using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Definitions;
using SPMeta2.Reverse.Regression.Base;
using SPMeta2.Syntax.Default;
using SPMeta2.Utils;
using SPMeta2.Reverse.Regression.Consts;
using SPMeta2.Utils;

namespace SPMeta2.Reverse.Regression.Validation
{
    public class ContentTypeDefinitionValidator : TypedReverseDefinitionValidatorBase<ContentTypeDefinition>
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var typedModelHost = modelHost.WithAssertAndCast<ReverseValidationModeHost>("modelHost", m => m.RequireNotNull());

            var originalDefinition = typedModelHost.OriginalModel.Value.WithAssertAndCast<ContentTypeDefinition>("value", m => m.RequireNotNull());
            var reversedDefinition = typedModelHost.ReversedModel.Value.WithAssertAndCast<ContentTypeDefinition>("value", m => m.RequireNotNull());

            var assert = ServiceFactory.AssertService.NewAssert(originalDefinition, reversedDefinition);

            assert
                .ShouldBeEqual(s => s.Name, r => r.Name)
                .ShouldBeEqual(s => s.Description, r => r.Description)
                .ShouldBeEqual(s => s.Group, r => r.Group)

                .ShouldBeEqual(s => s.Hidden, r => r.Hidden)

                .ShouldBeEqual(s => s.Id, r => r.Id)
                .ShouldBeEqual(s => s.ParentContentTypeId, r => r.ParentContentTypeId)

                .SkipProperty(s => s.ParentContentTypeName, SkipMessages.NotImplemented)
                .SkipProperty(s => s.NameResource, SkipMessages.NotImplemented)
                .SkipProperty(s => s.DescriptionResource, SkipMessages.NotImplemented)
                .SkipProperty(s => s.DocumentTemplate, SkipMessages.NotImplemented)
                .SkipProperty(s => s.IdNumberValue, SkipMessages.NotImplemented)
                ;

            if (originalDefinition.ReadOnly.HasValue)
            {
                assert.ShouldBeEqual(s => s.ReadOnly, r => r.ReadOnly);
            }
            else
            {
                assert.SkipProperty(s => s.ReadOnly, SkipMessages.NotImplemented);
            }

            if (originalDefinition.Sealed.HasValue)
            {
                assert.ShouldBeEqual(s => s.Sealed, r => r.Sealed);
            }
            else
            {
                assert.SkipProperty(s => s.Sealed, SkipMessages.NotImplemented);
            }
        }
    }
}
