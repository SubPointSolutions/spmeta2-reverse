using SPMeta2.Reverse.Regression.Services;

namespace SPMeta2.Reverse.Regression
{
    public static class ServiceFactory
    {
        #region static

        static ServiceFactory()
        {

        }

        #endregion

        #region properties

        public static ReverseRegressionAssertService AssertService = new ReverseRegressionAssertService();

        #endregion
    }
}
