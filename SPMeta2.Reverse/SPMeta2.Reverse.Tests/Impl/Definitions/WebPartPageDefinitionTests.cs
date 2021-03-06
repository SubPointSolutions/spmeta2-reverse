using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Containers;
using SPMeta2.Containers.Extensions;
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
    public class WebPartPageDefinitionTests : ReverseTestBase
    {
        #region tests

        [TestMethod]
        [TestCategory("WebPartPages")]
        public void Can_Reverse_WebPartPages()
        {
            // only root web
            var options = ReverseOptions.Default
                            .AddDepthOption<WebDefinition>(0);

            // TODO
            // var title = Rnd.String();
            // options.AddFilterOption<WikiPageDefinition>(d => d.Title == title);

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddWebFeature(BuiltInWebFeatures.WikiPageHomePage
                                                    .Inherit()
                                                    .Enable(), f =>
                                                    {
                                                        f.RegExcludeFromValidation();
                                                    });

                web.AddHostList(BuiltInListDefinitions.SitePages, list =>
                {
                    list.AddRandomFolder(folder =>
                    {
                        folder.AddRandomWebPartPage();
                    });

                    list.AddRandomWebPartPage();
                    list.AddRandomWebPartPage();
                });
            });

            var listTitle = BuiltInListDefinitions.SitePages.Title;
            options.AddFilterOption<ListDefinition>(l => l.Title == listTitle);

            var defs = GetAllDefinitionOfType<WikiPageDefinition>(model);

            foreach (var def in defs)
            {
                options.AddFilterOption<WebPartPageDefinition>(l => l.FileName == def.FileName);
            }

            DeployReverseAndTestModel(model, options, new[]
            {
                typeof(WebReverseHandler),
                typeof(ListReverseHandler),
                typeof(FolderReverseHandler),
                typeof(WebPartPageReverseHandler),
            });
        }

        #endregion
    }
}
