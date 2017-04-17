using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using SPMeta2.ModelHosts;
using SPMeta2.Models;
using SPMeta2.Reverse.Services;

namespace SPMeta2.Reverse.Regression.Base
{
    public class ReverseValidationModeHost : ModelHostBase
    {
        public ModelNode OriginalModel { get; set; }
        public ModelNode ReversedModel { get; set; }

        public ReverseOptions ReverseOptions { get; set; }
    }

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
