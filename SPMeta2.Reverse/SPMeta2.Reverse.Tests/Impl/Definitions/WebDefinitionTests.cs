using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers;
using SPMeta2.Definitions;
using SPMeta2.Reverse.CSOM.Foundation.ReverseHandlers;
using SPMeta2.Reverse.CSOM.Foundation.Services;
using SPMeta2.Reverse.Services;
using SPMeta2.Reverse.Tests.Base;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Reverse.Tests.Impl.Definitions
{
    [TestClass]
    public class WebDefinitionTests : ReverseTestBase
    {
        #region tests

        [TestMethod]
        [TestCategory("Webs")]
        public void Can_Reverse_Webs()
        {
            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddWeb(ModelGeneratorService.GetRandomDefinition<WebDefinition>());
                site.AddWeb(ModelGeneratorService.GetRandomDefinition<WebDefinition>());
            });

            DeployReverseAndTestModel(model);
        }

        [TestMethod]
        [TestCategory("Webs")]
        public void Can_Reverse_Webs_Hierarchy()
        {
            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddWeb(ModelGeneratorService.GetRandomDefinition<WebDefinition>(), w =>
                {
                    w.AddWeb(ModelGeneratorService.GetRandomDefinition<WebDefinition>());
                });

                site.AddWeb(ModelGeneratorService.GetRandomDefinition<WebDefinition>(), w =>
                {
                    w.AddWeb(ModelGeneratorService.GetRandomDefinition<WebDefinition>());
                });
            });

            DeployReverseAndTestModel(model);
        }

        #endregion
    }
}
