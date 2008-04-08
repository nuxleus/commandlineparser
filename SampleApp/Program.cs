#region Copyright (C) 2005 - 2008 Giacomo Stelluti Scala
//
// Command Line Library: CopyrightInfoFixture.cs
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

namespace SampleApp
{
        using System;
        using System.Collections.Generic;
        using CommandLine;

        internal sealed class Program
        {
                private sealed class Options
                {
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
                                HelpText = "Verbose level. Ranges (from 0 to 2).")]
                        public int VerboseLevel = 0;

                        [Option("i", null,
                               HelpText = "If file has errors don't stop processing.")]
                        public bool IgnoreErrors = false;

                        [Option("j", "jump",
                                HelpText = "Data processing start offset.")]
                        public double StartOffset = 0;

                        [ValueList(typeof(List<string>))]
                        public IList<string> DefinitionFiles = null;

                        [HelpOption]
                        public string GetUsage()
                        {
                                HelpText info = new HelpText("CommandLine Library SampleApp", "1.2.1");
                                info.Copyright = new CopyrightInfo("Giacomo Stelluti Scala", 2005, 2007);
                                info.AddLicenseLine("This is free software. You may redistribute copies of it under the terms of");
                                info.AddLicenseLine("the MIT License <http://www.opensource.org/licenses/mit-license.php>.");
                                info.AddUsageLine("Usage: SampleApp -rMyData.in -wMyData.out --calculate");
                                info.AddUsageLine(string.Format("       SampleApp -rMyData.in -i -j{0} file0.def file1.def", 9.7));
                                info.AddOptions(this);
                                return info;
                        }
                }

                private static void Main(string[] args)
                {
                        Options options = new Options();
                        if (Parser.ParseArguments(args, options, Console.Out))
                        {
                                Console.WriteLine("Verbose Level: {0}", (options.VerboseLevel < 0 || options.VerboseLevel > 2) ? "#invalid value#" : options.VerboseLevel.ToString());
                                Console.WriteLine();
                                Console.WriteLine("Reading input file: {0} ...", options.InputFile);
                                foreach (string defFile in options.DefinitionFiles)
                                        Console.WriteLine("  using definition file: {0}", defFile);
                                Console.WriteLine("  start offset: {0}", options.StartOffset);
                                Console.WriteLine("  tabular data computation: {0}", options.Calculate.ToString().ToLowerInvariant());
                                Console.WriteLine("  on errors: {0}", options.IgnoreErrors ? "continue" : "stop processing");
                                Console.WriteLine();
                                if (options.OutputFile.Length > 0)
                                {
                                        Console.WriteLine("Writing elaborated data: {0} ...", options.OutputFile);
                                }
                                else
                                {
                                        Console.WriteLine("Elaborated data:");
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
