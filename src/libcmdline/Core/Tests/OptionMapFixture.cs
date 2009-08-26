#region Copyright (C) 2005 - 2009 Giacomo Stelluti Scala
//
// Command Line Library: OptionMapFixture.cs
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
    using CommandLine;
    using NUnit.Framework;

    [TestFixture]
    public sealed class OptionMapFixture
    {
        #region Helper Nested Class
        class OptionMapBuilder
        {
            private readonly IOptionMap optionMap;
            private readonly List<OptionInfo> options;
            private readonly List<string> names;

            public OptionMapBuilder(int capacity)
            {
                this.optionMap = new OptionMap(capacity, new CommandLineParserSettings(true));
                this.options = new List<OptionInfo>(capacity);
                this.names = new List<string>(capacity);
            }

            public void AppendOption(string shortName, string longName)
            {
                OptionAttribute oa = new OptionAttribute(shortName, longName);
                OptionInfo oi = oa.CreateOptionInfo();
                this.optionMap[oa.UniqueName] = oi;
                this.options.Add(oi);
                this.names.Add(oa.UniqueName);
            }

            public List<OptionInfo> Options
            {
                get { return this.options; }
            }

            public List<string> Names
            {
                get { return this.names; }
            }

            public IOptionMap OptionMap
            {
                get { return this.optionMap; }
            }
        }
        #endregion
        private static IOptionMap optionMap;
        private static OptionMapBuilder omBuilder;

        [SetUp]
        public void CreateInstance()
        {
            omBuilder = new OptionMapBuilder(3);
            omBuilder.AppendOption("p", "pretend");
            omBuilder.AppendOption(null, "newuse");
            omBuilder.AppendOption("D", null);

            optionMap = omBuilder.OptionMap;
        }

        [TearDown]
        public void ShutdownInstance()
        {
            optionMap = null;
        }

        [Test]
        public void ManageOptions()
        {
            Assert.AreSame(omBuilder.Options[0], optionMap[omBuilder.Names[0]]);
            Assert.AreSame(omBuilder.Options[1], optionMap[omBuilder.Names[1]]);
            Assert.AreSame(omBuilder.Options[2], optionMap[omBuilder.Names[2]]);
        }

        [Test]
        public void RetrieveNotExistentShortOption()
        {
            OptionInfo shortOi = optionMap["y"];
            Assert.IsNull(shortOi);
        }

        [Test]
        public void RetrieveNotExistentLongOption()
        {
            OptionInfo longOi = optionMap["nomorebugshere"];
            Assert.IsNull(longOi);
        }

        private static IOptionMap CreateMap(ref OptionMap map,  IDictionary<string, OptionInfo> optionCache)
        {
            if (map == null)
            {
                map = new OptionMap(3, new CommandLineParserSettings(true));
            }

            OptionAttribute attribute1 = new OptionAttribute("p", "pretend");
            OptionAttribute attribute2 = new OptionAttribute(null, "newuse");
            OptionAttribute attribute3 = new OptionAttribute("D", null);

            OptionInfo option1 = attribute1.CreateOptionInfo();
            OptionInfo option2 = attribute2.CreateOptionInfo();
            OptionInfo option3 = attribute3.CreateOptionInfo();

            map[attribute1.UniqueName] = option1;
            map[attribute2.UniqueName] = option2;
            map[attribute3.UniqueName] = option3;

            if (optionCache != null)
            {
                optionCache[attribute1.UniqueName] = option1;
                optionCache[attribute1.UniqueName] = option2;
                optionCache[attribute2.UniqueName]= option3;
            }

            return map;
        }
    }
}
#endif