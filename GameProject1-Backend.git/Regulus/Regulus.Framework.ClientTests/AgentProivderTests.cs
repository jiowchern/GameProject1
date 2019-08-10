﻿

using Regulus.Remote;
using Regulus.Serialization;

namespace Regulus.Framework.Client.JIT.Tests
{
    
    public class Test
    {

    }

    public class TestProtocol : Regulus.Remote.IProtocol
    {
        byte[] IProtocol.VerificationCode => throw new System.NotImplementedException();

        EventProvider IProtocol.GetEventProvider()
        {
            throw new System.NotImplementedException();
        }

        InterfaceProvider IProtocol.GetInterfaceProvider()
        {
            throw new System.NotImplementedException();
        }

        MemberMap IProtocol.GetMemberMap()
        {
            throw new System.NotImplementedException();
        }

        ISerializer IProtocol.GetSerialize()
        {
            throw new System.NotImplementedException();
        }
    }

    public class AgentProivderTests
    {
        [NUnit.Framework.Test()]
        public void CreateProtocol()
        {            
            var type = typeof(TestProtocol);
            
            var protocol = Regulus.Remote.Protocol.ProtocolProvider.Create(type.Assembly);
            NUnit.Framework.Assert.AreNotEqual(protocol , null);
        }

        [NUnit.Framework.Test()]
        public void CreateRudpTest()
        {
            Regulus.Remote.Client.JIT.AgentProivder.CreateRudp(typeof(Test));            
        }

        [NUnit.Framework.Test()]
        public void CreateTcpTest()
        {            
            Regulus.Remote.Client.JIT.AgentProivder.CreateTcp(typeof(Test));
        }
    }
}