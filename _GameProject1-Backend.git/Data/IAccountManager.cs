using Regulus.Remote;




namespace Regulus.Project.GameProject1.Data
{
	public interface IAccountManager 
	{
        Value<ACCOUNT_REQUEST_RESULT> Create(Account account);
		Value<Account[]> QueryAllAccount();

		Value<ACCOUNT_REQUEST_RESULT> Delete(string account);

		Value<ACCOUNT_REQUEST_RESULT> Update(Account account);
	}
}
