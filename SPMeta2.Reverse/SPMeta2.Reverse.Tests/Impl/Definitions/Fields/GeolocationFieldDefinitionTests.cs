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
    public class GeolocationFieldDefinitionTests : ReverseTestBase
    {
        #region tests

        [TestMethod]
        [TestCategory("Fields.Geolocation")]
        public void Can_Reverse_Site_GeolocationFields()
        {
            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddGeolocationField(Def<GeolocationFieldDefinition>());
                site.AddGeolocationField(Def<GeolocationFieldDefinition>());
            });

            DeployReverseAndTestModel(model);
        }

        [TestMethod]
        [TestCategory("Fields.Geolocation")]
        public void Can_Reverse_Web_GeolocationFields()
        {
            var options = ReverseOptions.Default
                            .AddDepthOption<WebDefinition>(0);

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddGeolocationField(Def<GeolocationFieldDefinition>());
                web.AddGeolocationField(Def<GeolocationFieldDefinition>());
            });

            DeployReverseAndTestModel(model, options);
        }

        #endregion
    }
}
