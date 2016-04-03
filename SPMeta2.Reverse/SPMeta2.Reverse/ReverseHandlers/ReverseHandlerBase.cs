using SPMeta2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Models;
using SPMeta2.Reverse.ReverseHosts;
using SPMeta2.Reverse.Services;

namespace SPMeta2.Reverse.ReverseHandlers
{
    public abstract class ReverseHandlerBase
    {
        public ReverseHandlerBase()
        {
            TraceService = ServiceContainer.Instance.GetService<TraceServiceBase>();
        }

        #region properties
        protected static TraceServiceBase TraceService { get; set; }

        /// <summary>
        /// Definition type on which reverse is performed
        /// </summary>
        public abstract Type ReverseType { get; }

        /// <summary>
        /// Parent definition typed on which current handler can work
        /// </summary>
        public abstract IEnumerable<Type> ReverseParentTypes { get; }

        #endregion

        #region methods

        public abstract IEnumerable<ReverseHostBase> ReverseHosts(ReverseHostBase parentHost, ReverseOptions options);

        public abstract ModelNode ReverseSingleHost(object reverseHost, ReverseOptions options);

        #endregion
    }
}
