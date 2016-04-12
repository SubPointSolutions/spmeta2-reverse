using SPMeta2.Definitions;
using SPMeta2.Reverse.Regression.Base;
using SPMeta2.Reverse.Regression.Consts;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Utils;

namespace SPMeta2.Reverse.Regression.Validation.Taxonomy
{
    public class TaxonomyTermDefinitionValidator
        : TypedReverseDefinitionValidatorBase<TaxonomyTermDefinition>
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var typedModelHost = modelHost.WithAssertAndCast<ReverseValidationModeHost>("modelHost", m => m.RequireNotNull());

            var originalDefinition = typedModelHost.OriginalModel.Value.WithAssertAndCast<TaxonomyTermDefinition>("value", m => m.RequireNotNull());
            var reversedDefinition = typedModelHost.ReversedModel.Value.WithAssertAndCast<TaxonomyTermDefinition>("value", m => m.RequireNotNull());

            var assert = ServiceFactory.AssertService.NewAssert(originalDefinition, reversedDefinition);

            assert
                .ShouldBeEqual(s => s.Id, r => r.Id)
                .ShouldBeEqual(s => s.Name, r => r.Name)

                .ShouldBeEqual(s => s.Description, r => r.Description)

                //.ShouldBeEqual(s => s.IsOpenForTermCreation, r => r.IsOpenForTermCreation)
                //.ShouldBeEqual(s => s.IsAvailableForTagging, r => r.IsAvailableForTagging)

                .SkipProperty(s => s.CustomSortOrder, SkipMessages.NotImplemented)
                .SkipProperty(s => s.CustomProperties, SkipMessages.NotImplemented)

                .SkipProperty(s => s.LCID, SkipMessages.NotImplemented)
                ;

            if (originalDefinition.IsAvailableForTagging.HasValue)
                assert.ShouldBeEqual(s => s.IsAvailableForTagging, r => r.IsAvailableForTagging);
            else
                assert.SkipProperty(s => s.IsAvailableForTagging, SkipMessages.Skipped);
        }
    }
}
