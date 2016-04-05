﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;
using SPMeta2.Reverse.Regression.Base;
using SPMeta2.Utils;

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

                .SkipProperty(s => s.UseUniquePermission, "")
                .SkipProperty(s => s.CustomWebTemplate, "")

                .SkipProperty(s => s.AlternateCssUrl, "")
                .SkipProperty(s => s.IndexedPropertyKeys, "")
                .SkipProperty(s => s.SiteLogoUrl, "")
                .SkipProperty(s => s.LCID, "")

                .SkipProperty(s => s.TitleResource, "")
                .SkipProperty(s => s.DescriptionResource, "")
                ;
        }
    }
}
