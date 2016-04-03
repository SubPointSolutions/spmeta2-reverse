using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Reverse.CSOM.Foundation.ReverseHandlers;
using SPMeta2.Reverse.CSOM.Foundation.Services;
using SPMeta2.Reverse.Services;
using SPMeta2.Reverse.Tests.Base;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Reverse.Tests.Impl.Definitions
{
    [TestClass]
    public class FieldDefinitionTests : ReverseTestBase
    {
        #region tests

        [TestMethod]
        [TestCategory("Fields")]
        public void Can_Reverse_Site_Fields()
        {
            WithCSOMContext(context =>
            {
                var service = new CSOMReverseService();

                service.Handlers.Clear();
                service.Handlers.Add(new SiteReverseHandler());

                service.Handlers.Add(new FieldReverseHandler());

                var result = service.ReverseSiteModel(context, ReverseOptions.Default);
            });
        }

        #endregion
    }
}
