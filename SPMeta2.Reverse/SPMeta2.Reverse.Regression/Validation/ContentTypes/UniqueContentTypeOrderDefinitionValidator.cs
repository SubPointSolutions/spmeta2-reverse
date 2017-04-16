using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Definitions;
using SPMeta2.Reverse.Regression.Base;
using SPMeta2.Definitions.ContentTypes;
using SPMeta2.Utils;
using SPMeta2.Containers.Assertion;

namespace SPMeta2.Reverse.Regression.Validation.ContentTypes
{
    public class UniqueContentTypeOrderDefinitionValidator :
        TypedReverseDefinitionValidatorBase<UniqueContentTypeOrderDefinition>
    {

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var typedModelHost = modelHost.WithAssertAndCast<ReverseValidationModeHost>("modelHost", m => m.RequireNotNull());

            var originalDefinition = typedModelHost.OriginalModel.Value.WithAssertAndCast<UniqueContentTypeOrderDefinition>("value", m => m.RequireNotNull());
            var reversedDefinition = typedModelHost.ReversedModel.Value.WithAssertAndCast<UniqueContentTypeOrderDefinition>("value", m => m.RequireNotNull());

            var assert = ServiceFactory.AssertService.NewAssert(originalDefinition, reversedDefinition);

            assert.ShouldBeEqual((p, s, d) =>
            {
                var isValid = true;

                var srcProp = s.GetExpressionValue(o => o.ContentTypes);
                var dstProp = d.GetExpressionValue(o => o.ContentTypes);

                // check the reversed order, should be the same
                // these aray should be the same in terms of ordering
                var srcArray = s.ContentTypes.Select(l => l.ContentTypeName).ToList();
                var dstArray = d.ContentTypes.Where(f => s.ContentTypes.Any(sl => sl.ContentTypeName == f.ContentTypeName))
                                       .Select(l => l.ContentTypeName).ToList();

                isValid = srcArray.Count == dstArray.Count;

                if (isValid)
                {
                    for (var i = 0; i < srcArray.Count; i++)
                    {
                        isValid = dstArray[i] == srcArray[i];

                        if (!isValid)
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
