using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Reverse.Services;
using SPMeta2.Reverse.Tests.Base;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Standard.Syntax;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Reverse.Tests.Impl.Definitions.Taxonomy
{
    [TestClass]
    public class TaxonomyTermDefinitionTests : ReverseTestBase
    {
        #region tests

        [TestMethod]
        [TestCategory("Terms")]
        public void Can_Reverse_Terms()
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

            var termSet1 = new TaxonomyTermSetDefinition
            {
                Name = Rnd.String(),
                Id = Rnd.Guid(),
                Contact = Rnd.String(),
                Description = Rnd.String()
            };

            var term1 = new TaxonomyTermDefinition
            {
                Name = Rnd.String(),
                Id = Rnd.Guid(),
                Description = Rnd.String()
            };

            var term2 = new TaxonomyTermDefinition
            {
                Name = Rnd.String(),
                Id = Rnd.Guid(),
                Description = Rnd.String()
            };

            // only witin a tes term group
            // performance boost
            var groupName = termGroup1.Name;
            var options = ReverseOptions.Default
                .AddFilterOption<TaxonomyTermGroupDefinition>(g => g.Name == groupName);

            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddTaxonomyTermStore(siteTermStore, store =>
                {
                    store.AddTaxonomyTermGroup(termGroup1, group =>
                    {
                        group.AddTaxonomyTermSet(termSet1, termSet =>
                        {
                            termSet.AddTaxonomyTerm(term1);
                            termSet.AddTaxonomyTerm(term2);
                        });

                    });
                });

            });

            DeployReverseAndTestModel(model, options);
        }

        #endregion
    }
}
