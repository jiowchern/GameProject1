using Regulus.Utility.WindowConsoleAppliction;
using System;
using System.Collections.Generic;
using System.Text;

namespace Regulus.Project.GameProject1.PlayUser
{
    class Program
    {
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="port"></param>
        /// <param name="mode"></param>
        static void Main(string address,int port,string mode)
        {

            var protocol = Regulus.Project.GameProject1.Data.ProtocolCreater.Create();
            using var server = new Regulus.Project.GameProject1.Game.Play.Server();
            var service = Regulus.Remote.Standalone.Provider.CreateService(server, protocol);
            using var agent = service.Create();
            
            var console = new Regulus.Project.GameProject1.PlayUser.Console(agent);
            var binder = new Regulus.Remote.Client.AgentCommandBinder(new Remote.Client.AgentCommandRegister(console.Command, new Remote.Client.TypeConverterSet()), protocol.GetInterfaceProvider().Types);
            var id = binder.Bind(agent);
            console.Run();
            binder.Unbind(id);
        }
    }
}
