using Regulus.Remote;
using Regulus.Utility;

namespace Regulus.Project.GameProject1.Storage.User
{
	public class ConnectStorageStage : IStatus
	{
		public delegate void DoneCallback(bool result);

		public event DoneCallback OnDoneEvent;

		private readonly string _IpAddress;

		private readonly int _Port;

		private readonly IUser _User;

		public ConnectStorageStage(IUser user, string ipaddress, int port)
		{
			// TODO: Complete member initialization
		    this._User = user;
		    this._IpAddress = ipaddress;
		    this._Port = port;
		}

		void IStatus.Enter()
		{
		    this._User.Remote.ConnectProvider.Supply += this._Connect;
		}

		void IStatus.Leave()
		{
		    this._User.Remote.ConnectProvider.Supply -= this._Connect;
		}

		void IStatus.Update()
		{
		}

		private void _Connect(IConnect obj)
		{
            var address = System.Net.IPAddress.Parse(this._IpAddress);
            
            var result = obj.Connect(new System.Net.IPEndPoint(address,_Port));
			result.OnValue += val => { this.OnDoneEvent(val); };
		}
	}
}
