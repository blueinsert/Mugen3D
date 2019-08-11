//  This file is part of YamlDotNet - A .NET library for YAML.
//  Copyright (c) Antoine Aubry and contributors

//  Permission is hereby granted, free of charge, to any person obtaining a copy of
//  this software and associated documentation files (the "Software"), to deal in
//  the Software without restriction, including without limitation the rights to
//  use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
//  of the Software, and to permit persons to whom the Software is furnished to do
//  so, subject to the following conditions:

//  The above copyright notice and this permission notice shall be included in all
//  copies or substantial portions of the Software.

//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//  SOFTWARE.

using YamlDotNet.Core;
using System;

namespace YamlDotNet.Serialization.Converters
{
    /// <summary>
    /// Converter for MetalSlug.Core.Number
    /// </summary>
    public class NumberConverter : IYamlTypeConverter
    {

        public NumberConverter()
        {
          
        }

        public bool Accepts(System.Type type)
        {
            return type == typeof(FixPointMath.Number);
        }

        public object ReadYaml(Core.IParser parser, System.Type type)
        {
            var value = ((Core.Events.Scalar)parser.Current).Value;
            parser.MoveNext();
            if (value.Contains(".")) {
                int n = value.Length - value.IndexOf(".") - 1;
                FixPointMath.Number result = int.Parse(value.Replace(".", ""));
                for (int i = 0; i < n ; i++) {
                    result = result / 10;
                }     
                return result;
            }
            return (FixPointMath.Number)int.Parse(value);
        }

        public void WriteYaml(Core.IEmitter emitter, object value, System.Type type)
        {
            var v = (FixPointMath.Number)value;
            emitter.Emit(new Core.Events.Scalar(null, null, v.ToString(),  ScalarStyle.Any, true, false));
        }
    }
}
