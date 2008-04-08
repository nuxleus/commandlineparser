#region Copyright (C) 2005 - 2008 Giacomo Stelluti Scala
//
// Command Line Library: OptionAttribute.cs
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
        using System.Collections.Generic;

        /// <summary>
        /// Models an help text and collects related informations.
        /// You can assign it where you assign any <see cref="System.String"/> instance.
        /// </summary>
        public class HelpText
        {
                private const int builderCapacity = 255;
                private string programName;
                private string version;
                private string copyright;
                private StringBuilder license;
                private StringBuilder usage;
                private StringBuilder optionsHelp;
                private static readonly string defaultRequiredWord = "Required.";

                /// <summary>
                /// Initializes a new instance of the <see cref="CommandLine.HelpText"/> class
                /// specifying the program name.
                /// </summary>
                /// <param name="programName">The program name printed out in the help screen.</param>
                public HelpText(string programName)
                        : this(programName, null)
                {
                }

                /// <summary>
                /// Initializes a new instance of the <see cref="CommandLine.HelpText"/> class
                /// specifying the program name and version.
                /// </summary>
                /// <param name="programName">The program name printed out in the help screen.</param>
                /// <param name="version">The program version printed out in the help screen.</param>
                public HelpText(string programName, string version)
                {
                        Validator.CheckIsNullOrEmpty(programName, programName);

                        this.programName = programName;
                        this.version = version;
                }

                /// <summary>
                /// Sets the copyright information string.
                /// You can directly assign a <see cref="CommandLine.CopyrightInfo"/> instance.
                /// </summary>
                public string Copyright
                {
                        set { this.copyright = value; }
                }

                /// <summary>
                /// Adds a text line with license informations.
                /// </summary>
                /// <param name="value">A <see cref="System.String"/> instance that contains license informations.</param>
                public void AddLicenseLine(string value)
                {
                        AddLine(this.LicenseBuilder, value);
                }

                /// <summary>
                /// Adds a text line with usage informations.
                /// </summary>
                /// <param name="value">A <see cref="System.String"/> instance that contains usage informations.</param>
                public void AddUsageLine(string value)
                {
                        AddLine(this.UsageBuilder, value);
                }

                /// <summary>
                /// Adds a text block with options usage informations.
                /// </summary>
                /// <param name="options">The instance that collected command line arguments parsed with <see cref="CommandLine.Parser"/> class.</param>
                public void AddOptions(object options)
                {
                        AddOptions(options, defaultRequiredWord);
                }

                /// <summary>
                /// Adds a text block with options usage informations.
                /// </summary>
                /// <param name="options">The instance that collected command line arguments parsed with <see cref="CommandLine.Parser"/> class.</param>
                /// <param name="requiredWord">The word to use when the option is required.</param>
                public void AddOptions(object options, string requiredWord)
                {
                        Validator.CheckIsNull(options, "options");
                        Validator.CheckIsNullOrEmpty(requiredWord, "requiredWord");

                        IList<BaseOptionAttribute> optionList =
                                ReflectionUtil.RetrieveFieldAttributeList<BaseOptionAttribute>(options);

                        HelpOptionAttribute optionHelp =
                                ReflectionUtil.RetrieveMethodAttributeOnly<HelpOptionAttribute>(options);
                        if (optionHelp != null)
                        {
                                optionList.Add(optionHelp);
                        }

                        if (optionList.Count == 0)
                        {
                                return;
                        }

                        int maxLength = GetMaxLength(optionList);
                        this.optionsHelp = new StringBuilder(builderCapacity);

                        foreach (BaseOptionAttribute option in optionList)
                        {
                                this.optionsHelp.Append("  ");
                                StringBuilder optionName = new StringBuilder(maxLength);
                                if (option.HasShortName)
                                {
                                        optionName.Append(option.ShortName);
                                        if (option.HasLongName)
                                        {
                                                optionName.Append(", ");
                                        }
                                }
                                if (option.HasLongName)
                                {
                                        optionName.Append(option.LongName);
                                }
                                if (optionName.Length < maxLength)
                                {
                                        this.optionsHelp.Append(optionName.ToString().PadRight(maxLength));
                                }
                                else
                                {
                                        this.optionsHelp.Append(optionName.ToString());
                                }
                                this.optionsHelp.Append("\t");
                                if (option.Required)
                                {
                                        this.optionsHelp.Append(requiredWord);
                                        this.optionsHelp.Append(' ');
                                }
                                this.optionsHelp.Append(option.HelpText);
                                this.optionsHelp.Append(Environment.NewLine);
                        }
                }

                /// <summary>
                /// Returns the help informations as a <see cref="System.String"/>.
                /// </summary>
                /// <returns>The <see cref="System.String"/> that contains the help informations.</returns>
                public override string ToString()
                {
                        const int extraLength = 10;
                        StringBuilder builder = new StringBuilder(this.programName.Length +
                                GetLength(this.version) + GetLength(this.copyright) + GetLength(this.license) +
                                GetLength(this.usage) + GetLength(this.optionsHelp) + extraLength);

                        builder.Append(this.programName);
                        if (!string.IsNullOrEmpty(this.version))
                        {
                                builder.Append(' ');
                                builder.Append(this.version);
                        }
                        if (!string.IsNullOrEmpty(this.copyright))
                        {
                                builder.Append(Environment.NewLine);
                                builder.Append(this.copyright);
                        }
                        if (this.license != null && this.license.Length > 0)
                        {
                                builder.Append(Environment.NewLine);
                                builder.Append(this.license.ToString());
                        }
                        if (this.usage != null && this.usage.Length > 0)
                        {
                                builder.Append(Environment.NewLine);
                                builder.Append(Environment.NewLine);
                                builder.Append(this.usage.ToString());
                        }
                        if (this.optionsHelp != null && this.optionsHelp.Length > 0)
                        {
                                builder.Append(Environment.NewLine);
                                builder.Append(Environment.NewLine);
                                builder.Append(this.optionsHelp.ToString());
                        }

                        return builder.ToString();
                }

                /// <summary>
                /// Converts the help informations to a <see cref="System.String"/>.
                /// </summary>
                /// <param name="info">This <see cref="CommandLine.HelpText"/> instance.</param>
                /// <returns>The <see cref="System.String"/> that contains the help informations.</returns>
                public static implicit operator string(HelpText info)
                {
                        return info.ToString();
                }

                private StringBuilder LicenseBuilder
                {
                        get
                        {
                                if (this.license == null)
                                {
                                        this.license = new StringBuilder(builderCapacity);
                                }
                                return this.license;
                        }
                }

                private StringBuilder UsageBuilder
                {
                        get
                        {
                                if (this.usage == null)
                                {
                                        this.usage = new StringBuilder(builderCapacity);
                                }
                                return this.usage;
                        }
                }

                private static void AddLine(StringBuilder builder, string value)
                {
                        Validator.CheckIsNullOrEmpty(value, "value");

                        if (builder.Length > 0)
                        {
                                builder.Append(Environment.NewLine);
                        }
                        builder.Append(value);
                }

                private static int GetLength(string value)
                {
                        if (value == null)
                        {
                                return 0;
                        }
                        else
                        {
                                return value.Length;
                        }
                }

                private static int GetLength(StringBuilder value)
                {
                        if (value == null)
                        {
                                return 0;
                        }
                        else
                        {
                                return value.Length;
                        }
                }

                private static int GetMaxLength(IList<BaseOptionAttribute> optionList)
                {
                        int length = 0;
                        foreach (BaseOptionAttribute option in optionList)
                        {
                                int optionLenght = 0;
                                bool hasShort = option.HasShortName;
                                bool hasLong = option.HasLongName;
                                if (hasShort)
                                {
                                        optionLenght += option.ShortName.Length;
                                }
                                if (hasLong)
                                {
                                        optionLenght += option.LongName.Length;
                                }
                                if (hasShort && hasLong)
                                {
                                        optionLenght += 2; // ", "
                                }
                                length = Math.Max(length, optionLenght);
                        }
                        return length;
                }
        }
}
