using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Reverse.CSOM.Foundation.Services;
using SPMeta2.Reverse.Services;

namespace SPMeta2.Reverse.CSOM.Standard.Services
{
    public class StandardCSOMReverseService : CSOMReverseService
    {
        #region constructors

        public StandardCSOMReverseService()
        {
            RegisterReverseHandlers(typeof(StandardCSOMReverseService).Assembly);
        }


        #endregion
    }
}
