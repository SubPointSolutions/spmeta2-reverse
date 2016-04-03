using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Containers.Services;
using SPMeta2.Reverse.Tests.Services;

namespace SPMeta2.Reverse.Tests
{
    public static class ServiceFactory
    {
        #region static

        static ServiceFactory()
        {

        }

        #endregion

        #region properties

        public static RegressionAssertService AssertService = new ReverseRegressionAssertService();

        #endregion
    }
}
