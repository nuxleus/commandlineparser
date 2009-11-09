#region License
//
// Command Line Library: HelpTextFixture.cs
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
#region Using Directives
using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
#endregion

namespace CommandLine.Text.Tests
{
    [TestFixture]
    public sealed class HelpTextFixture
    {
        class MockOptions
        {
            [Option("v", "verbose")]
            public bool Verbose = false;

            [Option(null, "input-file")]
            public string FileName = string.Empty;
        }

        private static HelpText _helpText = new HelpText(
            new HeadingInfo(ThisAssembly.Title, ThisAssembly.Version));

        [Test]
        public void AddAnEmptyPreOptionsLineIsAllowed()
        {
            _helpText.AddPreOptionsLine(string.Empty); // == ""
        }

        /// <summary>
        /// Ref.: #REQ0002
        /// </summary>
        [Test]
        public void PostOptionsLinesFeatureAdded()
        {
            var local = new HelpText("Heading Info.");
            local.AddPreOptionsLine("This is a first pre-options line.");
            local.AddPreOptionsLine("This is a second pre-options line.");
            local.AddOptions(new MockOptions());
            local.AddPostOptionsLine("This is a first post-options line.");
            local.AddPostOptionsLine("This is a second post-options line.");

            string help = local.ToString();
            Console.Write(help);

            string[] lines = help.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            Assert.AreEqual(lines[lines.Length - 2], "This is a first post-options line.");
            Assert.AreEqual(lines[lines.Length - 1], "This is a second post-options line.");
        }
    }
}
#endif
