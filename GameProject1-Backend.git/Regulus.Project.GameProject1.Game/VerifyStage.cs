using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Regulus.Project.GameProject1.Game
{
	class VerifyStage : Utility.IStatus
	{
		public event Verify.DoneCallback DoneEvent;

		private readonly Remote.IBinder _Binder;

		private readonly Verify _Verify;

		Play.UnbindHelper _UnbindHelper;

	    
		public VerifyStage(Remote.IBinder binder, Verify verify)
		{
		    this._Verify = verify;
		    this._Binder = binder;
			_UnbindHelper = new Play.UnbindHelper(binder);
		}

		void Utility.IStatus.Enter()
		{
		    this._Verify.OnDoneEvent += this.DoneEvent;

			_UnbindHelper += this._Binder.Bind<Regulus.Project.GameProject1.Data.IVerify>(this._Verify);
		}

		void Utility.IStatus.Leave()
		{
			_UnbindHelper.Release();

			this._Verify.OnDoneEvent -= this.DoneEvent;
		}

		void Utility.IStatus.Update()
		{
		}
	}
}
