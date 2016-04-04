using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Reverse.CSOM.Foundation.ReverseHandlers;
using SPMeta2.Reverse.CSOM.Foundation.Services;
using SPMeta2.Reverse.Services;
using SPMeta2.Reverse.Tests.Base;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Reverse.Tests.Impl.Definitions
{
    [TestClass]
    public class FeatureDefinitionTests : ReverseTestBase
    {
        #region tests

        [TestMethod]
        [TestCategory("Features")]
        public void Can_Reverse_Site_Features()
        {
            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddSiteFeature(BuiltInSiteFeatures.BasicWebParts.Enable().Clone<FeatureDefinition>());
                site.AddSiteFeature(BuiltInSiteFeatures.Workflows.Enable().Clone<FeatureDefinition>());
            });

            DeployReverseAndTestModel(model);
        }

        [TestMethod]
        [TestCategory("Features")]
        public void Can_Reverse_Web_Features()
        {
            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddWebFeature(BuiltInWebFeatures.MinimalDownloadStrategy.Enable().Clone<FeatureDefinition>());
                web.AddWebFeature(BuiltInWebFeatures.WikiPageHomePage.Enable().Clone<FeatureDefinition>());
            });

            DeployReverseAndTestModel(model);
        }

        #endregion
    }
}
