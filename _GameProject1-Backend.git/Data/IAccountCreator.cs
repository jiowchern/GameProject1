using Regulus.Remote;




namespace Regulus.Project.GameProject1.Data
{
	public interface IAccountCreator
	{
		Value<ACCOUNT_REQUEST_RESULT> Create(Account account);
	}
}
