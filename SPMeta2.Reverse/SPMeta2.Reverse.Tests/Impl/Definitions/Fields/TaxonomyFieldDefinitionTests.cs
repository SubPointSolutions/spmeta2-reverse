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
    public class TaxonomyFieldDefinitionTests : ReverseTestBase
    {
        #region tests

        [TestMethod]
        [TestCategory("Fields.Taxonomy")]
        public void Can_Reverse_Site_TaxonomyFields()
        {
            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddTaxonomyField(Def<TaxonomyFieldDefinition>());
                site.AddTaxonomyField(Def<TaxonomyFieldDefinition>());
            });

            DeployReverseAndTestModel(model);
        }

        [TestMethod]
        [TestCategory("Fields.Taxonomy")]
        public void Can_Reverse_Web_TaxonomyFields()
        {
            var options = ReverseOptions.Default
                            .AddDepthOption<WebDefinition>(0);

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddTaxonomyField(Def<TaxonomyFieldDefinition>());
                web.AddTaxonomyField(Def<TaxonomyFieldDefinition>());
            });

            DeployReverseAndTestModel(model, options);
        }

        #endregion
    }
}
