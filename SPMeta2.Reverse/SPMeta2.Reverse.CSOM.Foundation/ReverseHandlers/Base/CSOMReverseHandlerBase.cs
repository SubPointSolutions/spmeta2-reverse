using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using SPMeta2.Models;
using SPMeta2.Reverse.CSOM.Foundation.Services;
using SPMeta2.Reverse.Exceptions;
using SPMeta2.Reverse.ReverseHandlers;
using SPMeta2.Reverse.Services;
using SPMeta2.Services;
using SPMeta2.Utils;

namespace SPMeta2.Reverse.CSOM.Foundation.ReverseHandlers.Base
{
    public abstract class CSOMReverseHandlerBase : ReverseHandlerBase
    {
        #region constructors

        internal CSOMReverseHandlerBase()
        {
            ReverseFilterService = new ReverseFilterService();
        }

        #endregion

        #region properties

        protected ReverseFilterService ReverseFilterService { get; set; }

        #endregion

        #region methods

        public virtual bool HasReverseFileters(ReverseOptions options)
        {
            return FindReverseFileters(options).Any();
        }

        public virtual List<ReverseFilterOption> FindReverseFileters(ReverseOptions options)
        {
            return ReverseFilterService.FindReverseFileters(options, this.ReverseType);
        }

        protected virtual IEnumerable<TItemType> ApplyReverseFilters<TItemType>(
          IEnumerable<TItemType> items,
          ReverseOptions options)
            where TItemType : class
        {
            return ReverseFilterService.ApplyReverseFilters(items, ReverseType, options);
        }

        #endregion
    }
}
