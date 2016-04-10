using System;
using System.Diagnostics;
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
                site.AddWeb(Def<WebDefinition>());
                site.AddWeb(Def<WebDefinition>());
            });

            DeployReverseAndTestModel(model);
        }

        [TestMethod]
        [TestCategory("Webs")]
        public void Can_Reverse_Webs_Hierarchy_Deep1()
        {
            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddWeb(Def<WebDefinition>(), w =>
                {
                    w.AddWeb(Def<WebDefinition>());
                });
            });

            DeployReverseAndTestModel(model);
        }

        [TestMethod]
        [TestCategory("Webs")]
        public void Can_Reverse_Webs_Hierarchy_Deep2()
        {
            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddWeb(Def<WebDefinition>(), w1 =>
                {
                    w1.AddWeb(Def<WebDefinition>(), w2 =>
                    {
                        w2.AddWeb(Def<WebDefinition>());
                    });
                });
            });

            DeployReverseAndTestModel(model);
        }

        [TestMethod]
        [TestCategory("Webs")]
        public void Can_Reverse_Webs_Hierarchy_Deep3_As_2()
        {
            // having 3 level web, but reverse only two levels
            var options = ReverseOptions.Default
                            .AddDepthOption<WebDefinition>(2);

            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddWeb(Def<WebDefinition>(), w1 =>
                {
                    w1.AddWeb(Def<WebDefinition>(), w2 =>
                    {
                        w2.AddWeb(Def<WebDefinition>(), w3 =>
                        {
                            w3.AddWeb(Def<WebDefinition>());
                        });
                    });
                });
            });

            DeployReverseAndTestModel(model, options);
        }

        #endregion
    }
}
