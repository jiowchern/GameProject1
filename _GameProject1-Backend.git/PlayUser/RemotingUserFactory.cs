using Regulus.Framework;
using Regulus.Network;
using Regulus.Remote;
using Regulus.Utility;


namespace Regulus.Project.GameProject1
{
	public class RemoteUserFactory : IUserFactoty<IUser>
	{
        private readonly System.Reflection.Assembly _ProtocolAssembly;

        public RemoteUserFactory(System.Reflection.Assembly assembly)
	    {
            this._ProtocolAssembly = assembly;
        }

	    IUser IUserFactoty<IUser>.SpawnUser()
		{



            return new User(Regulus.Remote.Client.AgentProivder.CreateTcp(_ProtocolAssembly));
        }

		ICommandParsable<IUser> IUserFactoty<IUser>.SpawnParser(Command command, Console.IViewer view, IUser user)
		{
			return new CommandParser(command, view, user);
		}
	}
}
