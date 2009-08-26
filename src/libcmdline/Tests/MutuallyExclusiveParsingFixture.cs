#region Copyright (C) 2005 - 2009 Giacomo Stelluti Scala
//
// Command Line Library: MutuallyExclusiveParsingFixture.cs
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
#endregion

#if UNIT_TESTS
namespace CommandLine.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public sealed class MutuallyExclusiveParsingFixture
    {
        private static ICommandLineParser parser = new CommandLineParser(
            new CommandLineParserSettings(false, true));  //mutually exclusive setting enabled (second parameter)

        #region Mock Options
        class MockOptionsWithDefaultSet
        {
            [Option("f", "file",
                MutuallyExclusiveSet = null)]
            public string FileName = string.Empty;

            [Option("i", "file-id",
                MutuallyExclusiveSet = null)]
            public int FileId = int.MinValue;

            [Option("d", "file-default",
                MutuallyExclusiveSet = null)]
            public bool FileDefault = false;

            [Option("v", "verbose")]
            public bool Verbose = false;
        }

        class MockColorOptions //Multiple Set
        {
            // rgb mutually exclusive set
            [Option("r", "red", MutuallyExclusiveSet = "rgb")]
            public byte Red = 0;

            [Option("g", "green", MutuallyExclusiveSet = "rgb")]
            public byte Green = 0;

            [Option("b", "blue", MutuallyExclusiveSet = "rgb")]
            public byte Blue = 0;

            // hsv mutually exclusive set
            [Option("h", "hue", MutuallyExclusiveSet = "hsv")]
            public short Hue = 0;

            [Option("s", "saturation", MutuallyExclusiveSet = "hsv")]
            public byte Saturation = 0;

            [Option("v", "value", MutuallyExclusiveSet = "hsv")]
            public byte Value = 0;
        }

        enum ColorSet
        {
            Undefined,
            RgbColorSet,
            HsvColorSet
        }

        class MockExtendedColorOptions : MockColorOptions
        {
            [Option("c", "default-color-set",
                Required = true)]
            public ColorSet DefaultColorSet = ColorSet.Undefined;
        }
        #endregion

        [Test]
        public void ParsingOneMutuallyExclusiveOptionSucceeds()
        {
            MockOptionsWithDefaultSet options = new MockOptionsWithDefaultSet();
            bool success = parser.ParseArguments(new string[] { "--file=mystuff.xml" }, options);
            Assert.IsTrue(success);

            Assert.AreEqual("mystuff.xml", options.FileName);
        }

        [Test]
        public void ParsingTwoMutuallyExclusiveOptionsFails()
        {
            MockOptionsWithDefaultSet options = new MockOptionsWithDefaultSet();
            bool success = parser.ParseArguments(new string[] { "-i", "1", "--file=mystuff.xml" }, options);
            Assert.IsFalse(success);
        }

        [Test]
        public void ParsingOneMutuallyExclusiveOptionWithAnotherOptionSucceeds()
        {
            MockOptionsWithDefaultSet options = new MockOptionsWithDefaultSet();
            bool success = parser.ParseArguments(new string[] { "--file=mystuff.xml", "-v" }, options);
            Assert.IsTrue(success);

            Assert.AreEqual("mystuff.xml", options.FileName);
            Assert.AreEqual(true, options.Verbose);
        }

        [Test]
        public void ParsingTwoMutuallyExclusiveOptionsInTwoSetSucceeds()
        {
            MockColorOptions options = new MockColorOptions();
            bool success = parser.ParseArguments(new string[] { "-g167", "--hue", "205" }, options);
            Assert.IsTrue(success);

            Assert.AreEqual(167, options.Green);
            Assert.AreEqual(205, options.Hue);
        }

        [Test]
        public void ParsingThreeMutuallyExclusiveOptionsInTwoSetFails()
        {
            MockColorOptions options = new MockColorOptions();
            bool success = parser.ParseArguments(new string[] { "-g167", "--hue", "205", "--saturation=37" }, options);
            Assert.IsFalse(success);
        }

        [Test]
        public void ParsingMutuallyExclusiveOptionsAndRequiredOptionFails()
        {
            MockExtendedColorOptions options = new MockExtendedColorOptions();
            bool success = parser.ParseArguments(new string[] { "-g167", "--hue", "205" }, options);
            Assert.IsFalse(success);
        }

        [Test]
        public void ParsingMutuallyExclusiveOptionsAndRequiredOptionSucceeds()
        {
            MockExtendedColorOptions options = new MockExtendedColorOptions();
            bool success = parser.ParseArguments(new string[] { "-g100", "-h200", "-cRgbColorSet" }, options);
            Assert.IsTrue(success);

            Assert.AreEqual(100, options.Green);
            Assert.AreEqual(200, options.Hue);
            Assert.AreEqual(ColorSet.RgbColorSet, options.DefaultColorSet);
        }

    }
}
#endif
