using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;
using SPMeta2.Reverse.Regression.Base;
using SPMeta2.Utils;

namespace SPMeta2.Reverse.Regression.Validation
{
    public class SecurityGroupDefinitionValidator : TypedReverseDefinitionValidatorBase<SecurityGroupDefinition>
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var typedModelHost = modelHost.WithAssertAndCast<ReverseValidationModeHost>("modelHost", m => m.RequireNotNull());

            var originalDefinition = typedModelHost.OriginalModel.Value.WithAssertAndCast<SecurityGroupDefinition>("value", m => m.RequireNotNull());
            var reversedDefinition = typedModelHost.ReversedModel.Value.WithAssertAndCast<SecurityGroupDefinition>("value", m => m.RequireNotNull());

            var assert = ServiceFactory.AssertService.NewAssert(originalDefinition, reversedDefinition);

            assert
                .ShouldBeEqual(s => s.Name, r => r.Name)
                .ShouldBeEqual(s => s.Description, r => r.Description)

                .SkipProperty(s => s.AllowMembersEditMembership, "")
                .SkipProperty(s => s.AllowRequestToJoinLeave, "")
                .SkipProperty(s => s.AutoAcceptRequestToJoinLeave, "")

                .SkipProperty(s => s.DefaultUser, "")

                .SkipProperty(s => s.Owner, "")
                .SkipProperty(s => s.OnlyAllowMembersViewMembership, "")
                ;
        }
    }
}
