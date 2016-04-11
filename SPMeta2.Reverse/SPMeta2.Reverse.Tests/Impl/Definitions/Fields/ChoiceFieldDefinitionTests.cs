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
    public class ChoiceFieldDefinitionTests : ReverseTestBase
    {
        #region tests

        [TestMethod]
        [TestCategory("Fields.Choice")]
        public void Can_Reverse_Site_ChoiceFields()
        {
            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddChoiceField(Def<ChoiceFieldDefinition>());
                site.AddChoiceField(Def<ChoiceFieldDefinition>());
            });

            DeployReverseAndTestModel(model);
        }
        
        [TestMethod]
        [TestCategory("Fields.Choice")]
        public void Can_Reverse_Web_ChoiceFields()
        {
            var options = ReverseOptions.Default
                            .AddDepthOption<WebDefinition>(0);

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddChoiceField(Def<ChoiceFieldDefinition>());
                web.AddChoiceField(Def<ChoiceFieldDefinition>());
            });

            DeployReverseAndTestModel(model,options);
        }

        #endregion
    }
}
