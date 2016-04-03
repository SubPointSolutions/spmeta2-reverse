using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;

namespace SPMeta2.Reverse.Regression.Base
{
    public abstract class ReverseDefinitionValidatorBase : ModelHandlerBase
    {

    }

    public abstract class TypedReverseDefinitionValidatorBase<TDefinition> : ReverseDefinitionValidatorBase
        where TDefinition : DefinitionBase
    {
        public override Type TargetType
        {
            get { return typeof(TDefinition); }
        }
    }
}
