#region Copyright (C) 2005 - 2009 Giacomo Stelluti Scala
//
// Command Line Library: Program.cs
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

namespace SampleApp
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using CommandLine;
    using CommandLine.Text;

    internal sealed class Program
    {
        private static readonly HeadingInfo headingInfo = new HeadingInfo("sampleapp", "1.2.5");

        private enum OptimizeFor
        {
            Unspecified,
            Speed,
            Accuracy
        }

        private sealed class Options
        {
            #region Standard Option Attribute
            [Option("r", "read",
                    Required = true,
                    HelpText = "Input file with data to process.")]
            public string InputFile = String.Empty;

            [Option("w", "write",
                    HelpText = "Output file with processed data (otherwise standard output).")]
            public string OutputFile = String.Empty;

            [Option(null, "calculate",
                    HelpText = "Add results in bottom of tabular data.")]
            public bool Calculate = false;

            [Option("v", null,
                    HelpText = "Verbose level. Range: from 0 to 2.")]
            public int VerboseLevel = 0;

            [Option("i", null,
                   HelpText = "If file has errors don't stop processing.")]
            public bool IgnoreErrors = false;

            [Option("j", "jump",
                    HelpText = "Data processing start offset.")]
            public double StartOffset = 0;

            [Option(null, "optimize",
                HelpText = "Optimize for Speed|Accuracy.")]
            public OptimizeFor Optimization = OptimizeFor.Unspecified;
            #endregion

            #region Specialized Option Attribute
            [ValueList(typeof(List<string>))]
            public IList<string> DefinitionFiles = null;

            [OptionList("o", "operators", Separator=';',
                HelpText = "Operators included in processing (+, -, ...).")]
            public IList<string> AllowedOperators = null;
            
            [HelpOption(
                    HelpText = "Dispaly this help screen.")]
            public string GetUsage()
            {
                HelpText help = new HelpText(Program.headingInfo);
                help.Copyright = new CopyrightInfo("Giacomo Stelluti Scala", 2005, 2007);
                help.AddPreOptionsLine("This is free software. You may redistribute copies of it under the terms of");
                help.AddPreOptionsLine("the MIT License <http://www.opensource.org/licenses/mit-license.php>.");
                help.AddPreOptionsLine("Usage: SampleApp -rMyData.in -wMyData.out --calculate");
                help.AddPreOptionsLine(string.Format("       SampleApp -rMyData.in -i -j{0} file0.def file1.def", 9.7));
                help.AddPreOptionsLine("       SampleApp -rMath.xml -wReport.bin -o *;/;+;-");
                help.AddOptions(this);
                return help;
            }
            #endregion
        }

        private static void Main(string[] args)
        {
            Options options = new Options();
            ICommandLineParser parser = new CommandLineParser();
            if (parser.ParseArguments(args, options, Console.Error))
            {
                Console.WriteLine("Verbose Level: {0}", (options.VerboseLevel < 0 || options.VerboseLevel > 2) ? "#invalid value#" : options.VerboseLevel.ToString());
                Console.WriteLine();
                Console.WriteLine("Reading input file: {0} ...", options.InputFile);
                foreach (string defFile in options.DefinitionFiles)
                {
                    Console.WriteLine("  using definition file: {0}", defFile);
                }
                Console.WriteLine("  start offset: {0}", options.StartOffset);
                Console.WriteLine("  tabular data computation: {0}", options.Calculate.ToString().ToLowerInvariant());
                Console.WriteLine("  on errors: {0}", options.IgnoreErrors ? "continue" : "stop processing");
                Console.WriteLine("  optimize for: {0}", options.Optimization);
                if (options.AllowedOperators != null)
                {
                    StringBuilder builder = new StringBuilder();
                    builder.Append("  allowed operators: ");
                    foreach (string op in options.AllowedOperators)
                    {
                        builder.Append(op);
                        builder.Append(", ");
                    }
                    Console.WriteLine(builder.Remove(builder.Length - 2, 2).ToString());
                }
                Console.WriteLine();
                if (options.OutputFile.Length > 0)
                {
                    headingInfo.WriteMessage(string.Format("Writing elaborated data: {0} ...", options.OutputFile));
                }
                else
                {
                    headingInfo.WriteMessage("Elaborated data:");
                    Console.WriteLine("[...]");
                }
                Environment.Exit(0);
            }
            else
            {
                Environment.Exit(1);
            }
        }
    }
}
