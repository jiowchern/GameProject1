using System;
using System.Collections.Generic;

using Regulus.Utility;

namespace Regulus.Project.GameProject1.Game.Play
{
    internal class SignRoster
    {
        struct Record
        {
            public Regulus.Utility.TimeCounter Time;
        }

        private Dictionary<Guid,Record> _Records;

        public SignRoster()
        {
            _Records = new Dictionary<Guid, Record>();
        }
        public bool Sign(Guid id)
        {
            Record record;
            if (_Records.TryGetValue(id, out record))
            {
                if (record.Time.Second > 10f)
                {
                    record.Time.Reset();
                    return true;
                }
                else
                {
                    return false;
                }
                                
            }
            
            _Records.Add(id , new Record() {Time = new TimeCounter()});
            return true;
        }
    }
}