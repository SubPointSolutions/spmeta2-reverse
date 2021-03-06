using System;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Reverse.Regression.Base;
using SPMeta2.Reverse.Regression.Consts;
using SPMeta2.Utils;

namespace SPMeta2.Reverse.Regression.Validation.Fields
{
    public class LookupFieldDefinitionValidator : FieldDefinitionValidator
    {
        public override Type TargetType
        {
            get { return typeof(LookupFieldDefinition); }
        }


        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            base.DeployModel(modelHost, model);

            var typedModelHost = modelHost.WithAssertAndCast<ReverseValidationModeHost>("modelHost", m => m.RequireNotNull());

            var originalDefinition = typedModelHost.OriginalModel.Value.WithAssertAndCast<LookupFieldDefinition>("value", m => m.RequireNotNull());
            var reversedDefinition = typedModelHost.ReversedModel.Value.WithAssertAndCast<LookupFieldDefinition>("value", m => m.RequireNotNull());

            var assert = ServiceFactory.AssertService.NewAssert(originalDefinition, reversedDefinition);

            assert
                .ShouldBeEqual(s => s.AllowMultipleValues, r => r.AllowMultipleValues)
                .ShouldBeEqual(s => s.RelationshipDeleteBehavior, r => r.RelationshipDeleteBehavior);

            if (!string.IsNullOrEmpty(originalDefinition.LookupField))
                assert.ShouldBeEqual(s => s.LookupField, r => r.LookupField);
            else
                assert.SkipProperty(s => s.LookupField, SkipMessages.Skipped);

            if (!string.IsNullOrEmpty(originalDefinition.LookupList))
                assert.ShouldBeEqual(s => s.LookupList, r => r.LookupList);
            else
                assert.SkipProperty(s => s.LookupList, SkipMessages.Skipped);

            if (!string.IsNullOrEmpty(originalDefinition.LookupListTitle))
                assert.ShouldBeEqual(s => s.LookupListTitle, r => r.LookupListTitle);
            else
                assert.SkipProperty(s => s.LookupListTitle, SkipMessages.Skipped);

            if (!string.IsNullOrEmpty(originalDefinition.LookupListUrl))
                assert.ShouldBeEqual(s => s.LookupListUrl, r => r.LookupListUrl);
            else
                assert.SkipProperty(s => s.LookupListUrl, SkipMessages.Skipped);

            if (!string.IsNullOrEmpty(originalDefinition.LookupWebUrl))
                assert.ShouldBeEqual(s => s.LookupWebUrl, r => r.LookupWebUrl);
            else
                assert.SkipProperty(s => s.LookupWebUrl, SkipMessages.Skipped);

            if (!originalDefinition.LookupWebId.HasValue)
                assert.ShouldBeEqual(s => s.LookupWebId, r => r.LookupWebId);
            else
                assert.SkipProperty(s => s.LookupWebId, SkipMessages.Skipped);

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
