using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Reverse.Exceptions;
using SPMeta2.Reverse.Services;
using SPMeta2.Utils;

namespace SPMeta2.Reverse.CSOM.Services
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

                foreach (var item in items)
                {
                    var currentPropValue = ReflectionUtils.GetPropertyValue(item, filterPropName);

                    switch (filterOperation)
                    {
                        case ReverseFilterOperationType.Equal:
                            {
                                if (currentPropValue != null)
                                {
                                    if (filterPropValue.Equals(currentPropValue))
                                    {
                                        result.Add(item);
                                    }
                                }
                            }
                            break;

                        case ReverseFilterOperationType.NotEqual:
                            {
                                if (currentPropValue != null)
                                {
                                    if (!filterPropValue.Equals(currentPropValue))
                                    {
                                        result.Add(item);
                                    }
                                }
                            }
                            break;

                        case ReverseFilterOperationType.StartsWith:
                            {
                                if (currentPropValue != null)
                                {
                                    var currentPropValueString = currentPropValue.ToString();

                                    if (currentPropValueString.StartsWith(filterPropValue))
                                    {
                                        result.Add(item);
                                    }
                                }
                            }
                            break;

                        case ReverseFilterOperationType.EndsWith:
                            {
                                if (currentPropValue != null)
                                {
                                    var currentPropValueString = currentPropValue.ToString();

                                    if (currentPropValueString.EndsWith(filterPropValue))
                                    {
                                        result.Add(item);
                                    }
                                }
                            }
                            break;

                        case ReverseFilterOperationType.Contains:
                            {
                                if (currentPropValue != null)
                                {
                                    var currentPropValueString = currentPropValue.ToString();

                                    if (currentPropValueString.Contains(filterPropValue))
                                    {
                                        result.Add(item);
                                    }
                                }
                            }
                            break;


                        default:
                            throw new SPMeta2ReverseException(
                                String.Format("Unsupported filter operation:[{0}]", filterOperation));
                    }
                }
            }

            // prevent multiple filter match
            result = result.Distinct().ToList();

            return result;
        }
    }
}
