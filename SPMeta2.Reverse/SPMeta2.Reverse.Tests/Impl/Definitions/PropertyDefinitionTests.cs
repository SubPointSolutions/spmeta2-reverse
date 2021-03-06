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
    public class PropertyDefinitionTests : ReverseTestBase
    {
        #region tests

        [TestMethod]
        [TestCategory("Properties")]
        public void Can_Reverse_Properties_At_Web()
        {
            // only root web
            var options = ReverseOptions.Default
                            .AddDepthOption<WebDefinition>(0);

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddProperty(Def<PropertyDefinition>());
                web.AddProperty(Def<PropertyDefinition>());
            });

            DeployReverseAndTestModel(model, options);
        }

        [TestMethod]
        [TestCategory("Properties")]
        public void Can_Reverse_Properties_At_ListLevel()
        {
            // only root web
            var options = ReverseOptions.Default
                            .AddDepthOption<WebDefinition>(0);

            var listTitle = Rnd.String();

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddRandomList(list =>
                {
                    (list.Value as ListDefinition).TemplateType = BuiltInListTemplateTypeId.DocumentLibrary;
                    (list.Value as ListDefinition).Url = null;
                    (list.Value as ListDefinition).CustomUrl = Rnd.String();

                    (list.Value as ListDefinition).Title = listTitle;


                    list.AddProperty(Def<PropertyDefinition>());
                    list.AddProperty(Def<PropertyDefinition>());
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


        [TestMethod]
        [TestCategory("Properties")]
        public void Can_Reverse_Properties_At_SiteLevel_TODO_M2_828()
        {
            // TODO
            // will fail, M2 issue with site level CSOM props
            // https://github.com/SubPointSolutions/spmeta2/issues/828

            return;

            // only root web
            var options = ReverseOptions.Default
                            .AddDepthOption<WebDefinition>(0);

            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddProperty(Def<PropertyDefinition>());
                site.AddProperty(Def<PropertyDefinition>());
            });

            DeployReverseAndTestModel(model, options,
                new[]
                {
                    typeof(SiteReverseHandler),
                    typeof(WebReverseHandler),
                    typeof(PropertyReverseHandler)
                });
        }

        #endregion
    }
}
