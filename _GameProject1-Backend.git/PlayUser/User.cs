using System;
using System.Reflection;

using Regulus.Framework;
using Regulus.Project.GameProject1.Data;
using Regulus.Remote;
using Regulus.Utility;




namespace Regulus.Project.GameProject1
{
	internal class User : IUser
	{
		private readonly IAgent _Agent;

		private readonly Updater _Updater;

		private readonly Remote.User _User;

		public User(IAgent agent)
		{
			this._Agent = agent;
			
			this._Updater = new Updater();
			this._User = new Remote.User(this._Agent);
		}

		bool IUpdatable.Update()
		{
			
			this._Updater.Working();
			return true;
		}

		void IBootable.Launch()
		{
			this._Updater.Add(this._User);
			_Agent.QueryNotifier<IVersion>().Supply += _VersionSupply;
		}

		void IBootable.Shutdown()
		{
			_Agent.QueryNotifier<IVersion>().Supply -= _VersionSupply;
			this._Updater.Shutdown();
		}

		void _VersionSupply(IVersion version)
		{


			Regulus.Utility.Log.Instance.WriteInfo("server version: " + version.Number);

		}
		
		

		Remote.User IUser.Remote
		{
			get { return this._User; }
		}

		INotifier<IVerify> IUser.VerifyProvider
		{
			get { return this._Agent.QueryNotifier<IVerify>(); }
		}

		INotifier<IVisible> IUser.VisibleProvider
		{
			get { return this._Agent.QueryNotifier<IVisible>(); }
		}

		INotifier<IMoveController> IUser.MoveControllerProvider
		{
			get { return this._Agent.QueryNotifier<IMoveController>(); }
		}

		INotifier<IPlayerProperys> IUser.PlayerProperysProvider { get { return this._Agent.QueryNotifier<IPlayerProperys>(); } }

		INotifier<IAccountStatus> IUser.AccountStatusProvider
		{
			get { return this._Agent.QueryNotifier<IAccountStatus>(); }
		}

		INotifier<IInventoryController> IUser.InventoryControllerProvider
		{
			get { return this._Agent.QueryNotifier<IInventoryController>(); }
		}
		INotifier<IBagNotifier> IUser.BagNotifierProvider
		{
			get { return this._Agent.QueryNotifier<IBagNotifier>(); }
		}

		INotifier<IEquipmentNotifier> IUser.EquipmentNotifierProvider
		{
			get { return this._Agent.QueryNotifier<IEquipmentNotifier>(); }
		}
		INotifier<INormalSkill> IUser.NormalControllerProvider { get { return this._Agent.QueryNotifier<INormalSkill>(); } }

		INotifier<IBattleSkill> IUser.BattleControllerProvider { get { return this._Agent.QueryNotifier<IBattleSkill>(); } }

		INotifier<ICastSkill> IUser.BattleCastControllerProvider { get { return this._Agent.QueryNotifier<ICastSkill>(); } }

		INotifier<IEmotion> IUser.EmotionControllerProvider { get { return this._Agent.QueryNotifier<IEmotion>(); } }
		INotifier<IMakeSkill> IUser.MakeControllerProvider { get { return this._Agent.QueryNotifier<IMakeSkill>(); } }

		INotifier<IDevelopActor> IUser.DevelopActorProvider { get { return this._Agent.QueryNotifier<IDevelopActor>(); } }

		INotifier<IJumpMap> IUser.JumpMapProvider { get { return this._Agent.QueryNotifier<IJumpMap>(); } }
	}
}
