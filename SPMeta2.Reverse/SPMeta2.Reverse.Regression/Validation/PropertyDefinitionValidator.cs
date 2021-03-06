using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Definitions;
using SPMeta2.Reverse.Regression.Base;
using SPMeta2.Utils;
using SPMeta2.Reverse.Regression.Consts;
using SPMeta2.Containers.Assertion;
using SPMeta2.Reverse.Exceptions;

namespace SPMeta2.Reverse.Regression.Validation
{
    public class PropertyDefinitionValidator : TypedReverseDefinitionValidatorBase<PropertyDefinition>
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var typedModelHost = modelHost.WithAssertAndCast<ReverseValidationModeHost>("modelHost", m => m.RequireNotNull());

            var originalDefinition = typedModelHost.OriginalModel.Value.WithAssertAndCast<PropertyDefinition>("value", m => m.RequireNotNull());
            var reversedDefinition = typedModelHost.ReversedModel.Value.WithAssertAndCast<PropertyDefinition>("value", m => m.RequireNotNull());

            var assert = ServiceFactory.AssertService.NewAssert(originalDefinition, reversedDefinition);

            assert
                .ShouldBeEqual(s => s.Key, r => r.Key)
                .ShouldBeEqual((p, s, d) =>
                {
                    var isValid = true;

                    var srcProp = s.GetExpressionValue(o => o.Value);
                    var dstProp = d.GetExpressionValue(o => o.Value);

                    if (s.Value is string)
                    {
                        isValid = (string)s.Value == (string)d.Value;
                    }
                    else if (s.Value is int)
                    {
                        isValid = (int)s.Value == (int)d.Value;
                    }
                    else
                    {
                        throw new SPMeta2ReverseException("Fix that untyped == thing, finally!");
                    }

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

                    var srcProp = s.GetExpressionValue(o => o.Overwrite);
                    var dstProp = d.GetExpressionValue(o => o.Overwrite);
                    
                    isValid = d.Overwrite == true;

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = dstProp,
                        IsValid = isValid
                    };
                })
                ;

        }
    }
}
