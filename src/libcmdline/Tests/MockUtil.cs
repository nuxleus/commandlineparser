﻿#region Copyright (C) 2005 - 2009 Giacomo Stelluti Scala
//
// Command Line Library: MockUtil.cs
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
    using System;
    using System.Reflection;
    using System.Text;
    using System.Collections.Generic;

    static class MockUtil
    {
        public static string ConvertOptionsToString(object instance)
        {
            StringBuilder builder = new StringBuilder(256);
            Type type = instance.GetType();
            FieldInfo[] fields = type.GetFields();            
            foreach (FieldInfo field in fields)
            {
                object[] attrs = field.GetCustomAttributes(false);
                if (attrs.Length > 0)
                {
                    object attr = attrs[0];
                    AppendBaseOptionAttribute(builder, instance, field, attr);
                    AppendValueListAttribute(builder, instance, field, attr);
                }
            }
            return builder.ToString();
        }

        private static void AppendBaseOptionAttribute(StringBuilder builder, object instance, FieldInfo field, object attr)
        {
            BaseOptionAttribute baseOA = attr as BaseOptionAttribute;
            if (baseOA != null)
            {
                if (baseOA.HasShortName)
                {
                    builder.Append(baseOA.ShortName);
                    if (baseOA.HasLongName)
                    {
                        builder.Append("/");
                    }
                }
                if (baseOA.HasLongName)
                {
                    builder.Append(baseOA.LongName);
                }
                builder.Append(": ");
                builder.Append(field.GetValue(instance));
                builder.Append(Environment.NewLine);
            }
        }

        private static void AppendValueListAttribute(StringBuilder builder, object instance, FieldInfo field, object attr)
        {
            ValueListAttribute valueList = attr as ValueListAttribute;
            if (valueList != null)
            {
                IList<string> values = (IList<string>)field.GetValue(instance); //ValueListAttribute.GetReference(instance);
                foreach (string value in values)
                {
                    builder.Append("non-option value: ");
                    builder.Append(value);
                    builder.Append(Environment.NewLine);
                }                
            }
        }
    }
}
#endif
