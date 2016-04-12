using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Reverse.Services;
using SPMeta2.Reverse.Tests.Base;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Reverse.Tests.Impl.Definitions.Fields
{
    [TestClass]
    public class URLFieldDefinitionTests : ReverseTestBase
    {
        #region tests

        [TestMethod]
        [TestCategory("Fields.URL")]
        public void Can_Reverse_Site_URLFields()
        {
            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddURLField(Def<URLFieldDefinition>());
                site.AddURLField(Def<URLFieldDefinition>());
            });

            DeployReverseAndTestModel(model);
        }

        [TestMethod]
        [TestCategory("Fields.URL")]
        public void Can_Reverse_Web_URLFields()
        {
            var options = ReverseOptions.Default
                            .AddDepthOption<WebDefinition>(0);

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddURLField(Def<URLFieldDefinition>());
                web.AddURLField(Def<URLFieldDefinition>());
            });

            DeployReverseAndTestModel(model, options);
        }

        #endregion
    }
}
