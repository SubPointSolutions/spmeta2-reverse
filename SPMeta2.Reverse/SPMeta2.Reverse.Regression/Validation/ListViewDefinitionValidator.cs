using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;
using SPMeta2.Reverse.Regression.Base;
using SPMeta2.Utils;
using SPMeta2.Containers.Assertion;
using SPMeta2.Reverse.Regression.Consts;

namespace SPMeta2.Reverse.Regression.Validation
{
    public class ListViewDefinitionValidator : TypedReverseDefinitionValidatorBase<ListViewDefinition>
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var typedModelHost = modelHost.WithAssertAndCast<ReverseValidationModeHost>("modelHost", m => m.RequireNotNull());

            var originalDefinition = typedModelHost.OriginalModel.Value.WithAssertAndCast<ListViewDefinition>("value", m => m.RequireNotNull());
            var reversedDefinition = typedModelHost.ReversedModel.Value.WithAssertAndCast<ListViewDefinition>("value", m => m.RequireNotNull());

            var assert = ServiceFactory.AssertService.NewAssert(originalDefinition, reversedDefinition);

            assert
                .ShouldBeEqual(s => s.Title, r => r.Title)
                .ShouldBeEqual(s => s.Url, r => r.Url)

                .ShouldBeEqual(s => s.Hidden, r => r.Hidden)

                .ShouldBeEqual(s => s.IsDefault, r => r.IsDefault)
                .ShouldBeEqual(s => s.IsPaged, r => r.IsPaged)
                .ShouldBeEqual(s => s.RowLimit, r => r.RowLimit)
                .ShouldBeEqual(s => s.Query, r => r.Query)

                .ShouldBeEqual((p, s, d) =>
                {
                    var isValid = true;

                    var srcProp = s.GetExpressionValue(o => o.Type);
                    var dstProp = d.GetExpressionValue(o => o.Type);

                    isValid = s.Type.ToUpper() == d.Type.ToUpper();

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
                    var isValid = true;

                    var srcProp = s.GetExpressionValue(o => o.Fields);
                    var dstProp = d.GetExpressionValue(o => o.Fields);

                    foreach (var sourceFields in s.Fields)
                    {
                        if (!d.Fields.Contains(sourceFields))
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
                })

                .SkipProperty(s => s.Aggregations, SkipMessages.NotImplemented)
                .SkipProperty(s => s.AggregationsStatus, SkipMessages.NotImplemented)

                .SkipProperty(s => s.ContentTypeId, SkipMessages.NotImplemented)
                .SkipProperty(s => s.ContentTypeName, SkipMessages.NotImplemented)

                .SkipProperty(s => s.DefaultViewForContentType, SkipMessages.NotImplemented)
                .SkipProperty(s => s.InlineEdit, SkipMessages.NotImplemented)

                .SkipProperty(s => s.JSLink, SkipMessages.NotImplemented)
                .SkipProperty(s => s.TabularView, SkipMessages.NotImplemented)

                .SkipProperty(s => s.TitleResource, SkipMessages.NotImplemented)

                .SkipProperty(s => s.ViewData, SkipMessages.NotImplemented)
                .SkipProperty(s => s.ViewStyleId, SkipMessages.NotImplemented)
                ;

            if (!string.IsNullOrEmpty(originalDefinition.Scope))
                assert.ShouldBeEqual(s => s.Scope, r => r.Scope);
            else
                assert.SkipProperty(s => s.Scope, SkipMessages.NotImplemented);
        }
    }
}
