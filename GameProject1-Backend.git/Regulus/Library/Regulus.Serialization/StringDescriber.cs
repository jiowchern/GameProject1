using System;
using System.ComponentModel;
using System.Linq;

namespace Regulus.Serialization
{
    public class StringDescriber : ITypeDescriber
    {
        

        private readonly ITypeDescriber _CharArrayDescriber;

        public StringDescriber(ITypeDescriber chars_describer)
        {
            _CharArrayDescriber = chars_describer;

        }

        
        Type ITypeDescriber.Type
        {
            get { return typeof (string); }
        }

        object ITypeDescriber.Default
        {
            get { return null; }
        }

        int ITypeDescriber.GetByteCount(object instance)
        {
            var str = instance as string;
            var chars = str.ToCharArray();

            var charCount = _CharArrayDescriber.GetByteCount(chars);

            return charCount;
        }

        int ITypeDescriber.ToBuffer(object instance, byte[] buffer, int begin)
        {
            var str = instance as string;
            var chars = str.ToCharArray();
            int offset = begin;            
            offset += _CharArrayDescriber.ToBuffer(chars, buffer, offset);
            return offset - begin;
        }

        int ITypeDescriber.ToObject(byte[] buffer, int begin, out object instnace)
        {
            int offset = begin;
            object chars;
            offset += _CharArrayDescriber.ToObject(buffer, offset, out chars);

            instnace = new string(chars as char[]);

            return offset - begin;
        }
        
    }
}