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

        public override string ToString()
        {
            var className = DefinitionClassFullName.Split('.').Last();
            return string.Format("Fetch '{0}' as '{1}'", className, Filter);
        }

        public override bool Equals(object obj)
        {
            if (obj is ReverseFilterOption)
            {
                var tmpFilterOption = obj as ReverseFilterOption;
                return tmpFilterOption.DefinitionClassFullName == this.DefinitionClassFullName
                       && tmpFilterOption.Filter.Equals(this.Filter);
            }

            return base.Equals(obj);
        }
    }

    public class ReverseDepthOption : ReverseOptionBase
    {
        public int Depth { get; set; }

        public override string ToString()
        {
            var className = DefinitionClassFullName.Split('.').Last();
            return string.Format("Fetch '{0}' with depth '{1}'", className, Depth);
        }
    }
}
