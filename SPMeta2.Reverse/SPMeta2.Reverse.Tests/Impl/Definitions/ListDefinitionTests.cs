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
    public class ListDefinitionTests : ReverseTestBase
    {
        #region tests

        [TestMethod]
        [TestCategory("Lists")]
        public void Can_Reverse_Lists()
        {
            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddRandomList(list =>
                {
                    (list.Value as ListDefinition).TemplateType = BuiltInListTemplateTypeId.DocumentLibrary;
                    (list.Value as ListDefinition).Url = null;
                    (list.Value as ListDefinition).CustomUrl = Rnd.String();

                });

                web.AddRandomList(list =>
                {
                    (list.Value as ListDefinition).TemplateType = BuiltInListTemplateTypeId.GenericList;
                    (list.Value as ListDefinition).Url = null;
                    (list.Value as ListDefinition).CustomUrl = string.Format("lists/{0}", Rnd.String());
                });
            });

            DeployReverseAndTestModel(model, new[]
            {
                typeof(SiteReverseHandler),
                 typeof(WebReverseHandler),
                  typeof(ListReverseHandler),
            });
        }

        // TODO, add tests to revere lists and libraries

        #endregion
    }
}
