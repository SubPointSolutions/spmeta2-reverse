﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers.Services;
using SPMeta2.Definitions;
using SPMeta2.Reverse.CSOM.ReverseHosts;
using SPMeta2.Reverse.CSOM.Services;
using SPMeta2.Syntax.Default;
using SPMeta2.Reverse.CSOM.Standard.Services;
using SPMeta2.Reverse.Services;

namespace SPMeta2.Reverse.Tests.Impl.Services
{
    [TestClass]
    public class CSOMReverseServiceTests
    {
        #region tests

        [TestMethod]
        [TestCategory("Services.ReverseService")]
        [TestCategory("CI.Core")]
        public void CSOMReverseService_Has_Handlers()
        {
            var service = new CSOMReverseService();

            Assert.IsTrue(service.Handlers.Count > 0);
        }

        [TestMethod]
        [TestCategory("Services.ReverseService")]
        [TestCategory("CI.Core")]
        public void StandardCSOMReverseService_Has_Handlers()
        {
            var service = new StandardCSOMReverseService();

            Assert.IsTrue(service.Handlers.Count > 0);
        }

        #endregion
    }
}
