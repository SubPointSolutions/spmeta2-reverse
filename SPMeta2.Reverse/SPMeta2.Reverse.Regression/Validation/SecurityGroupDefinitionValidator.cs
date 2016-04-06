using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;
using SPMeta2.Reverse.Regression.Base;
using SPMeta2.Utils;
using SPMeta2.Reverse.Regression.Consts;

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

                .SkipProperty(s => s.AllowMembersEditMembership, SkipMessages.NotImplemented)
                .SkipProperty(s => s.AllowRequestToJoinLeave, SkipMessages.NotImplemented)
                .SkipProperty(s => s.AutoAcceptRequestToJoinLeave, SkipMessages.NotImplemented)

                .SkipProperty(s => s.DefaultUser, SkipMessages.NotImplemented)

                .SkipProperty(s => s.Owner, SkipMessages.NotImplemented)
                .SkipProperty(s => s.OnlyAllowMembersViewMembership, SkipMessages.NotImplemented)
                ;
        }
    }
}
