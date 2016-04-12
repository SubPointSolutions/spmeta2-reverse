using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Reverse.Tests.Base;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Standard.Syntax;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Reverse.Tests.Impl.Definitions.Taxonomy
{
    [TestClass]
    public class TaxonomyTermStoreDefinitionTests : ReverseTestBase
    {
        #region tests

        [TestMethod]
        [TestCategory("TermStore")]
        public void Can_Reverse_Site_TermStore()
        {
            var siteTermStore = new TaxonomyTermStoreDefinition
            {
                Id = null,
                Name = string.Empty,
                UseDefaultSiteCollectionTermStore = true
            };

            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddTaxonomyTermStore(siteTermStore);

            });

            DeployReverseAndTestModel(model);
        }

        #endregion
    }
}
