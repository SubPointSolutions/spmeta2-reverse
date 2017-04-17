using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Definitions;
using SPMeta2.Reverse.Regression.Base;
using SPMeta2.Utils;
using SPMeta2.Reverse.Regression.Consts;

namespace SPMeta2.Reverse.Regression.Validation
{
    public class FieldDefinitionValidator : TypedReverseDefinitionValidatorBase<FieldDefinition>
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var typedModelHost = modelHost.WithAssertAndCast<ReverseValidationModeHost>("modelHost", m => m.RequireNotNull());

            var originalDefinition = typedModelHost.OriginalModel.Value.WithAssertAndCast<FieldDefinition>("value", m => m.RequireNotNull());
            var reversedDefinition = typedModelHost.ReversedModel.Value.WithAssertAndCast<FieldDefinition>("value", m => m.RequireNotNull());

            var assert = ServiceFactory.AssertService.NewAssert(originalDefinition, reversedDefinition);

            assert
                .ShouldBeEqual(s => s.Title, r => r.Title)
                .ShouldBeEqual(s => s.Description, r => r.Description)
                .ShouldBeEqual(s => s.InternalName, r => r.InternalName)
                .ShouldBeEqual(s => s.FieldType, r => r.FieldType)
                
                .ShouldBeEqual(s => s.Required, r => r.Required)
                .ShouldBeEqual(s => s.Group, r => r.Group)
                .ShouldBeEqual(s => s.Id, r => r.Id)
                .ShouldBeEqual(s => s.Hidden, r => r.Hidden)
                
                .ShouldBeEqual(s => s.StaticName, r => r.StaticName)
                
                .SkipProperty(s => s.TitleResource, SkipMessages.NotImplemented)
                .SkipProperty(s => s.DescriptionResource, SkipMessages.NotImplemented)
                
                .SkipProperty(s => s.AddFieldOptions, SkipMessages.NotImplemented)
                .SkipProperty(s => s.AdditionalAttributes, SkipMessages.NotImplemented)
                .SkipProperty(s => s.AddToDefaultView, SkipMessages.NotImplemented)
                .SkipProperty(s => s.AllowDeletion, SkipMessages.NotImplemented)
                .SkipProperty(s => s.EnforceUniqueValues, SkipMessages.NotImplemented)
                .SkipProperty(s => s.Indexed, SkipMessages.NotImplemented)
                .SkipProperty(s => s.JSLink, SkipMessages.NotImplemented)
                .SkipProperty(s => s.RawXml, SkipMessages.NotImplemented)

                .SkipProperty(s => s.ShowInDisplayForm, SkipMessages.NotImplemented)
                .SkipProperty(s => s.ShowInEditForm, SkipMessages.NotImplemented)
                .SkipProperty(s => s.ShowInListSettings, SkipMessages.NotImplemented)
                .SkipProperty(s => s.ShowInNewForm, SkipMessages.NotImplemented)
                .SkipProperty(s => s.ShowInVersionHistory, SkipMessages.NotImplemented)
                .SkipProperty(s => s.ShowInViewForms, SkipMessages.NotImplemented)

                .SkipProperty(s => s.ValidationFormula, SkipMessages.NotImplemented)
                .SkipProperty(s => s.ValidationMessage, SkipMessages.NotImplemented)
                ;

            if (!string.IsNullOrEmpty(originalDefinition.DefaultValue))
                assert.ShouldBeEqual(s => s.DefaultValue, r => r.DefaultValue);
            else
                assert.SkipProperty(s => s.DefaultValue, SkipMessages.Skipped);
        }
    }
}
