using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Reverse.CSOM.Services
{
    public class ReverseProgressEventArgs
    {
        public Type TargetType { get; set; }

        public ModelNode CurrentNode { get; set; }

        public int ProcessedModelNodeCount { get; set; }

        public int TotalModelNodeCount { get; set; }
    }
}
