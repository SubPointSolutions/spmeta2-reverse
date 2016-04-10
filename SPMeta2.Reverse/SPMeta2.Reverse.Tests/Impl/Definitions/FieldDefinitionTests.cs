using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers;
using SPMeta2.Definitions;
using SPMeta2.Reverse.CSOM;
using SPMeta2.Reverse.CSOM;
using SPMeta2.Reverse.Services;
using SPMeta2.Reverse.Tests.Base;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Reverse.Tests.Impl.Definitions
{
    [TestClass]
    public class FieldDefinitionTests : ReverseTestBase
    {
        #region tests

        [TestMethod]
        [TestCategory("Fields")]
        public void Can_Reverse_Site_Fields()
        {
            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddRandomField();
                site.AddRandomField();
            });

            DeployReverseAndTestModel(model);
        }

        [TestMethod]
        [TestCategory("Fields")]
        public void Can_Reverse_Web_Fields()
        {
            var options = ReverseOptions.Default
                            .AddDepthOption<WebDefinition>(0);

            var model = SPMeta2Model.NewWebModel(site =>
            {
                site.AddRandomField();
                site.AddRandomField();
            });

            DeployReverseAndTestModel(model, options);
        }

        #endregion
    }
}
