using System;
using Regulus.Project.GameProject1.Data;
using Regulus.Remote;
using Regulus.Utility;

namespace Regulus.Project.GameProject1.Game.Play
{
    internal class LoadRecordStage : IStatus
    {
        private readonly Guid _AccountId;
        private readonly IBinder _Binder;
        private readonly IGameRecorder _GameRecorder;

        public delegate void RecordCallback(GamePlayerRecord record);

        public event RecordCallback DoneEvent;

        public LoadRecordStage(Guid account_id , IBinder binder, IGameRecorder gameRecorder)
        {
            this._AccountId = account_id;
            this._Binder = binder;
            this._GameRecorder = gameRecorder;
        }

        void IStatus.Enter()
        {
            this._GameRecorder.Load(this._AccountId).OnValue += this._LoadResult; 
        }

        private void _LoadResult(GamePlayerRecord obj)
        {
            this.DoneEvent(obj);
        }

        void IStatus.Leave()
        {
            
        }

        void IStatus.Update()
        {

        }
    }
}