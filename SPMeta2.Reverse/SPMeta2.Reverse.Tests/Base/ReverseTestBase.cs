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
using SPMeta2.CSOM.Standard.Services;
using SPMeta2.Definitions;
using SPMeta2.Exceptions;
using SPMeta2.Models;
using SPMeta2.Reverse.CSOM.ReverseHosts;
using SPMeta2.Reverse.CSOM.Services;
using SPMeta2.Reverse.Regression.Services;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Standard.Syntax;
using SPMeta2.Utils;
using SPMeta2.Reverse.Tests.Services;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Reverse.CSOM.ReverseHandlers.Base;
using SPMeta2.Reverse.CSOM.Standard.Services;
using SPMeta2.Reverse.Services;
using SPMeta2.Reverse.Regression.Base;
using SPMeta2.Containers.Services.Rnd;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Reverse.Regression;
using SPMeta2.Containers.Assertion;
using SPMeta2.Reverse.Regression.Consts;
using System.IO;
using System.Xml;
using SPMeta2.Containers.DefinitionGenerators;
using SPMeta2.Containers.Standard.DefinitionGenerators.Taxonomy;
using SPMeta2.Syntax.Default;
using SPMeta2.Extensions;
using SPMeta2.Reverse.Exceptions;
using SPMeta2.Reverse.ReverseHandlers;

namespace SPMeta2.Reverse.Tests.Base
{


    [TestClass]
    public class ReverseTestBase
    {
        #region static

        protected static void DeleteAllSubWebs(Web web)
        {
            var context = web.Context;

            context.Load(web,
            website => website.Webs,
            website => website.Title);

            context.ExecuteQuery();
            for (int i = 0; i != web.Webs.Count; )
            {
                DeleteAllWebs(web.Webs[i]);
            }
        }
        private static void DeleteAllWebs(Web web)
        {
            var context = web.Context;

            context.Load(web,
                website => website.Webs,
                website => website.Title
            );

            context.ExecuteQuery();

            for (int i = 0; i != web.Webs.Count; )
            {
                DeleteAllWebs(web.Webs[i]);
            }

            web.DeleteObject();
            web.Update();

            context.ExecuteQuery();
        }

        static ReverseTestBase()
        {
            GlobalInternalInit();
        }

        private static void GlobalInternalInit()
        {
            RegressionAssertService.OnPropertyValidated += OnReversePropertyValidated;
        }

        private static void OnReversePropertyValidated(object sender, OnPropertyValidatedEventArgs e)
        {
            var reportService = new DefaultCoverageReportService();

            reportService.RegenerateReports(ReverseRegressionAssertService.ModelValidations);
        }

        #endregion

        #region constructors

        public ReverseTestBase()
        {
            SiteUrl = RunnerEnvironmentUtils.GetEnvironmentVariables(EnvironmentConsts.O365_SiteUrls).First();

            UserName = RunnerEnvironmentUtils.GetEnvironmentVariable(EnvironmentConsts.O365_UserName);
            UserPassword = RunnerEnvironmentUtils.GetEnvironmentVariable(EnvironmentConsts.O365_Password);

            AssertService = new VSAssertService();

            ModelGeneratorService = new ModelGeneratorService();

            ModelGeneratorService.RegisterDefinitionGenerators(typeof(FieldDefinitionGenerator).Assembly);
            ModelGeneratorService.RegisterDefinitionGenerators(typeof(TaxonomyTermDefinitionGenerator).Assembly);

            Rnd = new DefaultRandomService();
        }

        #endregion

        #region properties

        public ModelGeneratorService ModelGeneratorService { get; set; }

        public RandomService Rnd { get; set; }

        public string SiteUrl { get; set; }

        public string UserName { get; set; }
        public string UserPassword { get; set; }


        public AssertServiceBase AssertService { get; set; }

        #endregion

        #region methods

        public void DeployReverseAndTestModel(ModelNode model)
        {
            DeployReverseAndTestModel(model, ReverseOptions.Default);
        }

        public void DeployReverseAndTestModel(IEnumerable<ModelNode> models)
        {
            DeployReverseAndTestModel(models, ReverseOptions.Default);
        }

        public void DeployReverseAndTestModel(ModelNode model, ReverseOptions options)
        {
            InternalDeployReverseAndTestModel(new[] { model }, null, options);
        }

        public void DeployReverseAndTestModel(ModelNode model, ReverseOptions options,
            IEnumerable<Type> modelHandlers)
        {
            InternalDeployReverseAndTestModel(new[] { model }, modelHandlers, options);
        }


        public void DeployReverseAndTestModel(IEnumerable<ModelNode> models, ReverseOptions options)
        {
            InternalDeployReverseAndTestModel(models, null, options);
        }

