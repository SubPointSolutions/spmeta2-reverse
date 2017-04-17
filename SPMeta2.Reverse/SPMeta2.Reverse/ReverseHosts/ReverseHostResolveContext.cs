using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SPMeta2.Reverse.ReverseHosts
{
    public class ReverseHostResolveContext
    {
        public object ModelHost { get; set; }
        public Action<object> Action { get; set; }

    }
}
