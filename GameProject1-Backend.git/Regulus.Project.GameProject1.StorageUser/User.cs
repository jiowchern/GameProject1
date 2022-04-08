// --------------------------------------------------------------------------------------------------------------------
// <copyright file="User.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the User type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


using Regulus.Project.GameProject1.Data;
using Regulus.Remote;
using Regulus.Utility;



namespace Regulus.Project.GameProject1.Storage.User
{
	internal class User : IUser
	{
		private readonly IAgent _Agent;

		private readonly Remote.User _Remote;

		private readonly Updater _Updater;

		public User(IAgent agent)
		{
		    this._Agent = agent;
		    this._Updater = new Updater();
		    this._Remote = new Remote.User(agent);
		}

		Remote.User IUser.Remote
		{
			get { return this._Remote; }
		}

		INotifier<IVerify> IUser.VerifyProvider
		{
			get { return this._Agent.QueryNotifier<IVerify>(); }
		}

		bool IUpdatable.Update()
		{
		    this._Updater.Working();
			return true;
		}

		void IBootable.Launch()
		{
		    this._Updater.Add(this._Agent);
		    this._Updater.Add(this._Remote);
		}

		void IBootable.Shutdown()
		{
		    this._Updater.Shutdown();
		}

		INotifier<T> IUser.QueryProvider<T>()
		{
			return this._Agent.QueryNotifier<T>();
		}

		INotifier<IStorageCompetences> IUser.StorageCompetencesProvider
		{
			get { return this._Agent.QueryNotifier<IStorageCompetences>(); }
		}
	}
}
