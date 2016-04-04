using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;
using SPMeta2.Reverse.Regression.Base;
using SPMeta2.Utils;

namespace SPMeta2.Reverse.Regression.Validation
{
    public class ListDefinitionValidator : TypedReverseDefinitionValidatorBase<ListDefinition>
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var typedModelHost = modelHost.WithAssertAndCast<ReverseValidationModeHost>("modelHost", m => m.RequireNotNull());

            var originalDefinition = typedModelHost.OriginalModel.Value.WithAssertAndCast<ListDefinition>("value", m => m.RequireNotNull());
            var reversedDefinition = typedModelHost.ReversedModel.Value.WithAssertAndCast<ListDefinition>("value", m => m.RequireNotNull());

            var assert = ServiceFactory.AssertService.NewAssert(originalDefinition, reversedDefinition);

            assert
                .ShouldBeEqual(s => s.Title, r => r.Title)
                .ShouldBeEqual(s => s.Description, r => r.Description)

                .ShouldBeEqual(s => s.TemplateType, r => r.TemplateType)
                .ShouldBeEqual(s => s.Hidden, r => r.Hidden)
                .ShouldBeEqual(s => s.ContentTypesEnabled, r => r.ContentTypesEnabled)

                .ShouldBeEqual(s => s.OnQuickLaunch, r => r.OnQuickLaunch)
                ;
                
        }
    }
}