        private void InternalDeployReverseAndTestModel(
            IEnumerable<ModelNode> models,
            IEnumerable<Type> reverseHandlers,
            ReverseOptions options)
        {
            // calculate handlers based on the model
            // that improved tests performance avoiding non-relevant reverse
            if (reverseHandlers == null)
            {
                var autoReverseHandlers = new List<Type>();

                var allDefinitions = models.SelectMany(
                    m => m.Flatten().Select(n => n.Value)
                    );

                var uniqueDefinitionTypes = new List<Type>();

                foreach (var def in allDefinitions)
                {
                    if (!uniqueDefinitionTypes.Contains(def.GetType()))
                        uniqueDefinitionTypes.Add(def.GetType());
                }

                var allHandlers = new List<ReverseHandlerBase>();

                var handlerTypes = ReflectionUtils.GetTypesFromAssemblies<CSOMReverseHandlerBase>(
new[]{              typeof(StandardCSOMReverseService).Assembly,
                  typeof(CSOMReverseService).Assembly})
                                                  .ToList();


                foreach (var type in handlerTypes)
                {
                    if (!allHandlers.Any(r => r.GetType() == type))
                    {
                        var instance = Activator.CreateInstance(type) as ReverseHandlerBase;

                        if (instance == null)
                            throw new SPMeta2ReverseException(
                                string.Format("Can't create reverse handle of type:[{0}]", type));

                        allHandlers.Add(instance);
                    }
                }

                foreach (var defType in uniqueDefinitionTypes)
                {
                    var neeedHandler = allHandlers.FirstOrDefault(h => h.ReverseType == defType);

                    if (neeedHandler == null)
                    {
                        throw new SPMeta2ReverseException(
                            string.Format("Can't find reverse handler for definition type:[{0}]", defType));
                    }

                    if (!autoReverseHandlers.Contains(neeedHandler.GetType()))
                        autoReverseHandlers.Add(neeedHandler.GetType());
                }

                reverseHandlers = autoReverseHandlers;
            }

            foreach (var deployedModel in models)
            {
                // deploy model
                DeployModel(deployedModel);

                // reverse model
                var reversedModel = ReverseModel(deployedModel, reverseHandlers, options);

                // validate model
                var reverseRegressionService = new ReverseValidationService();

                reverseRegressionService.DeployModel(new ReverseValidationModeHost
                {
                    OriginalModel = deployedModel,
                    ReversedModel = reversedModel,
                    ReverseOptions = options
                }, null);

                // assert model
                var hasMissedOrInvalidProps = ReverseRegressionAssertService.ResolveModelValidation(deployedModel);
                AssertService.IsFalse(hasMissedOrInvalidProps);
            }
        }

        private ModelNode ReverseModel(ModelNode deployedModel,
            IEnumerable<Type> reverseHandlers,
            ReverseOptions options)
        {
            ReverseResult reverseResut = null;

            WithCSOMContext(context =>
            {
                var reverseService = new StandardCSOMReverseService();

                if (reverseHandlers != null)
                {
                    reverseService.Handlers.Clear();

                    foreach (var reverseHandler in reverseHandlers)
                    {
                        var reverseHandlerInstance = Activator.CreateInstance(reverseHandler)
                            as CSOMReverseHandlerBase;

                        reverseService.Handlers.Add(reverseHandlerInstance);
                    }
                }

                if (deployedModel.Value.GetType() == typeof(FarmDefinition))
                {
                    throw new SPMeta2NotImplementedException(
                        string.Format("Runner does not support model of type: [{0}]", deployedModel.Value.GetType()));
                }
                else if (deployedModel.Value.GetType() == typeof(WebApplicationDefinition))
                {
                    throw new SPMeta2NotImplementedException(
                        string.Format("Runner does not support model of type: [{0}]", deployedModel.Value.GetType()));
                }
                else if (deployedModel.Value.GetType() == typeof(SiteDefinition))
                {
                    reverseResut = reverseService.ReverseSiteModel(context, options);
                }
                else if (deployedModel.Value.GetType() == typeof(WebDefinition))
                {
                    reverseResut = reverseService.ReverseWebModel(context, options);
                }
                else if (deployedModel.Value.GetType() == typeof(ListDefinition))
                {
                    throw new SPMeta2NotImplementedException(
                        string.Format("Runner does not support model of type: [{0}]", deployedModel.Value.GetType()));
                }
                else
                {
                    throw new SPMeta2NotImplementedException(
                        string.Format("Runner does not support model of type: [{0}]", deployedModel.Value.GetType()));

                }

            });

            return reverseResut.Model;
        }

        private void DeployModel(ModelNode model)
        {
            WithCSOMContext(context =>
            {
                var provisionService = new StandardCSOMProvisionService();

                if (model.Value.GetType() == typeof(FarmDefinition))
                {
                    throw new SPMeta2NotImplementedException(
                        string.Format("Runner does not support model of type: [{0}]", model.Value.GetType()));
                }
                else if (model.Value.GetType() == typeof(WebApplicationDefinition))
                {
                    throw new SPMeta2NotImplementedException(
                     string.Format("Runner does not support model of type: [{0}]", model.Value.GetType()));
                }
                else if (model.Value.GetType() == typeof(SiteDefinition))
                {
                    provisionService.DeployModel(SiteModelHost.FromClientContext(context), model);
                }
                else if (model.Value.GetType() == typeof(WebDefinition))
                {
                    provisionService.DeployModel(WebModelHost.FromClientContext(context), model);
                }
                else if (model.Value.GetType() == typeof(ListDefinition))
                {
                    throw new SPMeta2NotImplementedException(
                     string.Format("Runner does not support model of type: [{0}]", model.Value.GetType()));
                }
                else
                {
                    throw new SPMeta2NotImplementedException(
                        string.Format("Runner does not support model of type: [{0}]", model.Value.GetType()));

                }
            });
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

        #region random model utils

        protected TDefinition Def<TDefinition>()
            where TDefinition : DefinitionBase
        {
            return Def<TDefinition>(null);
        }

        protected TDefinition Def<TDefinition>(Action<TDefinition> action)
            where TDefinition : DefinitionBase
        {
            return ModelGeneratorService.GetRandomDefinition<TDefinition>(action);
        }


        #endregion

        #region model filtering utils

        protected virtual IEnumerable<TDefinition> GetAllDefinitionOfType<TDefinition>(ModelNode model)
            where TDefinition : DefinitionBase
        {
            return model.Flatten()
                                 .Where(n => n.Value is TDefinition)
                                 .Select(s => s.Value as TDefinition);
        }

        #endregion
    }
}
