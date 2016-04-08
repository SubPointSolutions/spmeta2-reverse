using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Models;
using SPMeta2.Reverse.Services;
using SPMeta2.Reverse.Tests.Base;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Reverse.Tests.Impl.API
{
    [TestClass]
    public class ReverseOptionsFilterTests : ReverseTestBase
    {
        [TestMethod]
        [TestCategory("API.ReverseOptions.Filters")]
        public void Can_Reverse_Filter_Equal()
        {
            // model
            var model = GetTestListModel();

            // only root web
            var options = ReverseOptions.Default
                                        .AddDepthOption<WebDefinition>(0);

            // only lists by 'Contains' filter 
            var listDefs = GetAllDefinitionOfType<ListDefinition>(model);

            foreach (var listDef in listDefs)
            {
                // ReverseFilterOperationType.Equal
                options.AddFilterOption<ListDefinition>(l => l.Title == listDef.Title);
            }

            DeployReverseAndTestModel(model, options);
        }

        [TestMethod]
        [TestCategory("API.ReverseOptions.Filters")]
        public void Can_Reverse_Filter_NotEqual()
        {
            // model
            var model = GetTestFieldModel();

            // only root web
            var options = ReverseOptions.Default
                                        .AddDepthOption<WebDefinition>(0);

            // only lists by 'Contains' filter 
            var listDefs = GetAllDefinitionOfType<FieldDefinition>(model);

            foreach (var listDef in listDefs)
            {
                var tmpValue = Rnd.String();

                // ReverseFilterOperationType.NotEqual
                options.AddFilterOption<ListDefinition>(l => l.Title != tmpValue);
            }

            DeployReverseAndTestModel(model, options);
        }

        [TestMethod]
        [TestCategory("API.ReverseOptions.Filters")]
        public void Can_Reverse_Filter_Contains()
        {
            // model
            var model = GetTestListModel();

            // only root web
            var options = ReverseOptions.Default
                                        .AddDepthOption<WebDefinition>(0);

            // only lists by 'Contains' filter 
            var listDefs = GetAllDefinitionOfType<ListDefinition>(model);

            foreach (var listDef in listDefs)
            {
                var firstTitlePart = listDef.Title.Substring(0, listDef.Title.Length / 2);

                // ReverseFilterOperationType.Contains
                options.AddFilterOption<ListDefinition>(l => l.Title.Contains(firstTitlePart));
            }

            DeployReverseAndTestModel(model, options);
        }

        [TestMethod]
        [TestCategory("API.ReverseOptions.Filters")]
        public void Can_Reverse_Filter_StartsWith()
        {
            // model
            var model = GetTestListModel();

            // only root web
            var options = ReverseOptions.Default
                                        .AddDepthOption<WebDefinition>(0);

            // only lists by 'Contains' filter 
            var listDefs = GetAllDefinitionOfType<ListDefinition>(model);

            foreach (var listDef in listDefs)
            {
                var firstTitlePart = listDef.Title.Substring(0, listDef.Title.Length / 2);

                // ReverseFilterOperationType.StartsWith
                options.AddFilterOption<ListDefinition>(l => l.Title.StartsWith(firstTitlePart));
            }

            DeployReverseAndTestModel(model, options);
        }

        [TestMethod]
        [TestCategory("API.ReverseOptions.Filters")]
        public void Can_Reverse_Filter_EndsWith()
        {
            // model
            var model = GetTestListModel();

            // only root web
            var options = ReverseOptions.Default
                                        .AddDepthOption<WebDefinition>(0);

            // only lists by 'Contains' filter 
            var listDefs = GetAllDefinitionOfType<ListDefinition>(model);

            foreach (var listDef in listDefs)
            {
                var middle = listDef.Title.Length / 2;
                var lastTitlePart = listDef.Title.Substring(middle, listDef.Title.Length - middle);

                // ReverseFilterOperationType.EndsWith
                options.AddFilterOption<ListDefinition>(l => l.Title.EndsWith(lastTitlePart));
            }

            DeployReverseAndTestModel(model, options);
        }

        #region utils

        public ModelNode GetTestFieldModel()
        {
            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddRandomField();
                site.AddRandomField();
            });

            return model;
        }

        private ModelNode GetTestListModel()
        {
            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddRandomList(list =>
                {
                    (list.Value as ListDefinition).TemplateType = BuiltInListTemplateTypeId.DocumentLibrary;
                    (list.Value as ListDefinition).Url = null;
                    (list.Value as ListDefinition).CustomUrl = Rnd.String();

                });
            });

            return model;
        }

        #endregion
    }
}
