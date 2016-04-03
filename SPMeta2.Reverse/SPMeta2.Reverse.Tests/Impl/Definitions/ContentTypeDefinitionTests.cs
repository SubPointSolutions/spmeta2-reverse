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
    public class ContentTypeDefinitionTests : ReverseTestBase
    {
        #region tests

        [TestMethod]
        [TestCategory("ContentTypes")]
        public void Can_Reverse_Site_ContentTypes()
        {
            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddRandomContentType();
                site.AddRandomContentType();
            });

            DeployReverseAndTestModel(model);
        }

        [TestMethod]
        [TestCategory("ContentTypes")]
        public void Can_Reverse_Web_ContentTypes()
        {
            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddRandomContentType();
                web.AddRandomContentType();
            });

            DeployReverseAndTestModel(model);
        }

        #endregion
    }
}
