using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers;
using SPMeta2.Definitions;
using SPMeta2.Reverse.CSOM.ReverseHandlers;
using SPMeta2.Reverse.CSOM.Services;
using SPMeta2.Reverse.Services;
using SPMeta2.Reverse.Tests.Base;
using SPMeta2.Syntax.Default;
using SPMeta2.Enumerations;

namespace SPMeta2.Reverse.Tests.Impl.Definitions
{
    [TestClass]
    public class ContentTypeFieldLinkDefinitionTests : ReverseTestBase
    {
        #region tests

        [TestMethod]
        [TestCategory("ContentTypeFieldLinks")]
        public void Can_Reverse_Site_ContentType_FieldLinks()
        {
            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddRandomContentType(ct =>
                {
                    // the definition id verification won't match on different ID/Name
                    // both should be there
                    var randomField = new ContentTypeFieldLinkDefinition
                    {
                        FieldInternalName = "CellPhone",
                        FieldId = BuiltInFieldId.CellPhone
                    };

                    ct.AddContentTypeFieldLink(randomField);
                });

                site.AddRandomContentType();
            });

            DeployReverseAndTestModel(model);
        }

        #endregion

        #region content type types


        #endregion
    }
}
