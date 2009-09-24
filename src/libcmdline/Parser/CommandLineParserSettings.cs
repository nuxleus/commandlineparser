#region Copyright (C) 2005 - 2009 Giacomo Stelluti Scala
//
// Command Line Library: CommandLineParserSettings.cs
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

namespace CommandLine
{
    using System.IO;

    /// <summary>
    /// Specifies a set of features to configure a <see cref="CommandLine.CommandLineParser"/> behavior.
    /// </summary>
    public sealed class CommandLineParserSettings
    {
        private bool caseSensitive = true;
        private TextWriter helpWriter = null;
        private bool mutuallyExclusive = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLine.CommandLineParserSettings"/> class.
        /// </summary>
        public CommandLineParserSettings()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLine.CommandLineParserSettings"/> class,
        /// setting the case comparison behavior.
        /// </summary>
        /// <param name="caseSensitive">If set to true, parsing will be case sensitive.</param>
        public CommandLineParserSettings(bool caseSensitive)
        {
            this.caseSensitive = caseSensitive;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLine.CommandLineParserSettings"/> class,
        /// setting the <see cref="System.IO.TextWriter"/> used for help method output.
        /// </summary>
        /// <param name="helpWriter">Any instance derived from <see cref="System.IO.TextWriter"/>,
        /// default <see cref="System.Console.Error"/>. Setting this argument to null, will disable help screen.</param>
        public CommandLineParserSettings(TextWriter helpWriter)
        {
            this.helpWriter = helpWriter;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLine.CommandLineParserSettings"/> class,
        /// setting case comparison and help output options.
        /// </summary>
        /// <param name="caseSensitive">If set to true, parsing will be case sensitive.</param>
        /// <param name="helpWriter">Any instance derived from <see cref="System.IO.TextWriter"/>,
        /// default <see cref="System.Console.Error"/>. Setting this argument to null, will disable help screen.</param>
        public CommandLineParserSettings(bool caseSensitive, TextWriter helpWriter)
        {
            this.caseSensitive = caseSensitive;
            this.helpWriter = helpWriter;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLine.CommandLineParserSettings"/> class,
        /// setting case comparison and mutually exclusive behaviors.
        /// </summary>
        /// <param name="caseSensitive">If set to true, parsing will be case sensitive.</param>
        /// <param name="mutuallyExclusive">If set to true, enable mutually exclusive behavior.</param>
        public CommandLineParserSettings(bool caseSensitive, bool mutuallyExclusive)
        {
            this.caseSensitive = caseSensitive;
            this.mutuallyExclusive = mutuallyExclusive;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLine.CommandLineParserSettings"/> class,
        /// setting case comparison, mutually exclusive behavior and help output option.
        /// </summary>
        /// <param name="caseSensitive">If set to true, parsing will be case sensitive.</param>
        /// <param name="mutuallyExclusive">If set to true, enable mutually exclusive behavior.</param>
        /// <param name="helpWriter">Any instance derived from <see cref="System.IO.TextWriter"/>,
        /// default <see cref="System.Console.Error"/>. Setting this argument to null, will disable help screen.</param>
        public CommandLineParserSettings(bool caseSensitive, bool mutuallyExclusive, TextWriter helpWriter)
        {
            this.caseSensitive = caseSensitive;
            this.mutuallyExclusive = mutuallyExclusive;
            this.helpWriter = helpWriter;
        }

        /// <summary>
        /// Gets or sets the case comparison behavior.
        /// Default is set to true.
        /// </summary>
        public bool CaseSensitive
        {
            internal get { return this.caseSensitive; }
            set { this.caseSensitive = value; }
        }

        /// <summary>
        /// Gets or sets the mutually exclusive behavior.
        /// Default is set to false.
        /// </summary>
        public bool MutuallyExclusive
        {
            internal get { return this.mutuallyExclusive; }
            set { this.mutuallyExclusive = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="System.IO.TextWriter"/> used for help method output.
        /// Setting this property to null, will disable help screen.
        /// </summary>
        public TextWriter HelpWriter
        {
            internal get { return this.helpWriter; }
            set { this.helpWriter = value; }
        }
    }
}
