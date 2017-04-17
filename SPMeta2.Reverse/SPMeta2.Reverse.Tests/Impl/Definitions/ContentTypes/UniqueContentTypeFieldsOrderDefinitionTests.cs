using SPMeta2.Reverse.Tests.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Syntax.Default;
using SPMeta2.Definitions.ContentTypes;
using SPMeta2.Reverse.Services;

namespace SPMeta2.Reverse.Tests.Impl.Definitions.ContentTypes
{
    [TestClass]
    public class UniqueContentTypeFieldsOrderDefinitionTests : ReverseTestBase
    {
        #region tests

        [TestMethod]
        [TestCategory("ContentTypes.UniqueContentTypeFieldsOrder")]
        public void Can_Reverse_UniqueContentTypeFieldsOrder()
        {
            var options = ReverseOptions.Default
                            .AddDepthOption<WebDefinition>(0);

            var f1 = Def<BooleanFieldDefinition>();
            var f2 = Def<BooleanFieldDefinition>();
            var f3 = Def<BooleanFieldDefinition>();

            var c1 = Def<ContentTypeDefinition>();

            options.AddFilterOption<ContentTypeDefinition>(l => l.Name == c1.Name);
            options.AddFilterOption<FieldDefinition>(l => l.InternalName == f1.InternalName);

            //ContentTypeFieldLinkDefinition

            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddField(f1);
                site.AddField(f2);
                site.AddField(f3);

                site.AddContentType(c1, contentType =>
                {
                    contentType.AddContentTypeFieldLink(f1);
                    contentType.AddContentTypeFieldLink(f2);
                    contentType.AddContentTypeFieldLink(f3);

                    contentType.AddUniqueContentTypeFieldsOrder(new UniqueContentTypeFieldsOrderDefinition
                    {
                        Fields = new List<FieldLinkValue>()
                        {
                            new FieldLinkValue { Id = f2.Id },
                            new FieldLinkValue { Id = f3.Id },
                            new FieldLinkValue { Id = f1.Id }
                        }
                    });
                });
            });

            DeployReverseAndTestModel(model, options);
        }

        #endregion
    }
}