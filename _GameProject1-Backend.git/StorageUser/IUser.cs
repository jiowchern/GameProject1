
using Regulus.Remote;
using Regulus.Utility;



namespace Regulus.Project.GameProject1.Storage.User
{
	public interface IUser : IUpdatable
	{
		Remote.User Remote { get; }

		INotifier<Data.IVerify> VerifyProvider { get; }

		INotifier<Data.IStorageCompetences> StorageCompetencesProvider { get; }

		

		INotifier<T> QueryProvider<T>();
	}
}
