using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SPMeta2.Reverse.Services
{

    public class ReverseFilter
    {
        public string PropertyName { get; set; }
        public string PropertyValue { get; set; }
        public ReverseFilterOperationType Operation { get; set; }

        public override string ToString()
        {
            return string.Format("Where '{0}' {1} '{2}'", PropertyName, Operation, PropertyValue);
        }

        public override bool Equals(object obj)
        {
            if (obj is ReverseFilter)
            {
                var tmpFilterOption = obj as ReverseFilter;

                return tmpFilterOption.PropertyName == this.PropertyName
                       && tmpFilterOption.PropertyValue == this.PropertyValue
                       && tmpFilterOption.Operation == this.Operation;
            }

            return base.Equals(obj);
        }
    }
}
