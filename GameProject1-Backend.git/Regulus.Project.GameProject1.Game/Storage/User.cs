using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Regulus.Framework;

using Regulus.Project.GameProject1.Data;
using Regulus.Remote;
using Regulus.Utility;

namespace Regulus.Project.GameProject1.Game.Storage
{
    internal class User : Regulus.Game.IUser
    {
        private event Regulus.Game.OnQuit _QuitEvent;

        private event Regulus.Game.OnNewUser _VerifySuccessEvent;

        private readonly IBinder _Binder;

        private readonly StageMachine _Machine;

        private readonly IStorage _Storage;

        private Account _Account;

        public User(IBinder binder, IStorage storage)
        {
            this._Storage = storage;
            this._Binder = binder;
            this._Machine = new StageMachine();
        }

        void Regulus.Game.IUser.OnKick(Guid id)
        {
        }

        event Regulus.Game.OnNewUser Regulus.Game.IUser.VerifySuccessEvent
        {
            add { this._VerifySuccessEvent += value; }
            remove { this._VerifySuccessEvent -= value; }
        }

        event Regulus.Game.OnQuit Regulus.Game.IUser.QuitEvent
        {
            add { this._QuitEvent += value; }
            remove { this._QuitEvent -= value; }
        }

        bool IUpdatable.Update()
        {
            this._Machine.Update();
            return true;
        }

        void IBootable.Launch()
        {
            this._ToVerify();
        }

        void IBootable.Shutdown()
        {
            this._Machine.Termination();
        }

        private void _ToVerify()
        {
            var verify = this._CreateVerify();
            this._AddVerifyToStage(verify);
        }

        private Verify _CreateVerify()
        {
            this._Account = null;
            var verify = new Verify(this._Storage);
            return verify;
        }

        private void _AddVerifyToStage(Verify verify)
        {
            var stage = new VerifyStage(this._Binder, verify);
            stage.DoneEvent += this._VerifySuccess;
            this._Machine.Push(stage);
        }

        private void _VerifySuccess(Account account)
        {
            this._VerifySuccessEvent(account.Id);
            this._Account = account;
            this._ToRelease(account);
        }

        private void _ToRelease(Account account)
        {
            var stage = new StroageAccess(this._Binder, account, this._Storage);
            stage.DoneEvent += this._ToVerify;
            this._Machine.Push(stage);
        }
    }
}
