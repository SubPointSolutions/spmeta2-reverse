using System;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Reverse.Regression.Base;
using SPMeta2.Utils;
using SPMeta2.Containers.Assertion;

namespace SPMeta2.Reverse.Regression.Validation.Fields
{
    public class MultiChoiceFieldDefinitionValidator : FieldDefinitionValidator
    {
        public override Type TargetType
        {
            get { return typeof(MultiChoiceFieldDefinition); }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            base.DeployModel(modelHost, model);

            var typedModelHost = modelHost.WithAssertAndCast<ReverseValidationModeHost>("modelHost", m => m.RequireNotNull());

            var originalDefinition = typedModelHost.OriginalModel.Value.WithAssertAndCast<MultiChoiceFieldDefinition>("value", m => m.RequireNotNull());
            var reversedDefinition = typedModelHost.ReversedModel.Value.WithAssertAndCast<MultiChoiceFieldDefinition>("value", m => m.RequireNotNull());

            var assert = ServiceFactory.AssertService.NewAssert(originalDefinition, reversedDefinition);

            // TODO, typed field validation
            assert
                .ShouldBeEqual(s => s.FillInChoice, r => r.FillInChoice);

            assert.ShouldBeEqual((p, s, d) =>
            {
                var isValid = true;

                var srcProp = s.GetExpressionValue(o => o.Choices);
                var dstProp = d.GetExpressionValue(o => o.Choices);

                foreach (var srcChoice in s.Choices)
                {
                    if (!d.Choices.Contains(srcChoice))
                    {
                        isValid = false;
                        break;
                    }
                }

                return new PropertyValidationResult
                {
                    Tag = p.Tag,
                    Src = srcProp,
                    Dst = dstProp,
                    IsValid = isValid
                };
            });

            assert.ShouldBeEqual((p, s, d) =>
            {
                var isValid = true;

                var srcProp = s.GetExpressionValue(o => o.Mappings);
                var dstProp = d.GetExpressionValue(o => o.Mappings);

                foreach (var srcChoice in s.Mappings)
                {
                    if (!d.Mappings.Contains(srcChoice))
                    {
                        isValid = false;
                        break;
                    }
                }

                return new PropertyValidationResult
                {
                    Tag = p.Tag,
                    Src = srcProp,
                    Dst = dstProp,
                    IsValid = isValid
                };
            });
        }
    }
}
