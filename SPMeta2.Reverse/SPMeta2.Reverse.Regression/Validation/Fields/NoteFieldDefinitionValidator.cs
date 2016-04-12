using System;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Reverse.Regression.Base;
using SPMeta2.Utils;

namespace SPMeta2.Reverse.Regression.Validation.Fields
{
    public class NoteFieldDefinitionValidator : FieldDefinitionValidator
    {
        public override Type TargetType
        {
            get { return typeof(NoteFieldDefinition); }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            base.DeployModel(modelHost, model);

            var typedModelHost = modelHost.WithAssertAndCast<ReverseValidationModeHost>("modelHost", m => m.RequireNotNull());

            var originalDefinition = typedModelHost.OriginalModel.Value.WithAssertAndCast<NoteFieldDefinition>("value", m => m.RequireNotNull());
            var reversedDefinition = typedModelHost.ReversedModel.Value.WithAssertAndCast<NoteFieldDefinition>("value", m => m.RequireNotNull());

            var assert = ServiceFactory.AssertService.NewAssert(originalDefinition, reversedDefinition);

            // TODO, typed field validation
            assert
                .ShouldBeEqual(s => s.AppendOnly, r => r.AppendOnly)
                .ShouldBeEqual(s => s.RichText, r => r.RichText)
                .ShouldBeEqual(s => s.RichTextMode, r => r.RichTextMode)
                .ShouldBeEqual(s => s.NumberOfLines, r => r.NumberOfLines)
                .ShouldBeEqual(s => s.UnlimitedLengthInDocumentLibrary, r => r.UnlimitedLengthInDocumentLibrary);
        }
    }
}
