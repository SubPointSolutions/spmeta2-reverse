using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using SPMeta2.Containers.Consts;
using SPMeta2.Containers.Services;
using SPMeta2.Containers.Utils;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Reverse.CSOM.Foundation.ReverseHosts;
using SPMeta2.Reverse.CSOM.Foundation.Services;
using SPMeta2.Reverse.Regression.Services;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Standard.Syntax;
using SPMeta2.Utils;
using SPMeta2.Reverse.Tests.Services;

namespace SPMeta2.Reverse.Tests.Base
{
    public class ReverseTestBase
    {
        #region constructors

        public ReverseTestBase()
        {
            AssertService = new VSAssertService();

            ModelGeneratorService = new ModelGeneratorService();

            ModelGeneratorService.RegisterDefinitionGenerators(typeof(FieldDefinition).Assembly);
            ModelGeneratorService.RegisterDefinitionGenerators(typeof(TaxonomyTermDefinition).Assembly);
        }

        #endregion

        #region properties

        public ModelGeneratorService ModelGeneratorService { get; set; }

        public string SiteUrl { get; set; }

        public string UserName { get; set; }
        public string UserPassword { get; set; }


        public AssertServiceBase AssertService { get; set; }

        #endregion

        #region methods

        public void DeployReverseAndTestModel(ModelNode model)
        {
            DeployReverseAndTestModel(new ModelNode[] { model });
        }

        public void DeployReverseAndTestModel(IEnumerable<ModelNode> models)
        {
            foreach (var model in models)
            {
                // deploy model, TODO


                // reverse model, TODO

                // validate model
                var reverseRegressionService = new ReverseValidationService();

                //reverseRegressionService.DeployModel();
                
                // assert model
                var hasMissedOrInvalidProps = ReverseRegressionAssertService.ResolveModelValidation(model);
                AssertService.IsFalse(hasMissedOrInvalidProps);
            }



        }

        #endregion

        #region utils

        private static SecureString GetSecurePasswordString(string password)
        {
            var securePassword = new SecureString();

            foreach (var s in password)
                securePassword.AppendChar(s);

            return securePassword;
        }


        public void WithCSOMContext(Action<ClientContext> action)
        {
            WithCSOMContext(SiteUrl, action);
        }

        public void WithCSOMContext(string siteUrl, Action<ClientContext> action)
        {
            WithCSOMContext(siteUrl, UserName, UserPassword, action);
        }

        /// <summary>
        /// Invokes given action under CSOM client context.
        /// </summary>
        /// <param name="siteUrl"></param>
        /// <param name="userName"></param>
        /// <param name="userPassword"></param>
        /// <param name="action"></param>
        private void WithCSOMContext(string siteUrl, string userName, string userPassword, Action<ClientContext> action)
        {
            using (var context = new ClientContext(siteUrl))
            {
                context.Credentials = new SharePointOnlineCredentials(userName, GetSecurePasswordString(userPassword));
                action(context);
            }
        }


        #endregion
    }
}
