using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Reverse.Exceptions;
using SPMeta2.Reverse.Services;
using SPMeta2.Utils;

namespace SPMeta2.Reverse.CSOM.Foundation.Services
{
    public class ReverseFilterService
    {
        public virtual bool HasReverseFileters(ReverseOptions options, Type definitionType)
        {
            return FindReverseFileters(options, definitionType).Any();
        }

        public virtual List<ReverseFilterOption> FindReverseFileters(ReverseOptions options, Type definitionType)
        {
            var filters = options.Options.Where(o =>
                                    o.DefinitionClassFullName == definitionType.FullName
                                    && o is ReverseFilterOption)
                .Select(s => s as ReverseFilterOption)
                .ToList();

            return filters;
        }

        public virtual IEnumerable<TItemType> ApplyReverseFilters<TItemType>(
         IEnumerable<TItemType> items,
         Type definitionType,
         ReverseOptions options)
           where TItemType : class
        {
            //no filters, returning original collection
            if (!HasReverseFileters(options, definitionType))
                return items;

            // TODO, remove to a separate service
            // reverse filtering logic should not be in the handler

            // filtering colleciton as per the filters
            var result = new List<TItemType>();
            var reverseFilters = FindReverseFileters(options, definitionType);

            foreach (var reverseFilter in reverseFilters)
            {
                var filterPropName = reverseFilter.Filter.PropertyName;
                var filterPropValue = reverseFilter.Filter.PropertyValue;

                var filterOperation = reverseFilter.Filter.Operation;

                switch (filterOperation)
                {
                    case ReverseFilterOperationType.Equal:
                        {
                            foreach (var list in items)
                            {
                                var tmpPropValue = ReflectionUtils.GetPropertyValue(list, filterPropName);

                                // TODO
                                // a better check is required
                                if (tmpPropValue != null)
                                {
                                    if (filterPropValue.Equals(tmpPropValue))
                                    {
                                        result.Add(list);
                                    }
                                }
                            }
                        }

                        break;
                    default:
                        throw new SPMeta2ReverseException(
                            String.Format("Unsupported filter operation:[{0}]", filterOperation));
                }
            }

            // prevent multiple filter match
            result = result.Distinct().ToList();

            return result;
        }
    }
}
