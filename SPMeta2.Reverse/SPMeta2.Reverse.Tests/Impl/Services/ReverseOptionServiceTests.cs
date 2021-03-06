﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Reverse.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;


namespace SPMeta2.Reverse.Tests.Impl.Services
{
    [TestClass]
    public class ReverseOptionServiceTests
    {
        #region tests

        [TestMethod]
        [TestCategory("Services.ReverseOptionService")]
        [TestCategory("CI.Core")]
        public void ReverseOptionService_CanParseOptions()
        {
            var service = new ReverseOptionService();

            var expressions = new List<Expression<Func<FieldDefinition, bool>>>();

            var guid = new Guid("02482472-F002-4FB6-8EA9-6F9D799A46E8");

            // equal
            expressions.Add((d => d.Title == "test"));

            // not equal
            expressions.Add((d => d.Id != guid));

            // starts with
            expressions.Add((d => d.Title.StartsWith("test")));

            // ends with
            expressions.Add((d => d.Title.EndsWith("test")));

            var result = service.ParseOptionFilters(expressions);

            Assert.IsTrue(result != null);
            Assert.IsTrue(result.Count == expressions.Count);
        }

        #endregion
    }
}
