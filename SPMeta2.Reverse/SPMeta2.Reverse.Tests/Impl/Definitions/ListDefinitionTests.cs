using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers;
using SPMeta2.Definitions;
using SPMeta2.Reverse.CSOM.Foundation.ReverseHandlers;
using SPMeta2.Reverse.CSOM.Foundation.Services;
using SPMeta2.Reverse.Services;
using SPMeta2.Reverse.Tests.Base;
using SPMeta2.Syntax.Default;
using SPMeta2.Enumerations;
using Microsoft.SharePoint.Client;
using SPMeta2.Extensions;
using SPMeta2.Models;

namespace SPMeta2.Reverse.Tests.Impl.Definitions
{
    [TestClass]
    public class ListDefinitionTests : ReverseTestBase
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
        [TestCategory("Lists")]
        public void Can_Reverse_Lists()
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

                });

                web.AddRandomList(list =>
                {
                    (list.Value as ListDefinition).TemplateType = BuiltInListTemplateTypeId.GenericList;
                    (list.Value as ListDefinition).Url = null;
                    (list.Value as ListDefinition).CustomUrl = string.Format("Lists/{0}", Rnd.String());
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
