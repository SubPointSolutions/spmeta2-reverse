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
    public class CurrencyFieldDefinitionTests : ReverseTestBase
    {
        #region tests

        [TestMethod]
        [TestCategory("Fields.Currency")]
        public void Can_Reverse_Site_CurrencyFields()
        {
            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddCurrencyField(Def<CurrencyFieldDefinition>());
                site.AddCurrencyField(Def<CurrencyFieldDefinition>());
            });

            DeployReverseAndTestModel(model);
        }

        [TestMethod]
        [TestCategory("Fields.Currency")]
        public void Can_Reverse_Web_CurrencyFields()
        {
            var options = ReverseOptions.Default
                            .AddDepthOption<WebDefinition>(0);

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddCurrencyField(Def<CurrencyFieldDefinition>());
                web.AddCurrencyField(Def<CurrencyFieldDefinition>());
            });

            DeployReverseAndTestModel(model,options);
        }

        #endregion
    }
}
