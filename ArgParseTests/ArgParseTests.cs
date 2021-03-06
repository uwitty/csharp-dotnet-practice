﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArgParse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgParse.Tests
{
    [TestClass()]
    public class ArgParseTests
    {
        [TestMethod()]
        public void ArgParseTest()
        {
            new ArgParser(desc: "test arg parse");
        }

        [TestMethod()]
        public void AddArgumentTest()
        {
            var parser = new ArgParser(desc: "add argument test parser");
            parser.AddArgument(name: "--required-option");
            Assert.IsTrue(parser.Arguments.ContainsKey("--required-option"));
            parser.AddArgument(name: "--option-with-default-value", defaultValue: "defaultValue");
            parser.AddArgument(name: "--option-with-help", help: "help message");
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void AddArgumentTest_Dup()
        {
            var parser = new ArgParser(desc: "add argument test parser");
            parser.AddArgument(name: "--duplicated-option");
            Assert.IsTrue(parser.Arguments.ContainsKey("--duplicated-option"));
            parser.AddArgument(name: "--duplicated-option");
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void AddArgumentTest_Position()
        {
            var parser = new ArgParser(desc: "add argument test parser");
            parser.AddArgument(name: "position-option");
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void AddArgumentTest_TooShort()
        {
            var parser = new ArgParser(desc: "add argument test parser");
            parser.AddArgument(name: "--");
        }

        [TestMethod()]
        public void ParseTest_Default()
        {
            var parser = new ArgParser(desc: "default option");
            parser.AddArgument("--option", "default");

            var result = parser.Parse(new string[0]);
            Assert.IsTrue(result.ContainsKey("--option"));
            Assert.AreEqual<string>("default", result.Get<string>("--option"));
            Assert.AreEqual<string>("default", (string)result["--option"]);
        }

        [TestMethod()]
        public void ParseTest()
        {
            var parser = new ArgParser(desc: "not default option");
            parser.AddArgument("--option", "default");

            var result = parser.Parse("--option not-default".Split(' '));
            Assert.IsTrue(result.ContainsKey("--option"));
            Assert.AreEqual<string>("not-default", result.Get<string>("--option"));
            Assert.AreEqual<string>("not-default", (string)result["--option"]);
        }

        [TestMethod()]
        public void ParseTest_Typed()
        {
            var parser = new ArgParser(desc: "typed options");
            parser.AddArgument("--option", "default");
            parser.AddArgument<int>("--int-option");

            var result = parser.Parse("--option not-default --int-option 1".Split(' '));
            Assert.IsTrue(result.ContainsKey("--option"));
            Assert.AreEqual<string>("not-default", (string)result["--option"]);
            Assert.AreEqual<string>("not-default", result.Get<string>("--option"));
            Assert.IsTrue(result.ContainsKey("--int-option"));
            Assert.AreEqual<int>(1, (int)result["--int-option"]);
            Assert.AreEqual<int>(1, result.Get<int>("--int-option"));
        }

        [TestMethod()]
        [ExpectedException(typeof(FormatException))]
        public void ParseTest_InvalidType()
        {
            var parser = new ArgParser(desc: "typed options");
            parser.AddArgument("--option", "default");
            parser.AddArgument<int>("--int-option");

            parser.Parse("--option not-default --int-option aaaa".Split(' '));
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void ParseTest_Unknown()
        {
            var parser = new ArgParser(desc: "unknown option");
            parser.AddArgument("--option", "default");

            parser.Parse("--option not-default --unknown-option 123".Split(' '));
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void ParseTest_MissRequired()
        {
            var parser = new ArgParser(desc: "unknown option");
            parser.AddArgument("--option", "default");
            parser.AddArgument("--required");

            parser.Parse("--option not-default".Split(' '));
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void ParseTest_InvalidArgs()
        {
            var parser = new ArgParser(desc: "unknown option");
            parser.AddArgument("--option", "default");

            parser.Parse("--option not-default --invalid-arg-name 123".Split(' '));
        }

        [TestMethod()]
        public void HelpTest()
        {
            var parser = new ArgParser(desc: "help check parser");
            parser.AddArgument(name: "--option");
            parser.AddArgument(name: "--name", metavar: "YOUR NAME");
            parser.AddArgument(name: "--xyz", metavar: "XYZ", help: "city hunter");
            var help = parser.Help;

            Assert.IsNotNull(help);
            Assert.AreEqual("help check parser\n    --name YOUR NAME\n    --option OPTION\n    --xyz XYZ\tcity hunter", help);
        }
    }
}