﻿using System.Linq;

namespace Regulus.Remote.Protocol
{
    public static class ProtocolProvider
    {
        public static Regulus.Remote.IProtocol Create(System.Reflection.Assembly protocol_assembly)
        {
            var types = protocol_assembly.GetExportedTypes();
            var protocolType = types.Where(type => type.GetInterface(nameof(Regulus.Remote.IProtocol)) != null).FirstOrDefault();
            if (protocolType == null)
                throw new System.Exception($"找不到{nameof(Regulus.Remote.IProtocol)}的實作");
            return System.Activator.CreateInstance(protocolType) as Regulus.Remote.IProtocol;
        }
    }
}
