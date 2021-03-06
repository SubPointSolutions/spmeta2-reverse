using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers;
using SPMeta2.Definitions;
using SPMeta2.Reverse.CSOM.ReverseHandlers;
using SPMeta2.Reverse.CSOM.Services;
using SPMeta2.Reverse.Services;
using SPMeta2.Reverse.Tests.Base;
using SPMeta2.Syntax.Default;
using SPMeta2.Enumerations;

namespace SPMeta2.Reverse.Tests.Impl.Definitions
{
    [TestClass]
    public class SandboxSolutionDefinitionTests : ReverseTestBase
    {
        #region tests

        [TestMethod]
        [TestCategory("SandboxSolutions")]
        public void Can_Reverse_SandboxSolutions()
        {
            // only root web
            var options = ReverseOptions.Default
                            .AddDepthOption<WebDefinition>(0);

            // TODO
            // var title = Rnd.String();
            // options.AddFilterOption<SandboxSolutionDefinition>(d => d.Title == title);

            var wspPath = @"Content/Apps/SPMeta2.Containers.SandboxSolutionContainer.wsp";

            var sandboxSolutionDef = new SandboxSolutionDefinition
            {
                FileName = string.Format("{0}.wsp", Rnd.String()),
                Activate = true,

                SolutionId = new Guid("e9a61998-07f2-45e9-ae43-9e93fa6b11bb"),

                Content = System.IO.File.ReadAllBytes(wspPath)
            };

            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddSandboxSolution(sandboxSolutionDef);
            });

            DeployReverseAndTestModel(model, options);
        }

        #endregion
    }
}
