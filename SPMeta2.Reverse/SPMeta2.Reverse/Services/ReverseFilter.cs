using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Reverse.Services
{
    public class ReverseFilter
    {
        public string PropertyName { get; set; }
        public string PropertyValue { get; set; }
        public string Operation { get; set; }

        public override string ToString()
        {
            return string.Format("Where '{0}' {1} '{2}'", PropertyName, Operation, PropertyValue);
        }
    }
}
