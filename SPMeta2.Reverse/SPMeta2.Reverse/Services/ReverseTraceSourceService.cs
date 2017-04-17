using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using SPMeta2.Services;
using SPMeta2.Services.Impl;
using SPMeta2.Utils;

namespace SPMeta2.Reverse.Services
{
    public class ReverseTraceSourceService : TraceSourceService
    {
        #region constructors

        public ReverseTraceSourceService()
            : base("SPMeta2.Reverse")
        {
           
        }

        #endregion

    }
}
