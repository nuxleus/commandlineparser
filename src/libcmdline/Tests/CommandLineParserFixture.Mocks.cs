#region Copyright (C) 2005 - 2009 Giacomo Stelluti Scala
//
// Command Line Library: CommandLineParserFixture.Mocks.cs
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
    using System.Collections.Generic;
    using System.IO;

    public sealed partial class CommandLineParserFixture
	{
        class MockOptions : MockOptionsBase
        {
            [Option("s", "string")]
            public string StringOption = string.Empty;

            [Option("i", null)]
            public int IntOption = 0;

            [Option(null, "switch")]
            public bool BoolOption = false;
        }

        class MockBoolPrevalentOptions : MockOptionsBase
        {
            [Option("a", "option-a")]
            public bool OptionA = false;

            [Option("b", "option-b")]
            public bool OptionB = false;

            [Option("c", "option-c")]
            public bool OptionC = false;

            [Option("d", "double")]
            public double DoubleOption = 0;
        }

        class MockOptionsWithValueList : MockOptionsBase
        {
            [Option("o", "output")]
            public string OutputFile = string.Empty;

            [Option("w", "overwrite")]
            public bool Overwrite = false;

            [ValueList(typeof(List<string>))]
            public IList<string> InputFilenames = null;
        }

        class MockOptionsWithValueListMaxElemDefined : MockOptionsBase
        {
            [Option("o", "output")]
            public string OutputFile = string.Empty;

            [Option("w", "overwrite")]
            public bool Overwrite = false;

            [ValueList(typeof(List<string>), MaximumElements = 3)]
            public IList<string> InputFilenames = null;
        }

        class MockOptionsWithValueListMaxElemEqZero : MockOptionsBase
        {
            [ValueList(typeof(List<string>), MaximumElements = 0)]
            public IList<string> Junk = null;
        }

        class MockOptionsWithOptionList
        {
            [Option("f", "filename")]
            public string FileName = string.Empty;

            [OptionList("s", "search", ':')]
            public IList<string> SearchKeywords = null;
        }

        class MockOptionsWithEnum : MockOptionsBase
        {
            [Option("f", "filename", Required = true)]
            public string FileName = string.Empty;

            [Option("a", "access", Required = true)]
            public FileAccess FileAccess = FileAccess.Read;
        }

        class MockOptionsCaseSensitive : MockOptionsBase
        {
            [Option("a", "Alfa-Option")]
            public string AlfaOption = string.Empty;

            [Option("b", "beta-OPTION")]
            public string BetaOption = string.Empty;

            [HelpOption(
                    HelpText = "Dispaly this help screen.")]
            public string GetUsage()
            {
                return "Needed when using ParserSettings object.";
            }
        }
	}
}
#endif
