using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers;
using SPMeta2.Definitions;
using SPMeta2.Reverse.CSOM.Foundation.ReverseHandlers;
using SPMeta2.Reverse.CSOM.Foundation.Services;
using SPMeta2.Reverse.Services;
using SPMeta2.Reverse.Tests.Base;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Reverse.Tests.Impl.Definitions
{
    [TestClass]
    public class ContentTypeFieldLinkDefinitionTests : ReverseTestBase
    {
        #region tests

        [TestMethod]
        [TestCategory("ContentTypeFieldLinks")]
        public void Can_Reverse_Site_ContentType_FieldLinks()
        {
            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddRandomContentType(ct =>
                {
                    ct.AddContentTypeFieldLink(ModelGeneratorService.GetRandomDefinition<ContentTypeFieldLinkDefinition>());
                });

                site.AddRandomContentType();
            });

            DeployReverseAndTestModel(model, new[]
            {
                typeof(SiteReverseHandler),
                typeof(ContentTypeReverseHandler),
                typeof(ContentTypeFieldLinkReverseHandler),
            });
        }

        #endregion

        #region content type types


        #endregion
    }
}
