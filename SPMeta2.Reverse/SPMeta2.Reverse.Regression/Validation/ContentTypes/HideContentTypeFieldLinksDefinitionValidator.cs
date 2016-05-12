using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;
using SPMeta2.Reverse.Regression.Base;
using SPMeta2.Definitions.ContentTypes;
using SPMeta2.Utils;
using SPMeta2.Containers.Assertion;

namespace SPMeta2.Reverse.Regression.Validation.ContentTypes
{
    public class HideContentTypeFieldLinksDefinitionValidator :
        TypedReverseDefinitionValidatorBase<HideContentTypeFieldLinksDefinition>
    {

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var typedModelHost = modelHost.WithAssertAndCast<ReverseValidationModeHost>("modelHost", m => m.RequireNotNull());

            var originalDefinition = typedModelHost.OriginalModel.Value.WithAssertAndCast<HideContentTypeFieldLinksDefinition>("value", m => m.RequireNotNull());
            var reversedDefinition = typedModelHost.ReversedModel.Value.WithAssertAndCast<HideContentTypeFieldLinksDefinition>("value", m => m.RequireNotNull());

            var assert = ServiceFactory.AssertService.NewAssert(originalDefinition, reversedDefinition);

            assert.ShouldBeEqual((p, s, d) =>
            {
                var isValid = true;

                var srcProp = s.GetExpressionValue(o => o.Fields);
                var dstProp = d.GetExpressionValue(o => o.Fields);

                // check the reversed order, should be the same
                // these aray should be the same in terms of ordering
                var srcArray = s.Fields.Select(l => l.Id).ToList();
                var dstArray = d.Fields.Where(f => s.Fields.Any(sl => sl.Id == f.Id))
                                       .Select(l => l.Id).ToList();

                isValid = srcArray.Count == dstArray.Count;

                if (isValid)
                {
                    for (var i = 0; i < srcArray.Count; i++)
                    {
                        isValid = dstArray.Contains(srcArray[i]);

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
