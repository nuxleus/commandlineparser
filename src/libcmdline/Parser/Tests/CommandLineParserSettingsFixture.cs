#region Copyright (C) 2005 - 2009 Giacomo Stelluti Scala
//
// Command Line Library: CommandLineParserSettingsFixture.cs
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
    using System.IO;
    using NUnit.Framework;

    [TestFixture]
    public sealed class CommandLineParserSettingsFixture
    {
        #region Mocks
        sealed class MockOptions
        {
            [Option(null, "filename")]
            public string FileName = string.Empty;

            [Option("o", "overwrite")]
            public bool Overwrite = false;

            [HelpOption]
            public string GetUsage()
            {
                return "MockOptions::GetUsage()";
            }
        }
        #endregion

        [Test]
        public void SettingHelpWriterUsingConstructor()
        {
            StringWriter writer = new StringWriter();
            ICommandLineParser parser = new CommandLineParser(new CommandLineParserSettings(writer));
            MockOptions options = new MockOptions();
            bool success = parser.ParseArguments(new string[] {"--help"}, options);
            Assert.AreEqual(false, success);
            Assert.AreEqual("MockOptions::GetUsage()", writer.ToString()); 
        }

        [Test]
        public void SettingHelpWriterUsingProperty()
        {
            StringWriter writer = new StringWriter();
            CommandLineParserSettings settings = new CommandLineParserSettings();
            settings.HelpWriter = writer;
            ICommandLineParser parser = new CommandLineParser(settings);
            MockOptions options = new MockOptions();
            bool success = parser.ParseArguments(new string[] { "--help" }, options);
            Assert.AreEqual(false, success);
            Assert.AreEqual("MockOptions::GetUsage()", writer.ToString());
        }

        [Test]
        public void SettingHelpWriterUsingArgument()
        {
            StringWriter writer = new StringWriter();
            ICommandLineParser parser = new CommandLineParser(new CommandLineParserSettings());
            MockOptions options = new MockOptions();
            bool success = parser.ParseArguments(new string[] { "--help" }, options, writer);
            Assert.AreEqual(false, success);
            Assert.AreEqual("MockOptions::GetUsage()", writer.ToString());
        }
    }
}
#endif