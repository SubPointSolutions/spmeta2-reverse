using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Definitions;
using SPMeta2.Reverse.Regression.Base;
using SPMeta2.Utils;
using SPMeta2.Reverse.Regression.Consts;
using SPMeta2.Containers.Assertion;

namespace SPMeta2.Reverse.Regression.Validation
{
    public class WebPartPageDefinitionValidator : TypedReverseDefinitionValidatorBase<WebPartPageDefinition>
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var typedModelHost = modelHost.WithAssertAndCast<ReverseValidationModeHost>("modelHost", m => m.RequireNotNull());

            var originalDefinition = typedModelHost.OriginalModel.Value.WithAssertAndCast<WebPartPageDefinition>("value", m => m.RequireNotNull());
            var reversedDefinition = typedModelHost.ReversedModel.Value.WithAssertAndCast<WebPartPageDefinition>("value", m => m.RequireNotNull());

            var assert = ServiceFactory.AssertService.NewAssert(originalDefinition, reversedDefinition);

            assert
                .ShouldBeEqual(s => s.FileName, r => r.FileName)

                .SkipProperty(s => s.Title, SkipMessages.NotImplemented)

                .SkipProperty(s => s.ContentTypeId, SkipMessages.NotImplemented)
                .SkipProperty(s => s.ContentTypeName, SkipMessages.NotImplemented)

                .SkipProperty(s => s.DefaultValues, SkipMessages.NotImplemented)

                .ShouldBeEqual((p, s, d) =>
                {
                    var isValid = true;

                    var srcProp = s.GetExpressionValue(o => o.CustomPageLayout);
                    var dstProp = d.GetExpressionValue(o => o.CustomPageLayout);

                    // custom page layout should not be null or empty
                    //
                    // we don't know what kind of web part page template was there
                    // always reverse to custom
                    isValid = !string.IsNullOrEmpty(d.CustomPageLayout);

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = dstProp,
                        IsValid = isValid
                    };
                })

                .ShouldBeEqual((p, s, d) =>
                {
                    var isValid = true;

                    var srcProp = s.GetExpressionValue(o => o.NeedOverride);
                    var dstProp = d.GetExpressionValue(o => o.NeedOverride);

                    isValid = d.NeedOverride == true;

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = dstProp,
                        IsValid = isValid
                    };
                })

                .SkipProperty(s => s.PageLayoutTemplate, SkipMessages.UserDefined)

                .SkipProperty(s => s.Values, SkipMessages.NotImplemented)
                .SkipProperty(s => s.DefaultValues, SkipMessages.NotImplemented)
                ;

        }
    }
}
