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
    public class FolderDefinition : ReverseTestBase
    {
        #region init

        [TestInitialize]
        public void InternalInit()
        {
            //WithCSOMContext(context => DeleteAllSubWebs(context.Site.RootWeb));
        }

        #endregion

        #region tests

        [TestMethod]
        [TestCategory("Folders")]
        public void Can_Reverse_Folders()
        {
            var listTitle = Rnd.String();

            // only root web
            var options = ReverseOptions.Default
                            .AddDepthOption<WebDefinition>(0);

            //only current list
            options.AddFilterOption<ListDefinition>(l => l.Title == listTitle);

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddRandomList(list =>
                {
                    (list.Value as ListDefinition).TemplateType = BuiltInListTemplateTypeId.DocumentLibrary;
                    (list.Value as ListDefinition).Url = null;
                    (list.Value as ListDefinition).CustomUrl = Rnd.String();

                    (list.Value as ListDefinition).Title = listTitle;


                    list.AddRandomFolder(folder =>
                    {
                        folder.AddRandomFolder();
                    });

                    list.AddRandomFolder(folder =>
                    {
                        folder.AddRandomFolder();
                    });
                });
            });



            DeployReverseAndTestModel(model, options);
        }

        #endregion
    }
}
