using System;

using Regulus.Framework;

using Regulus.Remote;
using Regulus.Utility;


namespace Regulus.Project.GameProject1.Storage
{
    public class Server : IEntry, Data.IStorage
    {
        private readonly Game.Storage.Center _Center;

        

        private readonly string _DefaultAdministratorName;

        private readonly string _Ip;

        private readonly LogFileRecorder _LogRecorder;

        private readonly string _Name;

        private readonly Updater _Updater;

        private IBinderProvider _BinderProvider;

        public Server()
        {
            this._LogRecorder = new LogFileRecorder("Storage");
            this._DefaultAdministratorName = "itisnotagame";
            this._Ip = "mongodb://127.0.0.1:27017";
            this._Name = "GameProject1";
            this._Updater = new Updater();
            
            this._Center = new Game.Storage.Center(this);

            _BinderProvider = _Center;
        }

        void IBinderProvider.AssignBinder(ISoulBinder binder)
        {
            _BinderProvider.AssignBinder(binder);
        }

        

        

        void IBootable.Launch()
        {
            Singleton<Log>.Instance.RecordEvent += this._LogRecorder.Record;
            AppDomain.CurrentDomain.UnhandledException += this.CurrentDomain_UnhandledException;

            this._Updater.Add(this._Center);
            

            this._HandleAdministrator();


            this._Updater.Working();

        }

        void IBootable.Shutdown()
        {
            
            this._Updater.Shutdown();

            AppDomain.CurrentDomain.UnhandledException -= this.CurrentDomain_UnhandledException;
            Singleton<Log>.Instance.RecordEvent -= this._LogRecorder.Record;
        }

        Value<Data.Account> Data.IAccountFinder.FindAccountByName(string name)
        {
            throw new NotImplementedException();
        }

        

        Value<Data.ACCOUNT_REQUEST_RESULT> Data.IAccountManager.Delete(string account)
        {
            throw new NotImplementedException();
        }

        Value<Data.Account[]> Data.IAccountManager.QueryAllAccount()
        {
            throw new NotImplementedException();
        }

        Value<Data.ACCOUNT_REQUEST_RESULT> Data.IAccountManager.Update(Data.Account account)
        {
            throw new NotImplementedException();
        }

        Value<Data.Account> Data.IAccountFinder.FindAccountById(Guid accountId)
        {
            throw new NotImplementedException();
        }

        Value<Data.GamePlayerRecord> Data.IGameRecorder.Load(Guid account_id)
        {
            throw new NotImplementedException();
        }

        void Data.IGameRecorder.Save(Data.GamePlayerRecord record)
        {
            throw new NotImplementedException();
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = (Exception)e.ExceptionObject;
            this._LogRecorder.Record(ex.ToString());
            this._LogRecorder.Save();
        }

        private async void _HandleAdministrator()
        {
            throw new NotImplementedException();
        }


        Value<Data.ACCOUNT_REQUEST_RESULT> Data.IAccountManager.Create(Data.Account account)
        {
            throw new NotImplementedException();
        }

        
    }
}
