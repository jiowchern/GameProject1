using Regulus.Project.GameProject1.Data;
using Regulus.Project.GameProject1.Storage.User;
using Regulus.Utility;

namespace Regulus.Project.GameProject1.Play
{
    internal class BuildCenterStage : IStatus
    {
        public delegate void SuccessBuiledCallback(ExternalFeature features);

        public event SuccessBuiledCallback OnBuiledEvent;

        public struct ExternalFeature
        {
            public IAccountFinder AccountFinder;

            

            public IGameRecorder GameRecorder;




        }

        

        private readonly IUser _StorageUser;

        private ExternalFeature _Feature;

        public BuildCenterStage( IUser storage_user)
        {
            this._Feature = new ExternalFeature();

            this._StorageUser = storage_user;
        }

        void IStatus.Enter()
        {
            this._StorageUser.QueryProvider<IAccountFinder>().Supply += this._AccountFinder;
        }

        void IStatus.Leave()
        {
        }

        void IStatus.Update()
        {
        }

        

        private void _AccountFinder(IAccountFinder obj)
        {
            this._StorageUser.QueryProvider<IAccountFinder>().Supply -= this._AccountFinder;
            this._Feature.AccountFinder = obj;

            this._StorageUser.QueryProvider<IGameRecorder>().Supply += this._RecordQueriers;
        }

        
        private void _RecordQueriers(IGameRecorder obj)
        {
            this._StorageUser.QueryProvider<IGameRecorder>().Supply -= this._RecordQueriers;
            this._Feature.GameRecorder = obj;

            this.OnBuiledEvent(this._Feature);
        }

       
    }
}