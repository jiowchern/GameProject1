using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Regulus.Project.GameProject1.Game
{
	class VerifyStage : Utility.IStage
	{
		public event Verify.DoneCallback DoneEvent;

		private readonly Remote.IBinder _Binder;

		private readonly Verify _Verify;


	    
		public VerifyStage(Remote.IBinder binder, Verify verify)
		{
		    this._Verify = verify;
		    this._Binder = binder;
		}

		void Utility.IStage.Enter()
		{
		    this._Verify.OnDoneEvent += this.DoneEvent;

		    this._Binder.Bind<Regulus.Project.GameProject1.Data.IVerify>(this._Verify);
		}

		void Utility.IStage.Leave()
		{
		    this._Binder.Unbind<Regulus.Project.GameProject1.Data.IVerify>(this._Verify);
		    this._Verify.OnDoneEvent -= this.DoneEvent;
		}

		void Utility.IStage.Update()
		{
		}
	}
}
