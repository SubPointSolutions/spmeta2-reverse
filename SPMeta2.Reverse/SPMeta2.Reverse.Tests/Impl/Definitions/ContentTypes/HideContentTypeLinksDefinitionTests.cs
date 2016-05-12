using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers;
using SPMeta2.Definitions;
using SPMeta2.Reverse.CSOM.ReverseHandlers;
using SPMeta2.Reverse.CSOM.Services;
using SPMeta2.Reverse.Services;
using SPMeta2.Reverse.Tests.Base;
using SPMeta2.Syntax.Default;
using SPMeta2.Enumerations;
using Microsoft.SharePoint.Client;
using SPMeta2.Containers.Extensions;
using SPMeta2.Definitions.ContentTypes;
using SPMeta2.Models;

namespace SPMeta2.Reverse.Tests.Impl.Definitions.ContentTypes
{
    [TestClass]
    public class HideContentTypeLinksDefinitionTests : ReverseTestBase
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
        [TestCategory("ContentType.HideContentTypeLinks")]
        public void Can_Reverse_HideContentTypeLinks()
        {
            // only root web
            var options = ReverseOptions.Default
                            .AddDepthOption<WebDefinition>(0);

            var ct1 = Def<ContentTypeDefinition>();
            var ct2 = Def<ContentTypeDefinition>();
            var ct3 = Def<ContentTypeDefinition>();
            var ct4 = Def<ContentTypeDefinition>();

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddContentType(ct1, c => c.RegExcludeFromValidation());
                site.AddContentType(ct2, c => c.RegExcludeFromValidation());
                site.AddContentType(ct3, c => c.RegExcludeFromValidation());
                site.AddContentType(ct4, c => c.RegExcludeFromValidation());
            });

            var listDef = Def<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.DocumentLibrary;
                def.Url = null;
                def.CustomUrl = Rnd.String();
                def.ContentTypesEnabled = true;
            });

            var model1 = SPMeta2Model.NewWebModel(web =>
            {
                web.AddList(listDef, list =>
                {
                    list.AddContentTypeLink(ct1);
                    list.AddContentTypeLink(ct2);
                    list.AddContentTypeLink(ct3);
                    list.AddContentTypeLink(ct4);
                });
            });


            var model2 = SPMeta2Model.NewWebModel(web =>
            {
                web.AddList(listDef, list =>
                {
                    list.AddHideContentTypeLinks(new HideContentTypeLinksDefinition
                    {
                        ContentTypes = new List<ContentTypeLinkValue>
                        {
                            new ContentTypeLinkValue { ContentTypeName =  ct2.Name },
                            new ContentTypeLinkValue { ContentTypeName =  ct3.Name },
                        }
                    });
                });
            });

            // only giving list, improve the test performance
            var listDefs = GetAllDefinitionOfType<ListDefinition>(model1);

            foreach (var listDefinition in listDefs)
            {
                options.AddFilterOption<ListDefinition>(l => l.Title == listDefinition.Title);
            }

            DeployReverseAndTestModel(new ModelNode[] { siteModel, model1, model2 }, options);
        }

        #endregion
    }
}
