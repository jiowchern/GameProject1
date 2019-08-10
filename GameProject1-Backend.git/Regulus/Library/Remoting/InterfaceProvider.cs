using System;
using System.Collections.Generic;

namespace Regulus.Remote
{
    /// <summary>
    /// 
    /// </summary>
    public class InterfaceProvider 
    {
        private readonly Dictionary<Type, Type> _Types;

        public InterfaceProvider(Dictionary<Type, Type> types)
        {
            _Types = types;
        }
        public Type Find(Type ghost_base_type)
        {
            if (_Types.ContainsKey(ghost_base_type))
            {                
                return _Types[ghost_base_type];              
            }
            return null;
        }
    }
}