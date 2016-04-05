using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;
using SPMeta2.Reverse.Regression.Base;
using SPMeta2.Syntax.Default;
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

                .SkipProperty(s => s.ParentContentTypeName, "Not implemented")
                .SkipProperty(s => s.NameResource, "Not implemented")
                .SkipProperty(s => s.DescriptionResource, "Not implemented")
                .SkipProperty(s => s.DocumentTemplate, "Not implemented")
                .SkipProperty(s => s.IdNumberValue, "Not implemented")
                ;

            if (originalDefinition.ReadOnly.HasValue)
            {
                assert.ShouldBeEqual(s => s.ReadOnly, r => r.ReadOnly);
            }
            else
            {
                assert.SkipProperty(s => s.ReadOnly, "Not implemented");
            }

            if (originalDefinition.Sealed.HasValue)
            {
                assert.ShouldBeEqual(s => s.Sealed, r => r.Sealed);
            }
            else
            {
                assert.SkipProperty(s => s.Sealed, "Not implemented");
            }
        }
    }
}
