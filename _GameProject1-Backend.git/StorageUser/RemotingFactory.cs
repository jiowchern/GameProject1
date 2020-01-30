using Regulus.Framework;
using Regulus.Network;
using Regulus.Remote;
using Regulus.Utility;


namespace Regulus.Project.GameProject1.Storage.User
{
	public class RemoteFactory : IUserFactoty<IUser>
	{
	    
	    private readonly IConnectProviderable _Client;

	    public RemoteFactory(Regulus.Network.IConnectProviderable client)
	    {
	        
	        _Client = client;
	    }

	    IUser IUserFactoty<IUser>.SpawnUser()
		{
            var types = new System.Collections.Generic.List<System.Type>();
            string dataNamesapce = "Regulus.Project.GameProject1.Data";
            foreach(var assembly in System.AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach(var type in assembly.GetExportedTypes())
                {
                    if(type.Namespace == dataNamesapce)
                    {
                        types.Add(type);
                    }
                }
            }
            

            return new User(Regulus.Remote.Client.JIT.AgentProivder.CreateTcp(types.ToArray()));
		}

		ICommandParsable<IUser> IUserFactoty<IUser>.SpawnParser(Command command, Console.IViewer view, IUser user)
		{
			return new CommandParser(command, view, user);
		}
	}
}
