using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using SPMeta2.Models;
using SPMeta2.Reverse.Exceptions;
using SPMeta2.Reverse.ReverseHandlers;
using SPMeta2.Reverse.Services;
using SPMeta2.Services;
using SPMeta2.Utils;

namespace SPMeta2.Reverse.CSOM.Foundation.ReverseHandlers.Base
{
    public abstract class CSOMReverseHandlerBase : ReverseHandlerBase
    {
        #region methods

        protected virtual bool HasReverseFileters(ReverseOptions options)
        {
            return FindReverseFileters(options).Any();
        }

        protected virtual List<ReverseFilterOption> FindReverseFileters(ReverseOptions options)
        {
            var filters = options.Options.Where(o =>
                                    o.DefinitionClassFullName == this.ReverseType.FullName
                                    && o is ReverseFilterOption)
                .Select(s => s as ReverseFilterOption)
                .ToList();

            return filters;
        }

        protected virtual IEnumerable<TItemType> ApplyReverseFilters<TItemType>(
          IEnumerable<TItemType> items,
          ReverseOptions options)
            where TItemType :  class
        {
            //no filters, returning original collection
            if (!HasReverseFileters(options))
                return items;

            // TODO, remove to a separate service
            // reverse filtering logic should not be in the handler

            // filtering colleciton as per the filters
            var result = new List<TItemType>();
            var reverseFilters = FindReverseFileters(options);

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

        #endregion
    }
}
