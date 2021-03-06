using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Reverse.Services;
using SPMeta2.Reverse.Tests.Base;
using SPMeta2.Standard.Definitions.Fields;
using SPMeta2.Standard.Syntax;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Reverse.Tests.Impl.Definitions.Fields
{
    [TestClass]
    public class HTMLFieldDefinitionTests : ReverseTestBase
    {
        #region tests

        [TestMethod]
        [TestCategory("Fields.HTML")]
        public void Can_Reverse_Site_HTMLFields()
        {
            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddHTMLField(Def<HTMLFieldDefinition>());
                site.AddHTMLField(Def<HTMLFieldDefinition>());
            });

            DeployReverseAndTestModel(model);
        }

        [TestMethod]
        [TestCategory("Fields.HTML")]
        public void Can_Reverse_Web_HTMLFields()
        {
            var options = ReverseOptions.Default
                            .AddDepthOption<WebDefinition>(0);

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddHTMLField(Def<HTMLFieldDefinition>());
                web.AddHTMLField(Def<HTMLFieldDefinition>());
            });

            DeployReverseAndTestModel(model, options);
        }

        #endregion
    }
}
