


using Regulus.Remote;
using Regulus.Utility;

namespace Regulus.Project.GameProject1.Storage.User
{
	internal class CommandParser : ICommandParsable<IUser>
	{
		private Command command;

		private IUser user;

		private Console.IViewer view;

		public CommandParser(Command command, Console.IViewer view, IUser user)
		{
		
			this.command = command;
			this.view = view;
			this.user = user;
		}

		void ICommandParsable<IUser>.Setup(IGPIBinderFactory build)
		{
		}

		void ICommandParsable<IUser>.Clear()
		{
		}
	}
}
