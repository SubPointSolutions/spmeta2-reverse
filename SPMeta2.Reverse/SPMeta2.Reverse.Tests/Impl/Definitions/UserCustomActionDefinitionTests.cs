using System;
using System.Runtime.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers;
using SPMeta2.Definitions;
using SPMeta2.Reverse.CSOM.Foundation.ReverseHandlers;
using SPMeta2.Reverse.CSOM.Foundation.Services;
using SPMeta2.Reverse.Services;
using SPMeta2.Reverse.Tests.Base;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Reverse.Tests.Impl.Definitions
{
    [TestClass]
    public class UserCustomActionDefinitionTests : ReverseTestBase
    {
        #region tests

        [TestMethod]
        [TestCategory("UserCustomAction")]
        public void Can_Reverse_UserCustomAction()
        {
            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddRandomUserCustomAction();
                site.AddRandomUserCustomAction();
            });

            DeployReverseAndTestModel(model);
        }

        [TestMethod]
        [TestCategory("UserCustomAction")]
        public void Can_Reverse_UserCustomAction_AtSite()
        {
            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddRandomUserCustomAction();
                site.AddRandomUserCustomAction();
            });

            DeployReverseAndTestModel(model);
        }

        [TestMethod]
        [TestCategory("UserCustomAction")]
        public void Can_Reverse_UserCustomAction_AtWeb()
        {
            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddRandomUserCustomAction();
                web.AddRandomUserCustomAction();
            });

            DeployReverseAndTestModel(model);
        }

        #endregion
    }
}
