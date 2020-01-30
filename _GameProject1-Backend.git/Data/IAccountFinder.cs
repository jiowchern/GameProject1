using System;


using Regulus.Remote;

namespace Regulus.Project.GameProject1.Data
{
	public interface IAccountFinder
	{
		Value<Account> FindAccountByName(string id);

		Value<Account> FindAccountById(Guid accountId);
	}
}
