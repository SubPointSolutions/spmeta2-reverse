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
    public class WelcomePageDefinitionTests : ReverseTestBase
    {
        #region tests

        [TestMethod]
        [TestCategory("WelcomePages")]
        public void Can_Reverse_WelcomePages_At_Web()
        {
            // only root web
            var options = ReverseOptions.Default
                            .AddDepthOption<WebDefinition>(0);

            // TODO
            // var title = Rnd.String();
            // options.AddFilterOption<WelcomePageDefinition>(d => d.Title == title);

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddRandomWelcomePage();
            });

            DeployReverseAndTestModel(model, options);
        }

        [TestMethod]
        [TestCategory("WelcomePages")]
        public void Can_Reverse_WelcomePages_At_List()
        {
            // only root web
            var options = ReverseOptions.Default
                            .AddDepthOption<WebDefinition>(0);

            // TODO
            var title = Rnd.String();
            options.AddFilterOption<ListDefinition>(d => d.Title == title);

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddRandomList(list =>
                {
                    (list.Value as ListDefinition).TemplateType = BuiltInListTemplateTypeId.DocumentLibrary;
                    (list.Value as ListDefinition).Url = null;
                    (list.Value as ListDefinition).CustomUrl = Rnd.String();

                    (list.Value as ListDefinition).Title = title;

                    list.AddRandomWelcomePage();
                });
            });

            DeployReverseAndTestModel(model, options);
        }

        [TestMethod]
        [TestCategory("WelcomePages")]
        public void Can_Reverse_WelcomePages_At_Folder()
        {
            // only root web
            var options = ReverseOptions.Default
                            .AddDepthOption<WebDefinition>(0);

            // TODO
            var title = Rnd.String();
            options.AddFilterOption<ListDefinition>(d => d.Title == title);

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddRandomList(list =>
                {
                    (list.Value as ListDefinition).TemplateType = BuiltInListTemplateTypeId.DocumentLibrary;
                    (list.Value as ListDefinition).Url = null;
                    (list.Value as ListDefinition).CustomUrl = Rnd.String();

                    (list.Value as ListDefinition).Title = title;

                    list.AddRandomFolder(folder =>
                    {
                        folder.AddRandomWelcomePage();
                    });
                });
            });

            DeployReverseAndTestModel(model, options);
        }

        #endregion
    }
}
