#region Copyright (C) 2005 - 2008 Giacomo Stelluti Scala
//
// Command Line Library: ValueListAttribute.cs
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
        using System.Collections.Generic;
        using System.Reflection;
        
        /// <summary>
        /// Models a list command line arguments that are not options.
        /// Must be applied to a field compatible with a <see cref="System.Collections.Generic.IList&lt;T&gt;"/> interface
        /// of <see cref="System.String"/> instances.
        /// </summary>
        [AttributeUsage(AttributeTargets.Field,
                AllowMultiple = false,
                Inherited = true)]
        public sealed class ValueListAttribute : Attribute
        {
                private Type concreteType;
                
                /// <summary>
                /// Initializes a new instance of the <see cref="CommandLine.ValueListAttribute"/> class.
                /// </summary>
                /// <param name="concreteType"></param>
                public ValueListAttribute(Type concreteType)
                {
                        if (concreteType == null)
                        {
                                throw new ArgumentNullException("concreteType");
                        }
                        if (!typeof(IList<string>).IsAssignableFrom(concreteType))
                        {
                                throw new IncompatibleTypesException();
                        }
                        this.concreteType = concreteType;
                }

                internal Type ConcreteType
                {
                        get { return this.concreteType; }
                }

                internal static IList<string> GetReference(object target)
                {
                        Type concreteType;
                        FieldInfo field = GetField(target, out concreteType);
                        if (field == null)
                        {
                                return null;
                        }
                        field.SetValue(target, Activator.CreateInstance(concreteType));
                        return (IList<string>)field.GetValue(target);
                }

                private static FieldInfo GetField(object target, out Type concreteType)
                {
                        concreteType = null;

                        IList<Pair<FieldInfo, ValueListAttribute>> list =
                                ReflectionUtil.RetrieveFieldList<ValueListAttribute>(target);
                        if (list.Count == 0)
                        {
                                return null;
                        }
                        if (list.Count > 1)
                        {
                                throw new InvalidOperationException();
                        }
                        Pair<FieldInfo, ValueListAttribute> pairZero = list[0];
                        concreteType = pairZero.Right.ConcreteType;
                        return pairZero.Left;
                }
        }
}
