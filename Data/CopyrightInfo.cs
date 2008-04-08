#region Copyright (C) 2005 - 2008 Giacomo Stelluti Scala
//
// Command Line Library: CopyrightInfo.cs
//
// Author:
//   Giacomo Stelluti Scala (giacomo.stelluti@gmail.com)
//
// Copyright (C) 2005 - 2008 Giacomo Stelluti Scala
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

namespace CommandLine
{
        using System;
        using System.Text;
        using System.Globalization;

        /// <summary>
        /// Models the copyright informations part of an help text.
        /// You can assign it where you assign any <see cref="System.String"/> instance.
        /// </summary>
        public class CopyrightInfo
        {
                /// <summary>
                /// Specifies the case of the copyright symbol.
                /// </summary>
                public enum Symbol
                {
                        /// <summary>
                        /// The copyright symbol will be of lower case.
                        /// </summary>
                        LowerCase,
                        /// <summary>
                        /// The copyright symbol will be of upper case.
                        /// </summary>
                        UpperCase
                }

                private Symbol symbol;
                private int[] years;
                private string author;
                private static readonly string defaultCopyrightWord = "Copyright";
                private static readonly string symbolLower = "(c)";
                private static readonly string symbolUpper = "(C)";
                private StringBuilder builder;

                /// <summary>
                /// Initializes a new instance of the <see cref="CommandLine.CopyrightInfo"/> class.
                /// </summary>
                protected CopyrightInfo()
                {
                }

                /// <summary>
                /// Initializes a new instance of the <see cref="CommandLine.CopyrightInfo"/> class
                /// specifying author and year.
                /// </summary>
                /// <param name="author">The company or person holding the copyright.</param>
                /// <param name="year">The year of coverage of copyright.</param>
                public CopyrightInfo(string author, int year)
                        : this(Symbol.UpperCase, author, new int[] { year })
                {
                }

                /// <summary>
                /// Initializes a new instance of the <see cref="CommandLine.CopyrightInfo"/> class
                /// specifying author and years.
                /// </summary>
                /// <param name="author">The company or person holding the copyright.</param>
                /// <param name="years">The years of coverage of copyright.</param>
                public CopyrightInfo(string author, params int[] years)
                        : this(Symbol.UpperCase, author, years)
                {
                }

                /// <summary>
                /// Initializes a new instance of the <see cref="CommandLine.CopyrightInfo"/> class
                /// specifying symbol case, author and years.
                /// </summary>
                /// <param name="symbol">The case of the copyright symbol.</param>
                /// <param name="author">The company or person holding the copyright.</param>
                /// <param name="years">The years of coverage of copyright.</param>
                public CopyrightInfo(Symbol symbol, string author, params int[] years)
                {
                        Validator.CheckIsNullOrEmpty(author, "author"); ;
                        Validator.CheckZeroLength(years, "years");

                        const int extraLength = 10;
                        this.symbol = symbol;
                        this.author = author;
                        this.years = years;
                        this.builder = new StringBuilder
                                (CopyrightWord.Length + author.Length + (4 * years.Length) + extraLength);
                }

                /// <summary>
                /// Returns the copyright informations as a <see cref="System.String"/>.
                /// </summary>
                /// <returns>The <see cref="System.String"/> that contains the copyright informations.</returns>
                public override string ToString()
                {
                        builder.Append(this.CopyrightWord);
                        builder.Append(' ');
                        if (this.symbol == Symbol.UpperCase)
                        {
                                builder.Append(symbolUpper);
                        }
                        else
                        {
                                builder.Append(symbolLower);    
                        }
                        builder.Append(' ');
                        builder.Append(this.FormatYears(years));
                        builder.Append(' ');
                        builder.Append(this.author);
                        return builder.ToString();
                }

                /// <summary>
                /// Converts the copyright informations to a <see cref="System.String"/>.
                /// </summary>
                /// <param name="info">This <see cref="CommandLine.CopyrightInfo"/> instance.</param>
                /// <returns>The <see cref="System.String"/> that contains the copyright informations.</returns>
                public static implicit operator string(CopyrightInfo info)
                {
                        return info.ToString();
                }

                /// <summary>
                /// When overridden in a derived class, allows to specify a different copyright word.
                /// </summary>
                protected virtual string CopyrightWord
                {
                        get { return defaultCopyrightWord; }
                }

                /// <summary>
                /// When overridden in a derived class, allows to specify a new algorithm to render copyright years
                /// as a <see cref="System.String"/> instance.
                /// </summary>
                /// <param name="years"></param>
                /// <returns></returns>
                protected virtual string FormatYears(int[] years)
                {
                        if (years.Length == 1)
                        {
                                return years[0].ToString(CultureInfo.InvariantCulture);
                        }

                        StringBuilder yearsPart = new StringBuilder(years.Length * 6);
                        for (int i = 0; i < years.Length; i++)
                        {
                                yearsPart.Append(years[i].ToString(CultureInfo.InvariantCulture));
                                int next = i + 1;
                                if (next < years.Length)
                                {
                                        if (years[next] - years[i] > 1)
                                        {
                                                yearsPart.Append(" - ");
                                        }
                                        else
                                        {
                                                yearsPart.Append(", ");
                                        }
                                }
                        }
                        return yearsPart.ToString();
                }
        }
}
