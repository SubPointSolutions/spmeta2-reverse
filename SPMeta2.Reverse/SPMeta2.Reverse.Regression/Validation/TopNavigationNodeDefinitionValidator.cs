using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;
using SPMeta2.Reverse.Regression.Base;
using SPMeta2.Utils;
using SPMeta2.Reverse.Regression.Consts;

namespace SPMeta2.Reverse.Regression.Validation
{
    public class TopNavigationNodeDefinitionValidator : TypedReverseDefinitionValidatorBase<TopNavigationNodeDefinition>
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var typedModelHost = modelHost.WithAssertAndCast<ReverseValidationModeHost>("modelHost", m => m.RequireNotNull());

            var originalDefinition = typedModelHost.OriginalModel.Value.WithAssertAndCast<TopNavigationNodeDefinition>("value", m => m.RequireNotNull());
            var reversedDefinition = typedModelHost.ReversedModel.Value.WithAssertAndCast<TopNavigationNodeDefinition>("value", m => m.RequireNotNull());

            var assert = ServiceFactory.AssertService.NewAssert(originalDefinition, reversedDefinition);

            assert
                .ShouldBeEqual(s => s.Title, r => r.Title)
                .ShouldBeEqual(s => s.Url, r => r.Url)
                .ShouldBeEqual(s => s.IsExternal, r => r.IsExternal)

                .SkipProperty(s => s.TitleResource, SkipMessages.NotImplemented)
                .SkipProperty(s => s.IsVisible,SkipMessages.NotImplemented)
                ;
        }
    }
}
