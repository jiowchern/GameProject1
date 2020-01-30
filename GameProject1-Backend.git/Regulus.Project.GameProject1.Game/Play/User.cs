using System;
using Regulus.Framework;

using Regulus.Project.GameProject1.Data;
using Regulus.Remote;
using Regulus.Utility;

namespace Regulus.Project.GameProject1.Game.Play
{
    public class User : Regulus.Game.IUser, IAccountStatus , IVersion
    {
        private event Action _KickEvent;

        private event Regulus.Game.OnQuit _QuitEvent;

        private event Regulus.Game.OnNewUser _VerifySuccessEvent;

        private readonly IAccountFinder _AccountFinder;

        private readonly IBinder _Binder;

        

        private readonly StageMachine _Machine;

        private readonly IGameRecorder _GameRecorder;

        private readonly Zone _Zone;

        private Account _Account;

        private GamePlayerRecord _GamePlayerRecord;

        private string _Version;
        public User(IBinder binder, IAccountFinder account_finder, IGameRecorder game_record_handler, Zone zone)
        {
            this._Machine = new StageMachine();

            this._Binder = binder;
            this._AccountFinder = account_finder;
            this._GameRecorder = game_record_handler;
            this._Zone = zone;
            _Version = "0.0.0.0";
        }

        event Action IAccountStatus.KickEvent
        {
            add { this._KickEvent += value; }
            remove { this._KickEvent -= value; }
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

        void Regulus.Game.IUser.OnKick(Guid id)
        {
            if (this._Account != null && this._Account.Id == id)
            {
                if (this._KickEvent != null)
                {
                    this._KickEvent();
                }

                this._ToVerify();
            }
        }

        bool IUpdatable.Update()
        {
            this._Machine.Update();
            return true;
        }

        void IBootable.Launch()
        {
            this._Binder.BreakEvent += this._Quit;
            this._Binder.Bind<IVersion>(this);
            this._Binder.Bind<IAccountStatus>(this);
            this._ToVerify();
        }

        void IBootable.Shutdown()
        {
            this._SaveRecord();
            this._Binder.Unbind<IVersion>(this);
            this._Binder.Unbind<IAccountStatus>(this);
            this._Machine.Termination();
            this._Binder.BreakEvent -= this._Quit;
        }

        private void _Quit()
        {
            this._QuitEvent();
        }

        private void _SaveRecord()
        {
            if (this._GamePlayerRecord != null)
            {
                this._GameRecorder.Save(this._GamePlayerRecord);
            }
        }

        private void _ToVerify()
        {
            var verify = this._CreateVerify();
            this._AddVerifyToStage(verify);
        }

        private Verify _CreateVerify()
        {
            this._Account = null;
            var verify = new Verify(this._AccountFinder);
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
            this._ToLoadRecord();
        }

        private void _ToLoadRecord()
        {
            var stage = new LoadRecordStage(this._Account.Id , this._Binder, this._GameRecorder);
            stage.DoneEvent += this._ToRequestMap;
            this._Machine.Push(stage);
        }

        private void _ToRequestMap(GamePlayerRecord record)
        {
            this._GamePlayerRecord = record;
            _ToRealm("town1");
           
        }

        private void _ToGame(GamePlayerRecord record , Realm.Map map)
        {
            var player = EntityProvider.Create(record.Entity);

            var itemProvider = new ItemProvider();
            var itemAxe = itemProvider.MakeItem( "Axe1" , 0.5f );            
            var itemSword1 = itemProvider.MakeItem("Sword1" , 0.5f);
            var itemSword2 = itemProvider.MakeItem("Sword2" , 0.5f);
            var itemShield1 = itemProvider.MakeItem("Shield1", 0.5f);

            player.Bag.Add(itemAxe);
            player.Bag.Add(itemSword1);
            player.Bag.Add(itemSword2);
            player.Bag.Add(itemShield1);

            foreach (var item in record.Items)
            {
                player.Bag.Add(item);
            }

            var stage = new GameStage(this._Binder, map.Finder, map.Gate, player);
            stage.ExitEvent += () => { };
            stage.TransmitEvent += _ToRealm;
            _Machine.Push(stage);

        }

        private void _ToRealm(string target)
        {
            var stage = new RealmReadyStage(_Binder , _Zone, target);
            stage.GameEvent += (map) => { _ToGame(_GamePlayerRecord, map); };
            stage.ErrorEvent += _ToVerify;
            _Machine.Push(stage);            
        }

        string IVersion.Number {
            get { return _Version; }
        }
    }
}