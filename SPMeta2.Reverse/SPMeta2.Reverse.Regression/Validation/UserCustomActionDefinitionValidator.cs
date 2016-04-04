using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;
using SPMeta2.Reverse.Regression.Base;
using SPMeta2.Syntax.Default;
using SPMeta2.Utils;

namespace SPMeta2.Reverse.Regression.Validation
{
    public class UserCustomActionDefinitionValidator : TypedReverseDefinitionValidatorBase<UserCustomActionDefinition>
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var typedModelHost = modelHost.WithAssertAndCast<ReverseValidationModeHost>("modelHost", m => m.RequireNotNull());

            var originalDefinition = typedModelHost.OriginalModel.Value.WithAssertAndCast<UserCustomActionDefinition>("value", m => m.RequireNotNull());
            var reversedDefinition = typedModelHost.ReversedModel.Value.WithAssertAndCast<UserCustomActionDefinition>("value", m => m.RequireNotNull());

            var assert = ServiceFactory.AssertService.NewAssert(originalDefinition, reversedDefinition);

            assert
                .ShouldBeEqual(s => s.Name, r => r.Name)
                .ShouldBeEqual(s => s.Title, r => r.Title)

                .ShouldBeEqual(s => s.ScriptBlock, r => r.ScriptBlock)
                .ShouldBeEqual(s => s.ScriptSrc, r => r.ScriptSrc)

                .ShouldBeEqual(s => s.Location, r => r.Location)

                .ShouldBeEqual(s => s.Sequence, r => r.Sequence)
                .ShouldBeEqual(s => s.Url, r => r.Url)
                
                .ShouldBeEqual(s => s.RegistrationId, r => r.RegistrationId)
                .ShouldBeEqual(s => s.RegistrationType, r => r.RegistrationType)

                .ShouldBeEqual(s => s.Description, r => r.Description)
                .ShouldBeEqual(s => s.Group, r => r.Group);

        }
    }
}
