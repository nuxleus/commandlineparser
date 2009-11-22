#region License
//
// Command Line Library: ArgumentParserFixture.cs
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
#if UNIT_TESTS
#region Using Directives
using System;
using System.Collections.Generic;
using NUnit.Framework;
#endregion

namespace CommandLine.Tests
{
    [TestFixture]
    public sealed class ArgumentParserFixture
    {
        class MockParser : ArgumentParser
        {
            public override ParserState Parse(IArgumentEnumerator argumentEnumerator, OptionMap map, object options)
            {
                throw new NotImplementedException();
            }

            public IList<string> PublicWrapperOfGetNextInputValues(IArgumentEnumerator ae)
            {
                return base.GetNextInputValues(ae);
            }
        }

        [Test]
        public void GetNextInputValues()
        {
            var mock = new MockParser();
            var ae = new StringArrayEnumerator(new string[] { "--optarr", "one", "two", "--plain", "3" });

            ae.MoveNext(); // skip first, working like in a real case

            var items = mock.PublicWrapperOfGetNextInputValues(ae);

            Assert.AreEqual(2, items.Count);
            Assert.AreEqual("one", items[0]);
            Assert.AreEqual("two", items[1]);
            Assert.AreEqual("two", ae.Current);
        }
    }
}
#endif