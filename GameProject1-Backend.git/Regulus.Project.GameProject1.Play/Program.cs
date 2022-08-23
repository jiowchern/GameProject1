using Regulus.Utility.WindowConsoleAppliction;

namespace Regulus.Project.GameProject1.Play
{
    class Program
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tcpport"></param>        
        static void Main(int tcpport)
        {
            var protocol = Regulus.Project.GameProject1.Data.ProtocolCreater.Create();
            var listener = new Regulus.Remote.Server.Tcp.Listener();
            using var server = new Regulus.Project.GameProject1.Play.Server();

            using var service = Regulus.Remote.Server.Provider.CreateService(server, protocol, listener);
            listener.Bind(tcpport);

            new Console().Run();
        }
    }
}
