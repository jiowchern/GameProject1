using System;


using Regulus.Remote;




namespace Regulus.Project.GameProject1.Data
{
	public interface IStorageCompetences
	{
		Value<Account.COMPETENCE[]> Query();

		Value<Guid> QueryForId();
	}
}
