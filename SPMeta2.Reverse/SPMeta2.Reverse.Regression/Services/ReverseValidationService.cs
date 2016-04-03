using SPMeta2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Reverse.Regression.Base;

namespace SPMeta2.Reverse.Regression.Services
{
    public class ReverseValidationService : ModelServiceBase
    {
        #region constructors

        public ReverseValidationService()
        {
            RegisterModelHandlers<ReverseDefinitionValidatorBase>(this, GetType().Assembly);
        }

        #endregion
    }
}
