using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Definitions;
using SPMeta2.Reverse.Regression.Base;
using SPMeta2.Utils;
using SPMeta2.Reverse.Regression.Consts;

namespace SPMeta2.Reverse.Regression.Validation
{
    public class WebDefinitionValidator : TypedReverseDefinitionValidatorBase<WebDefinition>
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var typedModelHost = modelHost.WithAssertAndCast<ReverseValidationModeHost>("modelHost", m => m.RequireNotNull());

            var originalDefinition = typedModelHost.OriginalModel.Value.WithAssertAndCast<WebDefinition>("value", m => m.RequireNotNull());
            var reversedDefinition = typedModelHost.ReversedModel.Value.WithAssertAndCast<WebDefinition>("value", m => m.RequireNotNull());

            var assert = ServiceFactory.AssertService.NewAssert(originalDefinition, reversedDefinition);

            assert
                .ShouldBeEqual(s => s.Title, r => r.Title)
                .ShouldBeEqual(s => s.Description, r => r.Description)
                .ShouldBeEqual(s => s.WebTemplate, r => r.WebTemplate)
                .ShouldBeEqual(s => s.Url, r => r.Url)

                .SkipProperty(s => s.UseUniquePermission, SkipMessages.NotImplemented)
                .SkipProperty(s => s.CustomWebTemplate, SkipMessages.NotImplemented)

                .SkipProperty(s => s.AlternateCssUrl, SkipMessages.NotImplemented)
                .SkipProperty(s => s.IndexedPropertyKeys, SkipMessages.NotImplemented)
                .SkipProperty(s => s.SiteLogoUrl, SkipMessages.NotImplemented)
                .SkipProperty(s => s.LCID, SkipMessages.NotImplemented)

                .SkipProperty(s => s.TitleResource, SkipMessages.NotImplemented)
                .SkipProperty(s => s.DescriptionResource, SkipMessages.NotImplemented)
                ;
        }
    }
}
