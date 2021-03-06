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
    public class ModuleFileDefinitionTests : ReverseTestBase
    {
        #region tests

        [TestMethod]
        [TestCategory("ModuleFiles")]
        public void Can_Reverse_ModuleFiles()
        {
            // only root web
            var options = ReverseOptions.Default
                            .AddDepthOption<WebDefinition>(0);

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddRandomList(list =>
                {
                    (list.Value as ListDefinition).TemplateType = BuiltInListTemplateTypeId.DocumentLibrary;
                    (list.Value as ListDefinition).Url = null;
                    (list.Value as ListDefinition).CustomUrl = Rnd.String();

                    list.AddRandomModuleFile();

                    list.AddRandomFolder(folder =>
                    {
                        folder.AddRandomModuleFile();
                    });
                });
            });

            // only giving list, improve the test performance
            var listDefs = GetAllDefinitionOfType<ListDefinition>(model);

            foreach (var listDef in listDefs)
            {
                options.AddFilterOption<ListDefinition>(l => l.Title == listDef.Title);
            }

            DeployReverseAndTestModel(model, options);
        }

        #endregion
    }
}
