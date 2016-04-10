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
    public class NumberFieldDefinitionTests : ReverseTestBase
    {
        #region tests

        [TestMethod]
        [TestCategory("Fields.Number")]
        public void Can_Reverse_Site_NumberFields()
        {
            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddNumberField(Def<NumberFieldDefinition>());
                site.AddNumberField(Def<NumberFieldDefinition>());
            });

            DeployReverseAndTestModel(model);
        }

        [TestMethod]
        [TestCategory("Fields.Number")]
        public void Can_Reverse_Web_NumberFields()
        {
            var options = ReverseOptions.Default
                            .AddDepthOption<WebDefinition>(0);

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddNumberField(Def<NumberFieldDefinition>());
                web.AddNumberField(Def<NumberFieldDefinition>());
            });

            DeployReverseAndTestModel(model,options);
        }

        #endregion
    }
}
