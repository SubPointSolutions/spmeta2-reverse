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
    public class BooleanFieldDefinitionTests : ReverseTestBase
    {
        #region tests

        [TestMethod]
        [TestCategory("Fields.Boolean")]
        public void Can_Reverse_Site_BooleanFields()
        {
            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddBooleanField(Def<BooleanFieldDefinition>());
                site.AddBooleanField(Def<BooleanFieldDefinition>());
            });

            DeployReverseAndTestModel(model);
        }

        [TestMethod]
        [TestCategory("Fields.Boolean")]
        public void Can_Reverse_Web_BooleanFields()
        {
            var options = ReverseOptions.Default
                            .AddDepthOption<WebDefinition>(0);

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddBooleanField(Def<BooleanFieldDefinition>());
                web.AddBooleanField(Def<BooleanFieldDefinition>());
            });

            DeployReverseAndTestModel(model,options);
        }

        #endregion
    }
}
