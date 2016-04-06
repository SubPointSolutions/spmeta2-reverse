using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers;
using SPMeta2.Definitions;
using SPMeta2.Reverse.CSOM.Foundation.ReverseHandlers;
using SPMeta2.Reverse.CSOM.Foundation.Services;
using SPMeta2.Reverse.Services;
using SPMeta2.Reverse.Tests.Base;
using SPMeta2.Syntax.Default;
using SPMeta2.Enumerations;

namespace SPMeta2.Reverse.Tests.Impl.Definitions
{
    [TestClass]
    public class ListViewDefinitionTests : ReverseTestBase
    {
        #region init

        [TestInitialize]
        public void InternalInit()
        {
            WithCSOMContext(context => DeleteAllSubWebs(context.Site.RootWeb));
        }

        #endregion

        #region tests

        [TestMethod]
        [TestCategory("ListViews")]
        public void Can_Reverse_ListViews()
        {
            // TODO, quick fix only
            WebReverseHandler.UseRootWebOnly = true;

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddRandomList(list =>
                {
                    (list.Value as ListDefinition).TemplateType = BuiltInListTemplateTypeId.DocumentLibrary;
                    (list.Value as ListDefinition).Url = null;
                    (list.Value as ListDefinition).CustomUrl = Rnd.String();

                    // only one out of two list views!
                    var isDefault = Rnd.Bool();

                    list.AddRandomListView(view =>
                    {
                        (view.Value as ListViewDefinition).Url = Rnd.String() + ".aspx";
                        (view.Value as ListViewDefinition).IsDefault = isDefault;
                    });

                    list.AddRandomListView(view =>
                    {
                        (view.Value as ListViewDefinition).Url = Rnd.String() + ".aspx";
                        (view.Value as ListViewDefinition).IsDefault = isDefault;
                    });
                });
            });

            DeployReverseAndTestModel(model, new[]
            {
                typeof(SiteReverseHandler),
                 typeof(WebReverseHandler),
                  typeof(ListReverseHandler),
                 typeof(ListViewReverseHandler)
            });

            // TODO, quick fix only
            WebReverseHandler.UseRootWebOnly = false;
        }

        // TODO, add tests to revere list view with CAML queries and so on

        #endregion
    }
}
