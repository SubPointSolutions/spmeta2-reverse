using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;
using SPMeta2.Reverse.Regression.Base;
using SPMeta2.Utils;
using SPMeta2.Containers.Assertion;

namespace SPMeta2.Reverse.Regression.Validation
{
    public class SecurityRoleDefinitionValidator : TypedReverseDefinitionValidatorBase<SecurityRoleDefinition>
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var typedModelHost = modelHost.WithAssertAndCast<ReverseValidationModeHost>("modelHost", m => m.RequireNotNull());

            var originalDefinition = typedModelHost.OriginalModel.Value.WithAssertAndCast<SecurityRoleDefinition>("value", m => m.RequireNotNull());
            var reversedDefinition = typedModelHost.ReversedModel.Value.WithAssertAndCast<SecurityRoleDefinition>("value", m => m.RequireNotNull());

            var assert = ServiceFactory.AssertService.NewAssert(originalDefinition, reversedDefinition);

            assert
                .ShouldBeEqual(s => s.Name, r => r.Name)
                .ShouldBeEqual(s => s.Description, r => r.Description)
                .ShouldBeEqual((p, s, d) =>
                {
                    var isValid = true;

                    var srcProp = s.GetExpressionValue(o => o.BasePermissions);
                    var dstProp = d.GetExpressionValue(o => o.BasePermissions);

                    foreach (var sourcePermission in s.BasePermissions)
                    {
                        if (!d.BasePermissions.Contains(sourcePermission))
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
            ;
        }
    }
}
