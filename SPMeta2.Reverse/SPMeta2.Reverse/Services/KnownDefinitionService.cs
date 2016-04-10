using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Utils;

namespace SPMeta2.Reverse.Services
{
    public class KnownDefinitionService
    {
        #region methods

        public IEnumerable<FeatureDefinition> GetKnownWebFeatures()
        {
            return GetKnownWebFeatures(null);
        }

        public IEnumerable<FeatureDefinition> GetKnownWebFeatures(
            IEnumerable<FeatureDefinition> additionalFeatures)
        {
            var result = new List<FeatureDefinition>();

            var items = typeof(BuiltInWebFeatures)
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .Where(f => f.FieldType == typeof(FeatureDefinition))
                .Select(f => f.GetValue(null) as FeatureDefinition);

            result.AddRange(items);

            if (additionalFeatures != null)
                result.AddRange(additionalFeatures);

            return result;
        }

        public IEnumerable<FeatureDefinition> GetKnownSiteFeatures()
        {
            return GetKnownSiteFeatures(null);
        }

        public IEnumerable<FeatureDefinition> GetKnownSiteFeatures(
            IEnumerable<FeatureDefinition> additionalFeatures)
        {
            var result = new List<FeatureDefinition>();

            var items = typeof(BuiltInSiteFeatures)
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .Where(f => f.FieldType == typeof(FeatureDefinition))
                .Select(f => f.GetValue(null) as FeatureDefinition);

            result.AddRange(items);

            if (additionalFeatures != null)
                result.AddRange(additionalFeatures);

            return result;
        }

        #endregion
    }
}
