using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Reverse.CSOM.ReverseHandlers;
using SPMeta2.Reverse.CSOM.Services;
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
        public void Can_Reverse_Site_Features_As_Enabled()
        {
            var options = ReverseOptions.Default
                            .AddDepthOption<WebDefinition>(0);

            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddSiteFeature(BuiltInSiteFeatures.BasicWebParts
                                                       .Clone<FeatureDefinition>()
                                                       .Enable()
                                                       .ForceActivate());

                site.AddSiteFeature(BuiltInSiteFeatures.Workflows
                                                       .Clone<FeatureDefinition>()
                                                       .Enable()
                                                       .ForceActivate());
            });

            DeployReverseAndTestModel(model, options);
        }

        [TestMethod]
        [TestCategory("Features")]
        public void Can_Reverse_Site_Features_As_Disabled()
        {
            var options = ReverseOptions.Default
                            .AddDepthOption<WebDefinition>(0);

            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddSiteFeature(BuiltInSiteFeatures.BasicWebParts
                                                       .Clone<FeatureDefinition>()
                                                       .Disable());

                site.AddSiteFeature(BuiltInSiteFeatures.Workflows
                                                       .Clone<FeatureDefinition>()
                                                       .Disable());
            });

            DeployReverseAndTestModel(model, options);
        }

        [TestMethod]
        [TestCategory("Features")]
        public void Can_Reverse_Web_Features_As_Enabled()
        {
            var options = ReverseOptions.Default
                            .AddDepthOption<WebDefinition>(0);

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddWebFeature(BuiltInWebFeatures.MinimalDownloadStrategy
                                                    .Clone<FeatureDefinition>()
                                                    .Enable()
                                                    .ForceActivate());

                web.AddWebFeature(BuiltInWebFeatures.GettingStarted
                                                    .Clone<FeatureDefinition>()
                                                    .Enable()
                                                    .ForceActivate());
            });

            DeployReverseAndTestModel(model, options);
        }

        [TestMethod]
        [TestCategory("Features")]
        public void Can_Reverse_Web_Features_As_Disabled()
        {
            var options = ReverseOptions.Default
                            .AddDepthOption<WebDefinition>(0);

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddWebFeature(BuiltInWebFeatures.MinimalDownloadStrategy
                                                    .Clone<FeatureDefinition>()
                                                    .Disable());

                web.AddWebFeature(BuiltInWebFeatures.GettingStarted
                                                    .Clone<FeatureDefinition>()
                                                    .Disable());
            });

            DeployReverseAndTestModel(model, options);
        }

        #endregion
    }
}
