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

                .ShouldBeEqual(s => s.CustomUrl, r => r.CustomUrl)

                .ShouldBeEqual(s => s.TemplateType, r => r.TemplateType)
                .ShouldBeEqual(s => s.Hidden, r => r.Hidden)
                .ShouldBeEqual(s => s.ContentTypesEnabled, r => r.ContentTypesEnabled)

                .ShouldBeEqual(s => s.OnQuickLaunch, r => r.OnQuickLaunch)

                .SkipProperty(s => s.TitleResource, SkipMessages.NotImplemented)
                .SkipProperty(s => s.DescriptionResource, SkipMessages.NotImplemented)

                .SkipProperty(s => s.TemplateName, SkipMessages.NotImplemented)
                .SkipProperty(s => s.DocumentTemplateUrl, SkipMessages.NotImplemented)

                .SkipProperty(s => s.DraftVersionVisibility, SkipMessages.NotImplemented)

                .SkipProperty(s => s.EnableAttachments, SkipMessages.NotImplemented)
                .SkipProperty(s => s.EnableFolderCreation, SkipMessages.NotImplemented)
                .SkipProperty(s => s.EnableMinorVersions, SkipMessages.NotImplemented)
                .SkipProperty(s => s.EnableModeration, SkipMessages.NotImplemented)
                .SkipProperty(s => s.EnableVersioning, SkipMessages.NotImplemented)

                .SkipProperty(s => s.ForceCheckout, SkipMessages.NotImplemented)

                .SkipProperty(s => s.IndexedRootFolderPropertyKeys, SkipMessages.NotImplemented)

                .SkipProperty(s => s.IrmEnabled, SkipMessages.NotImplemented)
                .SkipProperty(s => s.IrmExpire, SkipMessages.NotImplemented)
                .SkipProperty(s => s.IrmReject, SkipMessages.NotImplemented)

                .SkipProperty(s => s.MajorVersionLimit, SkipMessages.NotImplemented)
                .SkipProperty(s => s.MajorWithMinorVersionsLimit, SkipMessages.NotImplemented)
                .SkipProperty(s => s.NoCrawl, SkipMessages.NotImplemented)

                .SkipProperty(s => s.Url, SkipMessages.NotImplemented)
                .SkipProperty(s => s.WriteSecurity, SkipMessages.NotImplemented)
                ;

        }
    }
}
