#region License
//
// Command Line Library: CommandLineParserFixture.cs
//
// Author:
//   Giacomo Stelluti Scala (gsscoder@ymail.com)
//
// Copyright (C) 2005 - 2009 Giacomo Stelluti Scala
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
#endregion
#region Using Directives
using System;
using System.IO;
using NUnit.Framework;
#endregion

#if UNIT_TESTS
namespace CommandLine.Tests
{
    [TestFixture]
    public sealed partial class CommandLineParserFixture : CommandLineParserBaseFixture
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WillThrowExceptionIfArgumentsArrayIsNull()
        {
            base.Parser.ParseArguments(null, new MockOptions());
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WillThrowExceptionIfOptionsInstanceIsNull()
        {
            base.Parser.ParseArguments(new string[] { }, null);
        }

        [Test]
        public void ParseStringOption()
        {
            var options = new MockOptions();
            bool success = base.Parser.ParseArguments(new string[] { "-s", "something" }, options);

            Assert.IsTrue(success);
            Assert.AreEqual("something", options.StringOption);
            Console.WriteLine(options);
        }

        [Test]
        public void ParseStringIntegerBoolOptions()
        {
            var options = new MockOptions();
            bool success = base.Parser.ParseArguments(
                    new string[] { "-s", "another string", "-i100", "--switch" }, options);

            Assert.IsTrue(success);
            Assert.AreEqual("another string", options.StringOption);
            Assert.AreEqual(100, options.IntOption);
            Assert.AreEqual(true, options.BoolOption);
            Console.WriteLine(options);
        }

        [Test]
        public void ParseShortAdjacentOptions()
        {
            var options = new MockBoolPrevalentOptions();
            bool success = base.Parser.ParseArguments(new string[] { "-ca", "-d65" }, options);

            Assert.IsTrue(success);
            Assert.IsTrue(options.OptionC);
            Assert.IsTrue(options.OptionA);
            Assert.IsFalse(options.OptionB);
            Assert.AreEqual(65, options.DoubleOption);
            Console.WriteLine(options);
        }

        [Test]
        public void ParseShortLongOptions()
        {
            var options = new MockBoolPrevalentOptions();
            bool success = base.Parser.ParseArguments(new string[] { "-b", "--double=9" }, options);

            Assert.IsTrue(success);
            Assert.IsTrue(options.OptionB);
            Assert.IsFalse(options.OptionA);
            Assert.IsFalse(options.OptionC);
            Assert.AreEqual(9, options.DoubleOption);
            Console.WriteLine(options);
        }
 
        [Test]
        public void ParseOptionList()
        {
            var options = new MockOptionsWithOptionList();
            bool success = base.Parser.ParseArguments(new string[] {
                                "-s", "string1:stringTwo:stringIII", "-f", "test-file.txt" }, options);

            Assert.IsTrue(success);
            Assert.AreEqual("string1", options.SearchKeywords[0]);
            Console.WriteLine(options.SearchKeywords[0]);
            Assert.AreEqual("stringTwo", options.SearchKeywords[1]);
            Console.WriteLine(options.SearchKeywords[1]);
            Assert.AreEqual("stringIII", options.SearchKeywords[2]);
            Console.WriteLine(options.SearchKeywords[2]);
        }

        /// <summary>
        /// Ref.: #BUG0000.
        /// </summary>
        [Test]
        public void ShortOptionRefusesEqualToken()
        {
            var options = new MockOptions();

            Assert.IsFalse(base.Parser.ParseArguments(new string[] { "-i=10" }, options));
            Console.WriteLine(options);
        }

        [Test]
        public void ParseEnumOptions()
        {
            var options = new MockOptionsWithEnum();

            bool success = base.Parser.ParseArguments(new string[] { "-f", "data.bin", "-a", "ReadWrite" }, options);

            Assert.IsTrue(success);
            Assert.AreEqual("data.bin", options.FileName);
            Assert.AreEqual(FileAccess.ReadWrite, options.FileAccess);
            Console.WriteLine(options);
        }

        #region #BUG0002
        [Test]
        public void ParsingNonExistentShortOptionFailsWithoutThrowingAnException()
        {
            var options = new MockOptions();
            bool success = base.Parser.ParseArguments(new string[] { "-x" }, options);

            Assert.IsFalse(success);
        }

        [Test]
        public void ParsingNonExistentLongOptionFailsWithoutThrowingAnException()
        {
            var options = new MockOptions();
            bool success = base.Parser.ParseArguments(new string[] { "--extend" }, options);

            Assert.IsFalse(success);
        }
        #endregion

        #region #REQ0000
        [Test]
        public void DefaultParsingIsCaseSensitive()
        {
            ICommandLineParser local = new CommandLineParser();
            var options = new MockOptionsCaseSensitive();
            bool success = local.ParseArguments(new string[] { "-a", "alfa", "--beta-OPTION", "beta" }, options);

            Assert.IsTrue(success);
            Assert.AreEqual("alfa", options.AlfaOption);
            Assert.AreEqual("beta", options.BetaOption);
        }

        [Test]
        public void UsingWrongCaseWithDefaultFails()
        {
            ICommandLineParser local = new CommandLineParser();
            var options = new MockOptionsCaseSensitive();
            bool success = local.ParseArguments(new string[] { "-A", "alfa", "--Beta-Option", "beta" }, options);

            Assert.IsFalse(success);
        }

        [Test]
        public void DisablingCaseSensitive()
        {
            ICommandLineParser local = new CommandLineParser(new CommandLineParserSettings(false)); //Ref.: #DGN0001
            var options = new MockOptionsCaseSensitive();
            bool success = local.ParseArguments(new string[] { "-A", "alfa", "--Beta-Option", "beta" }, options);

            Assert.IsTrue(success);
            Assert.AreEqual("alfa", options.AlfaOption);
            Assert.AreEqual("beta", options.BetaOption);
        }
        #endregion

 
        #region #BUG0003
        [Test]
        public void PassingNoValueToAStringTypeLongOptionFails()
        {
            var options = new MockOptions();
            bool success = base.Parser.ParseArguments(new string[] { "--string" }, options);

            Assert.IsFalse(success);
        }

        [Test]
        public void PassingNoValueToAByteTypeLongOptionFails()
        {
            var options = new MockNumericOptions();
            bool success = base.Parser.ParseArguments(new string[] { "--byte" }, options);

            Assert.IsFalse(success);
        }

        [Test]
        public void PassingNoValueToAShortTypeLongOptionFails()
        {
            var options = new MockNumericOptions();
            bool success = base.Parser.ParseArguments(new string[] { "--short" }, options);

            Assert.IsFalse(success);
        }

        [Test]
        public void PassingNoValueToAIntegerTypeLongOptionFails()
        {
            var options = new MockNumericOptions();
            bool success = base.Parser.ParseArguments(new string[] { "--int" }, options);

            Assert.IsFalse(success);
        }

        [Test]
        public void PassingNoValueToALongTypeLongOptionFails()
        {
            var options = new MockNumericOptions();
            bool success = base.Parser.ParseArguments(new string[] { "--long" }, options);

            Assert.IsFalse(success);
        }

        [Test]
        public void PassingNoValueToAFloatTypeLongOptionFails()
        {
            var options = new MockNumericOptions();
            bool success = base.Parser.ParseArguments(new string[] { "--float" }, options);

            Assert.IsFalse(success);
        }

        [Test]
        public void PassingNoValueToADoubleTypeLongOptionFails()
        {
            var options = new MockNumericOptions();
            bool success = base.Parser.ParseArguments(new string[] { "--double" }, options);

            Assert.IsFalse(success);
        }
        #endregion

        #region #REQ0001
        [Test]
        public void AllowSingleDashAsOptionInputValue()
        {
            var options = new MockOptions();
            bool success = base.Parser.ParseArguments(new string[] { "--string", "-" }, options);

            Assert.IsTrue(success);
            Assert.AreEqual("-", options.StringOption);
        }

        [Test]
        public void AllowSingleDashAsNonOptionValue()
        {
            var options = new MockOptionsExtended();
            bool success = base.Parser.ParseArguments(new string[] { "-sparser.xml", "-", "--switch" }, options);

            Assert.IsTrue(success);
            Assert.AreEqual("parser.xml", options.StringOption);
            Assert.AreEqual(true, options.BoolOption);
            Assert.AreEqual(1, options.Elements.Count);
            Assert.AreEqual("-", options.Elements[0]);
        }
        #endregion
    }
}
#endif