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
    public class QuickLaunchNavigationNodeDefinitionTests : ReverseTestBase
    {
        #region tests

        [TestMethod]
        [TestCategory("QuickLaunchNavigation")]
        public void Can_Reverse_QuickLaunchNavigationNodes()
        {
            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddQuickLaunchNavigationNode(ModelGeneratorService.GetRandomDefinition<QuickLaunchNavigationNodeDefinition>());
                web.AddQuickLaunchNavigationNode(ModelGeneratorService.GetRandomDefinition<QuickLaunchNavigationNodeDefinition>());
            });

            DeployReverseAndTestModel(model, new[]
            {
                typeof(SiteReverseHandler),
                typeof(WebReverseHandler),
                typeof(QuickLaunchNavigationNodeReverseHandler)
            });
        }

        // TODO, add tests to revere lists and libraries

        #endregion
    }
}
