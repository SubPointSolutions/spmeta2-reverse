using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Definitions;
using SPMeta2.Reverse.Regression.Base;
using SPMeta2.Utils;
using SPMeta2.Reverse.Regression.Consts;
using SPMeta2.Containers.Assertion;
using System.IO;

namespace SPMeta2.Reverse.Regression.Validation
{
    public class SandboxSolutionDefinitionValidator : TypedReverseDefinitionValidatorBase<SandboxSolutionDefinition>
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var typedModelHost = modelHost.WithAssertAndCast<ReverseValidationModeHost>("modelHost", m => m.RequireNotNull());

            var originalDefinition = typedModelHost.OriginalModel.Value.WithAssertAndCast<SandboxSolutionDefinition>("value", m => m.RequireNotNull());
            var reversedDefinition = typedModelHost.ReversedModel.Value.WithAssertAndCast<SandboxSolutionDefinition>("value", m => m.RequireNotNull());

            var assert = ServiceFactory.AssertService.NewAssert(originalDefinition, reversedDefinition);

            assert
                //.ShouldBeEqual(s => s.FileName, r => r.FileName)
                .ShouldBeEqual(s => s.SolutionId, r => r.SolutionId)

                .ShouldBeEqual((p, s, d) =>
                {
                    var isValid = false;

                    var srcProp = s.GetExpressionValue(o => o.FileName);
                    var dstProp = d.GetExpressionValue(o => o.FileName);

                    var srcFileNameWithoutExtension = Path.GetFileNameWithoutExtension(s.FileName);

                    // check for file name and *.wp presense
                    // with CSOM the target file would  be 'my-solution-v1.0.wsp'
                    isValid = d.FileName.Contains(srcFileNameWithoutExtension)
                              && d.FileName.Contains(".wsp");


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
                    var isValid = false;

                    var srcProp = s.GetExpressionValue(o => o.Activate);
                    var dstProp = d.GetExpressionValue(o => o.Activate);

                    // always true for CSOM and in reverse
                    isValid = d.Activate == true;

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
                    var isValid = false;

                    var srcProp = s.GetExpressionValue(o => o.Content);
                    var dstProp = d.GetExpressionValue(o => o.Content);

                    if (d.Content == null)
                    {
                        isValid = false;
                    }
                    else if (s.Content.Length != d.Content.Length)
                    {
                        isValid = false;
                    }
                    else
                    {
                        isValid = s.Content.SequenceEqual(d.Content);
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
