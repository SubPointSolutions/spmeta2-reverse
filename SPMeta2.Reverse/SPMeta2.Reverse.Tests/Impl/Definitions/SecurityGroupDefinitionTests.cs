﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers;
using SPMeta2.Definitions;
using SPMeta2.Reverse.CSOM.ReverseHandlers;
using SPMeta2.Reverse.CSOM.Services;
using SPMeta2.Reverse.Services;
using SPMeta2.Reverse.Tests.Base;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Reverse.Tests.Impl.Definitions
{
    [TestClass]
    public class SecurityGroupDefinitionTests : ReverseTestBase
    {
        #region tests

        [TestMethod]
        [TestCategory("SecurityGroups")]
        public void Can_Reverse_SecurityGroups()
        {
            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddSecurityGroup(Def<SecurityGroupDefinition>());
                site.AddSecurityGroup(Def<SecurityGroupDefinition>());
            });

            DeployReverseAndTestModel(model);
        }

        #endregion
    }
}
