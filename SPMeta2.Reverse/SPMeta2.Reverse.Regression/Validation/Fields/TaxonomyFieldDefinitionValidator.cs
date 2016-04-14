using System;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Reverse.Regression.Base;
using SPMeta2.Reverse.Regression.Consts;
using SPMeta2.Standard.Definitions.Fields;
using SPMeta2.Utils;

namespace SPMeta2.Reverse.Regression.Validation.Fields
{
    public class TaxonomyFieldDefinitionValidator : FieldDefinitionValidator
    {
        public override Type TargetType
        {
            get { return typeof(TaxonomyFieldDefinition); }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            base.DeployModel(modelHost, model);

            var typedModelHost = modelHost.WithAssertAndCast<ReverseValidationModeHost>("modelHost", m => m.RequireNotNull());

            var originalDefinition = typedModelHost.OriginalModel.Value.WithAssertAndCast<TaxonomyFieldDefinition>("value", m => m.RequireNotNull());
            var reversedDefinition = typedModelHost.ReversedModel.Value.WithAssertAndCast<TaxonomyFieldDefinition>("value", m => m.RequireNotNull());

            var assert = ServiceFactory.AssertService.NewAssert(originalDefinition, reversedDefinition);

            assert
                .SkipProperty(s => s.SspId, SkipMessages.NotImplemented)
                .SkipProperty(s => s.SspName, SkipMessages.NotImplemented)

                .SkipProperty(s => s.CreateValuesInEditForm, SkipMessages.NotImplemented)

                .SkipProperty(s => s.IsMulti, SkipMessages.NotImplemented)

                .SkipProperty(s => s.IsPathRendered, SkipMessages.NotImplemented)
                .SkipProperty(s => s.IsSiteCollectionGroup, SkipMessages.NotImplemented)

                .SkipProperty(s => s.Open, SkipMessages.NotImplemented)

                .SkipProperty(s => s.TermGroupId, SkipMessages.NotImplemented)
                .SkipProperty(s => s.TermGroupName, SkipMessages.NotImplemented)

                .SkipProperty(s => s.TermSetId, SkipMessages.NotImplemented)
                .SkipProperty(s => s.TermSetName, SkipMessages.NotImplemented)
                .SkipProperty(s => s.TermSetLCID, SkipMessages.NotImplemented)

                .SkipProperty(s => s.TermId, SkipMessages.NotImplemented)
                .SkipProperty(s => s.TermName, SkipMessages.NotImplemented)
                .SkipProperty(s => s.TermLCID, SkipMessages.NotImplemented)

                .SkipProperty(s => s.UseDefaultSiteCollectionTermStore, SkipMessages.NotImplemented)
                ;

            //assert
            //    .ShouldBeEqual(s => s.AllowDisplay, r => r.AllowDisplay)
            //    .ShouldBeEqual(s => s.AllowMultipleValues, r => r.AllowMultipleValues)
            //    .ShouldBeEqual(s => s.Presence, r => r.Presence)
            //    .ShouldBeEqual(s => s.SelectionGroup, r => r.SelectionGroup)
            //    .ShouldBeEqual(s => s.SelectionGroupName, r => r.SelectionGroupName)

            //    .ShouldBeEqual(s => s.LookupField, r => r.LookupField)

            //    .ShouldBeEqual(s => s.SelectionMode, r => r.SelectionMode);
        }
    }
}
