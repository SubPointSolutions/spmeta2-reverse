using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Reverse.Services;
using SPMeta2.Reverse.Tests.Base;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Standard.Syntax;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Reverse.Tests.Impl.Definitions.Taxonomy
{
    [TestClass]
    public class TaxonomyTermGroupDefinitionTests : ReverseTestBase
    {
        #region tests

        [TestMethod]
        [TestCategory("TermGroups")]
        public void Can_Reverse_TermGroups()
        {
            var siteTermStore = new TaxonomyTermStoreDefinition
            {
                Id = null,
                Name = string.Empty,
                UseDefaultSiteCollectionTermStore = true
            };

            var termGroup1 = new TaxonomyTermGroupDefinition
            {
                Name = Rnd.String(),
                Id = Rnd.Guid()
            };

            var termGroup2 = new TaxonomyTermGroupDefinition
            {
                Name = Rnd.String(),
                Id = Rnd.Guid()
            };


            
            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddTaxonomyTermStore(siteTermStore, store =>
                {
                    store.AddTaxonomyTermGroup(termGroup1);
                    store.AddTaxonomyTermGroup(termGroup2);
                });

            });

            DeployReverseAndTestModel(model);
        }

        #endregion
    }
}
