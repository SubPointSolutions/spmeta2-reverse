using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Models;
using SPMeta2.Reverse.ReverseHandlers;

namespace SPMeta2.Reverse.Services
{
    public abstract class ReverseServiceBase
    {
        #region constructors

        public ReverseServiceBase()
        {
            Handlers = new List<ReverseHandlerBase>();
        }

        #endregion

        #region properties

        public List<ReverseHandlerBase> Handlers { get; set; }

        #endregion

        #region methods

        public ReverseResult Reverse(object modelHost)
        {
            return Reverse(modelHost, ReverseOptions.Default);
        }
        public abstract ReverseResult Reverse(object modelHost, ReverseOptions options);

        #endregion
    }
}
