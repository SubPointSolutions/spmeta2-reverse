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
                .ShouldBeEqual(s => s.DefaultValue, r => r.DefaultValue)
                .ShouldBeEqual(s => s.Required, r => r.Required)
                .ShouldBeEqual(s => s.Group, r => r.Group)
                .ShouldBeEqual(s => s.Id, r => r.Id)
                .ShouldBeEqual(s => s.Hidden, r => r.Hidden)
                
                .ShouldBeEqual(s => s.StaticName, r => r.StaticName)
                
                .SkipProperty(s => s.TitleResource, "")
                .SkipProperty(s => s.DescriptionResource, "")
                
                .SkipProperty(s => s.AddFieldOptions, "")
                .SkipProperty(s => s.AdditionalAttributes, "")
                .SkipProperty(s => s.AddToDefaultView, "")
                .SkipProperty(s => s.AllowDeletion, "")
                .SkipProperty(s => s.EnforceUniqueValues, "")
                .SkipProperty(s => s.Indexed, "")
                .SkipProperty(s => s.JSLink, "")
                .SkipProperty(s => s.RawXml, "")

                .SkipProperty(s => s.ShowInDisplayForm, "")
                .SkipProperty(s => s.ShowInEditForm, "")
                .SkipProperty(s => s.ShowInListSettings, "")
                .SkipProperty(s => s.ShowInNewForm, "")
                .SkipProperty(s => s.ShowInVersionHistory, "")
                .SkipProperty(s => s.ShowInViewForms, "")

                .SkipProperty(s => s.ValidationFormula, "")
                .SkipProperty(s => s.ValidationMessage, "")
                ;
        }
    }
}
