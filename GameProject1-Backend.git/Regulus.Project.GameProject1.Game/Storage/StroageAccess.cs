using System;
using System.Linq;
using Regulus.Project.GameProject1.Data;
using Regulus.Remote;
using Regulus.Utility;

namespace Regulus.Project.GameProject1.Game.Storage
{
    public class StroageAccess : IStatus, IQuitable, IStorageCompetences
    {
        public delegate void DoneCallback();

        public event DoneCallback DoneEvent;

        private readonly Account _Account;

        private readonly IBinder _Binder;

        private readonly IStorage _Storage;

        
        readonly System.Collections.Generic.Dictionary<System.Guid, Play.UnbindHelper> _UnbindHelper;

        public StroageAccess(IBinder binder, Account account, IStorage storage)
        {
            this._Binder = binder;
            this._Account = account;
            this._Storage = storage;
            _UnbindHelper = new System.Collections.Generic.Dictionary<System.Guid, Play.UnbindHelper>();
        }

        void IQuitable.Quit()
        {
            this.DoneEvent();
        }

        void IStatus.Enter()
        {
            this._Attach(this._Account);
        }

        void IStatus.Leave()
        {
            this._Detach(this._Account);
        }

        void IStatus.Update()
        {
        }

        Value<Account.COMPETENCE[]> IStorageCompetences.Query()
        {
            return this._Account.Competnces.ToArray();
        }

        Value<Guid> IStorageCompetences.QueryForId()
        {
            return this._Account.Id;
        }

        private void _Attach(Account account)
        {
            var binder = new Play.UnbindHelper(_Binder);
            _UnbindHelper.Add(account.Id , binder);
            binder += this._Binder.Bind<IStorageCompetences>(this);

            if (account.HasCompetnce(Account.COMPETENCE.ACCOUNT_FINDER))
            {
                binder += this._Binder.Bind<IAccountFinder>(this._Storage);
                binder += this._Binder.Bind<IGameRecorder>(this._Storage);
            }

            if (account.HasCompetnce(Account.COMPETENCE.ACCOUNT_MANAGER))
            {
                binder += this._Binder.Bind<IAccountManager>(this._Storage);
            }

        }

        private void _Detach(Account account)
        {
            _UnbindHelper.TryGetValue(account.Id , out var binder);
            _UnbindHelper.Remove(account.Id);
            binder.Release();



        }
    }
}