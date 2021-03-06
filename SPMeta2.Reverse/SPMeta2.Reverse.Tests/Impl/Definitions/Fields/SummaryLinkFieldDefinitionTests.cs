using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Reverse.Services;
using SPMeta2.Reverse.Tests.Base;
using SPMeta2.Standard.Definitions.Fields;
using SPMeta2.Standard.Syntax;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Reverse.Tests.Impl.Definitions.Fields
{
    [TestClass]
    public class SummaryLinkFieldDefinitionTests : ReverseTestBase
    {
        #region tests

        [TestMethod]
        [TestCategory("Fields.SummaryLink")]
        public void Can_Reverse_Site_SummaryLinkFields()
        {
            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddSummaryLinkField(Def<SummaryLinkFieldDefinition>());
                site.AddSummaryLinkField(Def<SummaryLinkFieldDefinition>());
            });

            DeployReverseAndTestModel(model);
        }

        [TestMethod]
        [TestCategory("Fields.SummaryLink")]
        public void Can_Reverse_Web_SummaryLinkFields()
        {
            var options = ReverseOptions.Default
                            .AddDepthOption<WebDefinition>(0);

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddSummaryLinkField(Def<SummaryLinkFieldDefinition>());
                web.AddSummaryLinkField(Def<SummaryLinkFieldDefinition>());
            });

            DeployReverseAndTestModel(model, options);
        }

        #endregion
    }
}
