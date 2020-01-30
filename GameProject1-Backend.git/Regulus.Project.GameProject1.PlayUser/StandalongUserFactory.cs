// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandaloneUserFactory.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the StandaloneUserFactory type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Regulus.Framework;

using Regulus.Remote;
using Regulus.Remote.Standalone;
using Regulus.Utility;

namespace Regulus.Project.GameProject1
{
	public class StandaloneUserFactory
		: IUserFactoty<IUser>
	{
		private readonly IEntry _Standalone;


        private readonly System.Reflection.Assembly _ProtocolAssembly;

        public StandaloneUserFactory(IEntry entry,System.Reflection.Assembly assembly)

        {
            _Standalone = entry;
            this._ProtocolAssembly = assembly;
        }
        

	    IUser IUserFactoty<IUser>.SpawnUser()
		{

            var protocol = Regulus.Remote.Protocol.ProtocolProvider.Create(_ProtocolAssembly);
            var agent = new Agent(protocol);
            (agent as IAgent).ConnectEvent += () => { this._Standalone.AssignBinder(agent); };

            return new User(agent);

        }

		ICommandParsable<IUser> IUserFactoty<IUser>.SpawnParser(Command command, Console.IViewer view, IUser user)
		{
			return new CommandParser(command, view, user);
		}
	}
}
