using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Models;
using SPMeta2.Reverse.ReverseHandlers;
using SPMeta2.Reverse.Services;
using SPMeta2.Services;

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

        #endregion
    }
}
