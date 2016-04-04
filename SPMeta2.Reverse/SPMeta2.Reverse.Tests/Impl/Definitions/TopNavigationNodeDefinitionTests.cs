﻿using System;
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
    public class TopNavigationNodeDefinitionTests : ReverseTestBase
    {
        #region tests

        [TestMethod]
        [TestCategory("TopNavigationNode")]
        public void Can_Reverse_TopNavigationNodes()
        {
            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddTopNavigationNode(ModelGeneratorService.GetRandomDefinition<TopNavigationNodeDefinition>());
                web.AddTopNavigationNode(ModelGeneratorService.GetRandomDefinition<TopNavigationNodeDefinition>());
            });

            DeployReverseAndTestModel(model, new[]
            {
                typeof(SiteReverseHandler),
                typeof(WebReverseHandler),
                typeof(TopNavigationNodeDefinitionReverseHandler),
            });
        }

        // TODO, add tests to revere lists and libraries

        #endregion
    }
}
