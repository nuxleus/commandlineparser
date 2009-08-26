#region Copyright (C) 2005 - 2009 Giacomo Stelluti Scala
//
// Command Line Library: OptionMap.cs
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
    using System;
    using System.Collections.Generic;

    sealed class OptionMap : IOptionMap
    {
        private CommandLineParserSettings settings;
        private Dictionary<string, string> names;
        private Dictionary<string, OptionInfo> map;
        private Dictionary<string, int> mutuallyExclusiveSetMap;

        public OptionMap(int capacity, CommandLineParserSettings settings) //public OptionMap(int capacity, bool caseSensitive)
        {
            this.settings = settings;
            IEqualityComparer<string> comparer;
            if (this.settings.CaseSensitive)
            {
                comparer = StringComparer.Ordinal;
            }
            else
            {
                comparer = StringComparer.OrdinalIgnoreCase;
            }
            this.names = new Dictionary<string, string>(capacity, comparer);
            this.map = new Dictionary<string, OptionInfo>(capacity * 2, comparer);
            if (this.settings.MutuallyExclusive)
            {
                this.mutuallyExclusiveSetMap = new Dictionary<string, int>(capacity, StringComparer.OrdinalIgnoreCase);
            }
        }

        public OptionInfo this[string key]
        {
            get
            {
                OptionInfo option = null;
                if (this.map.ContainsKey(key))
                {
                    option = this.map[key];
                }
                else
                {
                    string optionKey = null;
                    if (this.names.ContainsKey(key))
                    {
                        optionKey = this.names[key];
                        option = this.map[optionKey];
                    }
                }
                return option;
            }
            set
            {
                this.map[key] = value;
                if (value.HasBothNames)
                {
                    this.names[value.LongName] = value.ShortName;
                }
            }
        }

        public bool EnforceRules()
        {
            //foreach (OptionInfo option in this.map.Values)
            //{
            //    if (option.Required && !option.IsDefined)
            //    {
            //        return false;
            //    }
            //}
            //return true;
            return EnforceMutuallyExclusiveMap() && EnforceRequiredRule();
        }

        private bool EnforceRequiredRule()
        {
            foreach (OptionInfo option in this.map.Values)
            {
                if (option.Required && !option.IsDefined)
                {
                    return false;
                }
            }
            return true;
        }

        private bool EnforceMutuallyExclusiveMap()
        {
            if (!this.settings.MutuallyExclusive)
            {
                return true;
            }
            foreach (OptionInfo option in this.map.Values)
            {
                if (option.IsDefined && option.MutuallyExclusiveSet != null)
                {
                    BuildMutuallyExclusiveMap(option);
                }
            }
            foreach (int occurrence in this.mutuallyExclusiveSetMap.Values)
            {
                if (occurrence > 1)
                {
                    return false;
                }
            }
            return true;
        }

        private void BuildMutuallyExclusiveMap(OptionInfo option)
        {
            string setName = option.MutuallyExclusiveSet;
            if (!this.mutuallyExclusiveSetMap.ContainsKey(setName))
            {
                this.mutuallyExclusiveSetMap.Add(setName, 0);
            }
            this.mutuallyExclusiveSetMap[setName]++;
        }
    }
}
