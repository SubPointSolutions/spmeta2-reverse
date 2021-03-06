using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.BuiltInDefinitions;
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
    public class MasterPageDefinitionTests : ReverseTestBase
    {
        #region tests

        [TestMethod]
        [TestCategory("MasterPages")]
        public void Can_Reverse_MasterPages()
        {
            // only root web
            var options = ReverseOptions.Default
                            .AddDepthOption<WebDefinition>(0);

            // TODO
            // var title = Rnd.String();
            // options.AddFilterOption<MasterPageDefinition>(d => d.Title == title);

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddHostList(BuiltInListDefinitions.Catalogs.MasterPage, list =>
                {
                    list.AddMasterPage(Def<MasterPageDefinition>());
                });
            });

            // only giving list, improve the test performance
            var listDefs = GetAllDefinitionOfType<ListDefinition>(model);

            foreach (var def in listDefs)
                options.AddFilterOption<ListDefinition>(l => l.Title == def.Title);

            // only giving iles
            var masterPageDefs = GetAllDefinitionOfType<MasterPageDefinition>(model);

            foreach (var def in masterPageDefs)
                options.AddFilterOption<MasterPageDefinition>(l => l.FileName == def.FileName);

            DeployReverseAndTestModel(model, options);
        }

        #endregion
    }
}
