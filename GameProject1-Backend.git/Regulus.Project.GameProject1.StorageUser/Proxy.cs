using Regulus.Framework;
using Regulus.Utility;

namespace Regulus.Project.GameProject1.Storage.User
{
	public class Proxy :
		IUpdatable, Console.IViewer, Console.IInput
	{
		private readonly Client<IUser> _Client;

		private readonly Updater _Updater;

		private readonly IUserFactoty<IUser> _UserFactory;

		private UserProvider<IUser> _UserProvider;

		public bool Enable
		{
			get { return this._Client.Enable; }
		}

		public Proxy(IUserFactoty<IUser> custom)
		{
		    this._UserFactory = custom;
		    this._Client = new Client<IUser>(this, new Command());
		    this._Updater = new Updater();

		    this.Client_ModeSelectorEvent(this._Client.Selector);
		}

		

		event Console.OnOutput Console.IInput.OutputEvent
		{
			add { }
			remove { }
		}

		bool IUpdatable.Update()
		{
		    this._Updater.Working();
			return this._Client.Enable;
		}

		void IBootable.Launch()
		{
		    this._Updater.Add(this._Client);
		}

		void IBootable.Shutdown()
		{
		    this._Updater.Shutdown();
		}

		void Console.IViewer.WriteLine(string message)
		{
		}

		void Console.IViewer.Write(string message)
		{
		}

		private void Client_ModeSelectorEvent(GameModeSelector<IUser> selector)
		{
			selector.AddFactoty("fac", this._UserFactory);
		    this._UserProvider = selector.CreateUserProvider("fac");
		}

		public IUser SpawnUser(string name)
		{
			return this._UserProvider.Spawn(name);
		}

		public void UnspawnUser(string name)
		{
		    this._UserProvider.Unspawn(name);
		}
	}
}
