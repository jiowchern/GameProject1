using System;

using Regulus.Remote;
using Regulus.Utility;



namespace Regulus.Project.GameProject1
{
	public interface IUser : IUpdatable
	{
		
		Remote.User Remote { get; }

		INotifier<Data.IVerify> VerifyProvider { get; }

		INotifier<Data.IVisible> VisibleProvider { get; }

		INotifier<Data.IMoveController> MoveControllerProvider { get; }

		INotifier<Data.IPlayerProperys> PlayerProperysProvider { get; }

		INotifier<Data.IAccountStatus> AccountStatusProvider { get; }

		INotifier<Data.IInventoryController> InventoryControllerProvider { get; }
		INotifier<Data.IBagNotifier> BagNotifierProvider { get; }
		INotifier<Data.IEquipmentNotifier> EquipmentNotifierProvider { get; }
		INotifier<Data.INormalSkill> NormalControllerProvider { get; }

		INotifier<Data.IBattleSkill> BattleControllerProvider { get; }

		INotifier<Data.ICastSkill> BattleCastControllerProvider { get; }

		INotifier<Data.IEmotion> EmotionControllerProvider { get; }
		INotifier<Data.IMakeSkill> MakeControllerProvider { get; }

		INotifier<Data.IDevelopActor> DevelopActorProvider { get; }

        INotifier<Data.IJumpMap> JumpMapProvider { get; }




    }
}
