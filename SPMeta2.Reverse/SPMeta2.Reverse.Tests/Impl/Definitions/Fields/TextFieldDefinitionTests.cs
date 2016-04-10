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
    public class TextFieldDefinitionTests : ReverseTestBase
    {
        #region tests

        [TestMethod]
        [TestCategory("Fields.Text")]
        public void Can_Reverse_Site_TextFields()
        {
            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddTextField(Def<TextFieldDefinition>());
                site.AddTextField(Def<TextFieldDefinition>());
            });

            DeployReverseAndTestModel(model);
        }

        [TestMethod]
        [TestCategory("Fields.Text")]
        public void Can_Reverse_Web_BooleanFields()
        {
            var options = ReverseOptions.Default
                            .AddDepthOption<WebDefinition>(0);

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddTextField(Def<TextFieldDefinition>());
                web.AddTextField(Def<TextFieldDefinition>());
            });

            DeployReverseAndTestModel(model,options);
        }

        #endregion
    }
}
