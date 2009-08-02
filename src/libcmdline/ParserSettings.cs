namespace CommandLine
{
    using System;
    using System.IO;

    /// <summary>
    /// Specifies a set of features to configure a <see cref="CommandLine.CommandLineParser"/> behavior.
    /// </summary>
    public sealed class ParserSettings
    {
        private bool caseSensitive = true;
        private TextWriter helpWriter = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLine.ParserSettings"/> class.
        /// </summary>
        public ParserSettings()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLine.ParserSettings"/> class,
        /// setting the case comparison behavior.
        /// </summary>
        /// <param name="caseSensitive">If set to true, parsing will be case sensitive.</param>
        public ParserSettings(bool caseSensitive)
        {
            this.caseSensitive = caseSensitive;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLine.ParserSettings"/> class,
        /// setting the <see cref="System.IO.TextWriter"/> used for help method output.
        /// </summary>
        /// <param name="helpWriter">Any instance derived from <see cref="System.IO.TextWriter"/>,
        /// default <see cref="System.Console.Error"/>.</param>
        public ParserSettings(TextWriter helpWriter)
        {
            this.helpWriter = helpWriter;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLine.ParserSettings"/> class,
        /// setting case comparison and help output options.
        /// </summary>
        /// <param name="caseSensitive">If set to true, parsing will be case sensitive.</param>
        /// <param name="helpWriter">Any instance derived from <see cref="System.IO.TextWriter"/>,
        /// default <see cref="System.Console.Error"/>.</param>
        public ParserSettings(bool caseSensitive, TextWriter helpWriter)
        {
            this.caseSensitive = caseSensitive;
            this.helpWriter = helpWriter;
        }

        /// <summary>
        /// Gets or sets the case comparison behavior.
        /// Default is set to true.
        /// </summary>
        public bool CaseSensitive
        {
            get { return this.caseSensitive; }
            set { this.caseSensitive = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="System.IO.TextWriter"/> used for help method output.
        /// </summary>
        public TextWriter HelpWriter
        {
            get
            {
                if (this.helpWriter == null)
                {
                    this.helpWriter = Console.Error;
                }
                return this.helpWriter;
            }
            set { this.helpWriter = value; }
        }
    }
}
