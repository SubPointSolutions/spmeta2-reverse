using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Reverse.Services
{
    public abstract class ReverseOptionBase
    {
        public string DefinitionClassFullName { get; set; }
    }

    public class ReverseFilterOption : ReverseOptionBase
    {
        public ReverseFilter Filter { get; set; }
    }

    public class ReverseDepthOption : ReverseOptionBase
    {
        public string DefinitionClassFullName { get; set; }
        public int Depth { get; set; }
    }
}
