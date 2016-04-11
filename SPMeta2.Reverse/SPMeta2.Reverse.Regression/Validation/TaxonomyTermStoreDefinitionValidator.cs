using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;
using SPMeta2.Reverse.Regression.Base;
using SPMeta2.Utils;
using SPMeta2.Reverse.Regression.Consts;
using SPMeta2.Standard.Definitions.Taxonomy;

namespace SPMeta2.Reverse.Regression.Validation
{
    public class TaxonomyTermStoreDefinitionValidator : TypedReverseDefinitionValidatorBase<TaxonomyTermStoreDefinition>
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var typedModelHost = modelHost.WithAssertAndCast<ReverseValidationModeHost>("modelHost", m => m.RequireNotNull());

            var originalDefinition = typedModelHost.OriginalModel.Value.WithAssertAndCast<TaxonomyTermStoreDefinition>("value", m => m.RequireNotNull());
            var reversedDefinition = typedModelHost.ReversedModel.Value.WithAssertAndCast<TaxonomyTermStoreDefinition>("value", m => m.RequireNotNull());

            var assert = ServiceFactory.AssertService.NewAssert(originalDefinition, reversedDefinition);

            assert
                .ShouldBeEqual(s => s.Id, r => r.Id)
                .ShouldBeEqual(s => s.UseDefaultSiteCollectionTermStore, r => r.UseDefaultSiteCollectionTermStore)
                ;

            if (!string.IsNullOrEmpty(originalDefinition.Name))
                assert.ShouldBeEqual(s => s.Name, r => r.Name);
            else
                assert.SkipProperty(s => s.Name, SkipMessages.Skipped);
        }
    }
}
