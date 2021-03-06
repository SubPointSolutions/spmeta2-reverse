using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;
using SPMeta2.Reverse.Regression.Base;
using SPMeta2.Utils;
using SPMeta2.Reverse.Regression.Consts;
using SPMeta2.Containers.Assertion;

namespace SPMeta2.Reverse.Regression.Validation
{
    public class TaxonomyFieldDefinitionValidator : TypedReverseDefinitionValidatorBase<TaxonomyFieldDefinition>
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var typedModelHost = modelHost.WithAssertAndCast<ReverseValidationModeHost>("modelHost", m => m.RequireNotNull());

            var originalDefinition = typedModelHost.OriginalModel.Value.WithAssertAndCast<TaxonomyFieldDefinition>("value", m => m.RequireNotNull());
            var reversedDefinition = typedModelHost.ReversedModel.Value.WithAssertAndCast<TaxonomyFieldDefinition>("value", m => m.RequireNotNull());

            var assert = ServiceFactory.AssertService.NewAssert(originalDefinition, reversedDefinition);

            //assert
            //    .ShouldBeEqual(s => s.Name, r => r.Name)
            //    .SkipProperty(s => s.Name, SkipMessages.Skipped)
            //    .ShouldBeEqual((p, s, d) =>
            //    {
            //        var isValid = true;

            //        var srcProp = s.GetExpressionValue(o => o.Scope);
            //        var dstProp = d.GetExpressionValue(o => o.Scope);

            //        // TODO

            //        return new PropertyValidationResult
            //        {
            //            Tag = p.Tag,
            //            Src = srcProp,
            //            Dst = dstProp,
            //            IsValid = isValid
            //        };
            //    })
            //    ;

        }
    }
}
