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
    public class DateTimeFieldDefinitionTests : ReverseTestBase
    {
        #region tests

        [TestMethod]
        [TestCategory("Fields.DateTime")]
        public void Can_Reverse_Site_DateTimeFields()
        {
            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddDateTimeField(Def<DateTimeFieldDefinition>());
                site.AddDateTimeField(Def<DateTimeFieldDefinition>());
            });

            DeployReverseAndTestModel(model);
        }

        [TestMethod]
        [TestCategory("Fields.DateTime")]
        public void Can_Reverse_Web_DateTimeFields()
        {
            var options = ReverseOptions.Default
                            .AddDepthOption<WebDefinition>(0);

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddDateTimeField(Def<DateTimeFieldDefinition>());
                web.AddDateTimeField(Def<DateTimeFieldDefinition>());
            });

            DeployReverseAndTestModel(model,options);
        }

        #endregion
    }
}
