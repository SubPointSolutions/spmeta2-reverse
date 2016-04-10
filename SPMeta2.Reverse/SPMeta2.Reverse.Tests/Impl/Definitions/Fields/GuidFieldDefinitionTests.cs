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
    public class GuidFieldDefinitionTests : ReverseTestBase
    {
        #region tests

        [TestMethod]
        [TestCategory("Fields.Guid")]
        public void Can_Reverse_Site_GuidFields()
        {
            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddGuidField(Def<GuidFieldDefinition>());
                site.AddGuidField(Def<GuidFieldDefinition>());
            });

            DeployReverseAndTestModel(model);
        }

        [TestMethod]
        [TestCategory("Fields.Guid")]
        public void Can_Reverse_Web_GuidFields()
        {
            var options = ReverseOptions.Default
                            .AddDepthOption<WebDefinition>(0);

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddGuidField(Def<GuidFieldDefinition>());
                web.AddGuidField(Def<GuidFieldDefinition>());
            });

            DeployReverseAndTestModel(model, options);
        }

        #endregion
    }
}
