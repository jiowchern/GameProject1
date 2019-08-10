﻿using System;

namespace Regulus.Serialization
{
    public class ByteArrayDescriber : ITypeDescriber
    {
        
        private readonly ITypeDescriber _IntTypeDescriber;

        public ByteArrayDescriber(ITypeDescriber byte_describer)
        {
            _IntTypeDescriber = byte_describer;
        }

        
        Type ITypeDescriber.Type
        {
            get { return typeof(byte[]); }
        }

        object ITypeDescriber.Default
        {
            get { return null; }
        }

        int ITypeDescriber.GetByteCount(object instance)
        {
            var array = instance as Array;
            var len = array.Length;
            var lenByetCount = _IntTypeDescriber.GetByteCount(len);
            return len + lenByetCount;
        }

        int ITypeDescriber.ToBuffer(object instance, byte[] buffer, int begin)
        {
            var array = instance as byte[];
            var len = array.Length;

            var offset = begin;

            
            offset += _IntTypeDescriber.ToBuffer(len, buffer, offset);
            for (int i = 0; i < len; i++)
            {
                buffer[offset++] = array[i];
            }
            return offset - begin;
        }

        int ITypeDescriber.ToObject(byte[] buffer, int begin, out object instnace)
        {
            int offset = begin;
            object lenObject = null;            
            offset += _IntTypeDescriber.ToObject(buffer, offset, out lenObject);

            var len = (int)lenObject;
            var array = new byte[len];
            for (int i = 0; i < len; i++)
            {
                array[i] = buffer[offset++];
            }
            instnace = array;
            return offset - begin; 
        }

        
    }
}