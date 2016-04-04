using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Attributes.Identity;
using SPMeta2.Definitions;
using SPMeta2.Reverse.Exceptions;
using SPMeta2.Utils;
using SPMeta2.Definitions.Base;

namespace SPMeta2.Reverse.Regression.Services
{
    public class ReverseModelIdentityService
    {
        public string GetDefinitionIdentityKey(DefinitionBase def)
        {
            var result = string.Empty;

            var definitionType = def.GetType();

            var isSingleIdenity = definitionType.GetCustomAttributes(typeof(SingletonIdentityAttribute), true).Any();
            var isInstanceIdentity = !definitionType.GetCustomAttributes(typeof(SingletonIdentityAttribute), true).Any();

            if (isSingleIdenity)
            {
                throw new SPMeta2ReverseException("isSingleIdenity is true. Was not implemented yet");
            }

            if (isInstanceIdentity)
            {
                var props = definitionType.GetProperties();

                var identityKeyNames = props
                    .Where(p => p.GetCustomAttributes(typeof(IdentityKeyAttribute), true).Any())
                    .Select(p => p.Name)
                    .OrderBy(s => s)
                    .ToList();

                // url gets transformed by SharePoint to the full one
                // so that lookup by identity won't work
                // rely only on title for the time being
                if (def is NavigationNodeDefinitionBase)
                {
                    identityKeyNames.Remove("Url");
                }

                // skipping list view URLs fro the tiime being
                if (def is ListViewDefinition)
                {
                    identityKeyNames.Remove("Url");
                }

                foreach (var keyName in identityKeyNames)
                {
                    var prop = props.FirstOrDefault(p => p.Name == keyName);
                    var keyValue = ConvertUtils.ToString(prop.GetValue(def, null));

                    result += keyValue;
                }

            }

            return result;
        }
    }
}
