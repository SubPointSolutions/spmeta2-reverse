using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers;
using SPMeta2.Definitions;
using SPMeta2.Reverse.CSOM.ReverseHandlers;
using SPMeta2.Reverse.CSOM.Services;
using SPMeta2.Reverse.Services;
using SPMeta2.Reverse.Tests.Base;
using SPMeta2.Standard.Syntax;
using SPMeta2.Syntax.Default;
using SPMeta2.Standard.Definitions.Taxonomy;

namespace SPMeta2.Reverse.Tests.Impl.Definitions
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
