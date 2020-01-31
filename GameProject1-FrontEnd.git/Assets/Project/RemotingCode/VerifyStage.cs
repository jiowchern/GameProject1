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


	    
		public VerifyStage(Remote.IBinder binder, Verify verify)
		{
		    this._Verify = verify;
		    this._Binder = binder;
		}

		void Utility.IStatus.Enter()
		{
		    this._Verify.OnDoneEvent += this.DoneEvent;

		    this._Binder.Bind<Regulus.Project.GameProject1.Data.IVerify>(this._Verify);
		}

		void Utility.IStatus.Leave()
		{
		    this._Binder.Unbind<Regulus.Project.GameProject1.Data.IVerify>(this._Verify);
		    this._Verify.OnDoneEvent -= this.DoneEvent;
		}

		void Utility.IStatus.Update()
		{
		}
	}
}
