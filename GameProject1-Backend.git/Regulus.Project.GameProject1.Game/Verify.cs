using Regulus.Remote;



namespace Regulus.Project.GameProject1.Game
{
    
    public class Verify : Regulus.Project.GameProject1.Data.IVerify
	{
        
        public delegate void DoneCallback(Regulus.Project.GameProject1.Data.Account account);

		public event DoneCallback OnDoneEvent;

		private readonly Regulus.Project.GameProject1.Data.IAccountFinder _Storage;

		public Verify(Regulus.Project.GameProject1.Data.IAccountFinder storage)
		{
			this._Storage = storage;
		}

		Value<bool> Regulus.Project.GameProject1.Data.IVerify.Login(string id, string password)
		{
			var returnValue = new Value<bool>();
			var val = this._Storage.FindAccountByName(id);
			val.OnValue += account =>
			{
				var found = account != null;
				if(found && account.IsPassword(password))
				{
					if(this.OnDoneEvent != null)
					{
						this.OnDoneEvent(account);
					}

					returnValue.SetValue(true);
				}
				else
				{
					returnValue.SetValue(false);
				}
			};
			return returnValue;
		}
	}
}



