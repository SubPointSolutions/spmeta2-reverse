using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Reverse.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Reverse.Tests.API
{
    [TestClass]
    public class ReverseOptionsTests
    {
        [TestMethod]
        [TestCategory("API.ReverseOptions")]
        [TestCategory("NET.Core")]
        public void CanBuildReverseOptions()
        {
            var options = ReverseOptions.Default
                                .AddOption<FieldDefinition>(f => f.Title == "10")
                                .AddOption<WebDefinition>(f => f.Title == "10");

            Assert.AreEqual(2, options.Options.Count());
        }

        [TestMethod]
        [TestCategory("API.ReverseOptions")]
        [TestCategory("NET.Core")]
        public void CanBuildDoubledReverseOptions()
        {
            // still should be 2 - perunique definition
            var options = ReverseOptions.Default

                                .AddOption<FieldDefinition>(f => f.Title == "10")
                                .AddOption<FieldDefinition>(f => f.Title == "10")
                                .AddOption<FieldDefinition>(f => f.Title == "10")

                                .AddOption<WebDefinition>(f => f.Title == "10")
                                .AddOption<WebDefinition>(f => f.Title == "10");

            Assert.AreEqual(2, options.Options.Count());
        }

        [TestMethod]
        [TestCategory("API.ReverseOptions")]
        [TestCategory("NET.Core")]
        public void CanBuildAlternativeSyntax1ReverseOptions()
        {
            var options = ReverseOptions.Default

                                .WithFields(f => f.Title == "10")
                                .WithWebs(f => f.Title == "10");

            Assert.AreEqual(2, options.Options.Count());
        }

        [TestMethod]
        [TestCategory("API.ReverseOptions")]
        [TestCategory("NET.Core")]
        public void CanBuildAlternativeSyntax2ReverseOptions()
        {
            var options = ReverseOptions.Default

                                .Include<FieldDefinition>(f => f.Title == "10")
                                .Include<WebDefinition>(f => f.Title == "10");

            Assert.AreEqual(2, options.Options.Count());
        }
    }
}
