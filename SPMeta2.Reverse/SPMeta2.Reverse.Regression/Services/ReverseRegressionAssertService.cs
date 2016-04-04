using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using SPMeta2.Containers.Assertion;
using SPMeta2.Containers.Extensions;
using SPMeta2.Containers.Services;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Reverse.Regression.Services
{
    public class ReverseRegressionAssertService : RegressionAssertService
    {
        #region classes

        public class ModelValidationResult
        {
            public ModelValidationResult()
            {
                Properties = new List<PropertyValidationResult>();
            }

            public DefinitionBase Model { get; set; }
            public List<PropertyValidationResult> Properties { get; set; }
        }

        #endregion

        #region properties

        private static List<ModelValidationResult> ModelValidations = new List<ModelValidationResult>();

        #endregion

        static ReverseRegressionAssertService()
        {
            RegExcludedDefinitionTypes = new List<Type>();

            ShowOnlyFalseResults = false;
            EnablePropertyValidation = true;

            OnPropertyValidated += OnReversePropertyValidated;
        }

        private static void OnReversePropertyValidated(object sender, OnPropertyValidatedEventArgs e)
        {
            var existingModelResult = ModelValidations.FirstOrDefault(r => r.Model == e.Result.Tag);

            if (existingModelResult == null)
            {
                existingModelResult = new ModelValidationResult
                {
                    Model = e.Result.Tag as DefinitionBase
                };

                ModelValidations.Add(existingModelResult);
            }

            existingModelResult.Properties.Add(e.Result);
        }

        public static bool ShowOnlyFalseResults { get; set; }
        public static bool EnablePropertyValidation { get; set; }


        public static List<Type> RegExcludedDefinitionTypes { get; set; }

        public static bool ResolveModelValidation(ModelNode modelNode)
        {
            return ResolveModelValidation(modelNode, string.Empty);
        }

        public static bool ResolveModelValidation(ModelNode modelNode, string start)
        {
            Trace.WriteLine(string.Format(""));

            var hasMissedOrInvalidProps = false;

            var model = modelNode.Value;
            Trace.WriteLine(string.Format("[INF] {2}MODEL CHECK [{0}] - ( {1} )", model.GetType(), model.ToString(), start));

            //if (model.RequireSelfProcessing || modelNode.Options.RequireSelfProcessing)
            if (modelNode.Options.RequireSelfProcessing)
            {
                var shouldProcessFlag = !modelNode.RegIsExcludedFromValidation();

                if (RegExcludedDefinitionTypes.Contains(modelNode.Value.GetType()))
                    shouldProcessFlag = false;

                if (shouldProcessFlag)
                {

                    var modelValidationResult = ModelValidations.FirstOrDefault(r => r.Model == model);

                    var shouldBeValidatedProperties = model.GetType()
                        .GetProperties()
                        .Where(
                            p =>
                                p.GetCustomAttributes<SPMeta2.Attributes.Regression.ExpectValidationAttribute>().Count() >
                                0)
                        .ToList();


                    if (modelValidationResult == null)
                    {
                        Trace.WriteLine(string.Format("[ERR]{2} Missing validation for model [{0}] - ( {1} )",
                            model.GetType(), model.ToString(), start));

                        hasMissedOrInvalidProps = true;
                        return hasMissedOrInvalidProps;
                    }

                    foreach (
                        var property in
                            modelValidationResult.Properties.OrderBy(p => p.Src != null ? p.Src.Name : p.ToString()))
                    {
                        if ((!property.IsValid) ||
                            (property.IsValid && !ShowOnlyFalseResults))
                        {
                            Trace.WriteLine(
                                string.Format(
                                    "[INF]{6} [{4}] - Src prop: [{0}] Src value: [{1}] Dst prop: [{2}] Dst value: [{3}] Message:[{5}]",
                                    new object[]
                                    {
                                        property.Src != null ? property.Src.Name : string.Empty,
                                        property.Src != null ? property.Src.Value : string.Empty,

                                        property.Dst != null ? property.Dst.Name : string.Empty,
                                        property.Dst != null ? property.Dst.Value : string.Empty,

                                        property.IsValid,
                                        property.Message,
                                        start
                                    }));
                        }

                        if (!property.IsValid)
                            hasMissedOrInvalidProps = true;

                    }

                    Trace.WriteLine(string.Format("[INF] {0}PROPERTY CHECK", start));

                    if (EnablePropertyValidation)
                    {
                        Trace.WriteLine(string.Format("[INF] {0}EnablePropertyValidation == true. Checking...", start));

                        foreach (var shouldBeValidatedProp in shouldBeValidatedProperties.OrderBy(p => p.Name))
                        {
                            var hasValidation = false;
                            var validationResult =
                                modelValidationResult.Properties.FirstOrDefault(
                                    r => r.Src != null && r.Src.Name == shouldBeValidatedProp.Name);

                            // convert stuff
                            if (validationResult == null)
                            {
                                validationResult = modelValidationResult.Properties.FirstOrDefault(
                                    r => r.Src != null && r.Src.Name.Contains("." + shouldBeValidatedProp.Name + ")"));
                            }

                            // nullables
                            if (validationResult == null)
                            {
                                validationResult = modelValidationResult.Properties.FirstOrDefault(
                                    r => r.Src != null &&
                                         (r.Src.Name.Contains("System.Nullable`1") &&
                                          r.Src.Name.Contains(shouldBeValidatedProp.Name)));
                            }

                            if (validationResult != null)
                            {
                                hasValidation = true;
                            }
                            else
                            {
                                hasMissedOrInvalidProps = true;
                                hasValidation = false;
                            }

                            if (hasValidation)
                            {
                                if (!ShowOnlyFalseResults)
                                {
                                    Trace.WriteLine(string.Format("[INF]{2} [{0}] - [{1}]",
                                        "VALIDATED",
                                        shouldBeValidatedProp.Name,
                                        start));
                                }
                            }
                            else
                            {
                                Trace.WriteLine(string.Format("[ERR]{2} [{0}] - [{1}]",
                                    "MISSED",
                                    shouldBeValidatedProp.Name,
                                    start));
                            }
                        }
                    }
                    else
                    {
                        Trace.WriteLine(string.Format("[INF]{0}EnablePropertyValidation == false. Skipping...", start));
                    }

                }
                else
                {
                    Trace.WriteLine(string.Format("[INF]{0} Skipping due .RegIsExcludedFromValidation ==  TRUE", start));
                }
            }
            else
            {
                Trace.WriteLine(string.Format("[INF]{0} Skipping due RequireSelfProcessing ==  FALSE", start));
            }

            foreach (var childModel in modelNode.ChildModels)
            {
                var tmp = ResolveModelValidation(childModel, start + start);

                if (tmp == true)
                    hasMissedOrInvalidProps = true;
            }

            return hasMissedOrInvalidProps;
        }
    }
}
