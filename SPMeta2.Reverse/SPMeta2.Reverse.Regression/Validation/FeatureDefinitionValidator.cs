using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Containers.Assertion;
using SPMeta2.Definitions;
using SPMeta2.Reverse.Regression.Base;
using SPMeta2.Reverse.Regression.Consts;
using SPMeta2.Utils;

namespace SPMeta2.Reverse.Regression.Validation
{
    public class FeatureDefinitionValidator : TypedReverseDefinitionValidatorBase<FeatureDefinition>
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var typedModelHost = modelHost.WithAssertAndCast<ReverseValidationModeHost>("modelHost", m => m.RequireNotNull());

            var originalDefinition = typedModelHost.OriginalModel.Value.WithAssertAndCast<FeatureDefinition>("value", m => m.RequireNotNull());
            var reversedDefinition = typedModelHost.ReversedModel.Value.WithAssertAndCast<FeatureDefinition>("value", m => m.RequireNotNull());

            var assert = ServiceFactory.AssertService.NewAssert(originalDefinition, reversedDefinition);

            assert
                .ShouldBeEqual(s => s.Id, r => r.Id)
                .SkipProperty(s => s.ForceActivate, SkipMessages.UserDefined)
                .ShouldBeEqual((p, s, d) =>
                {
                    var isValid = true;

                    var srcProp = s.GetExpressionValue(o => o.Scope);
                    var dstProp = d.GetExpressionValue(o => o.Scope);

                    isValid = s.Scope.ToString().ToUpper() == d.Scope.ToString().ToUpper();

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = dstProp,
                        IsValid = isValid
                    };
                })
                .ShouldBeEqual(s => s.Enable, r => r.Enable)
                ;
        }
    }
}
