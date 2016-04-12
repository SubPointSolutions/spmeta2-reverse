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
    public class MultiChoiceFieldDefinitionTests : ReverseTestBase
    {
        #region tests

        [TestMethod]
        [TestCategory("Fields.MultiChoice")]
        public void Can_Reverse_Site_MultiChoiceFields()
        {
            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddMultiChoiceField(Def<MultiChoiceFieldDefinition>());
                site.AddMultiChoiceField(Def<MultiChoiceFieldDefinition>());
            });

            DeployReverseAndTestModel(model);
        }
        
        [TestMethod]
        [TestCategory("Fields.MultiChoice")]
        public void Can_Reverse_Web_MultiChoiceFields()
        {
            var options = ReverseOptions.Default
                            .AddDepthOption<WebDefinition>(0);

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddMultiChoiceField(Def<MultiChoiceFieldDefinition>());
                web.AddMultiChoiceField(Def<MultiChoiceFieldDefinition>());
            });

            DeployReverseAndTestModel(model,options);
        }

        #endregion
    }
}
