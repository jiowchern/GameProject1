using Regulus.Utility;




namespace Regulus.Project.GameProject1.Storage.User
{
	public class VerifyStorageStage : IStage
	{
		public delegate void DoneCallback(bool result);

		public event DoneCallback OnDoneEvent;

		private readonly string _Account;

		private readonly string _Password;

		private readonly IUser _User;

		public VerifyStorageStage(IUser user, string account, string password)
		{
		    this._Account = account;
		    this._Password = password;
		    this._User = user;
		}

		void IStage.Update()
		{
		}

		void IStage.Leave()
		{
		    this._User.VerifyProvider.Supply -= this._ToVerify;
		}

		void IStage.Enter()
		{
		    this._User.VerifyProvider.Supply += this._ToVerify;
		}

		private void _ToVerify(Data.IVerify obj)
		{
			var result = obj.Login(this._Account, this._Password);
			result.OnValue += val => { this.OnDoneEvent(val); };
		}
	}
}
