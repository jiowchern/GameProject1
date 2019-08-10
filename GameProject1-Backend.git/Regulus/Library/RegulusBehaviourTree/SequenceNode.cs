﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Regulus.BehaviourTree
{
    class SequenceNode  : IParent
    {
        private List<ITicker> _Childs;
        private readonly Queue<ITicker> _Queue;


        private ITicker _RunninTicker;
        private ITicker _CurrentTicker;
        private readonly Guid _Id;
        private readonly string _Tag;
        public SequenceNode()
        {
            _Tag = "Sequence";
            _Id = Guid.NewGuid();
            _Childs = new List<ITicker>();
            _Queue = new Queue<ITicker>();
        }

        public Guid Id { get { return _Id; } }
        string ITicker.Tag { get { return _Tag; } }

        ITicker[] ITicker.GetChilds()
        {
            return _Childs.ToArray();
        }

        void ITicker.GetPath(ref List<Guid> nodes)
        {
            nodes.Add(_Id);
            if (_CurrentTicker != null)
                _CurrentTicker.GetPath(ref nodes);
        }

        void ITicker.Reset()
        {
            if (_RunninTicker != null)
            {
                _RunninTicker.Reset();
                _RunninTicker = null;
            }
            _Queue.Clear();
        }

        public TICKRESULT Tick(float delta)
        {
            if (_RunninTicker != null)
            {
                var resultRunner = _RunninTicker.Tick(delta);
                
                if (resultRunner == TICKRESULT.RUNNING)
                    return TICKRESULT.RUNNING;

                _RunninTicker = null;
                if (resultRunner == TICKRESULT.FAILURE)
                {
                    _Queue.Clear();
                    return TICKRESULT.FAILURE;
                }

                if (_Queue.Count == 0)
                {                    
                    return TICKRESULT.SUCCESS;
                }

            }

            if (_Queue.Count == 0)
            {
                _Reload();
            }

            _CurrentTicker = _Queue.Dequeue();
            var result = _CurrentTicker.Tick(delta);
            if (result == TICKRESULT.FAILURE)
            {
                _Queue.Clear();
                return TICKRESULT.FAILURE;
            }


            if (result == TICKRESULT.RUNNING)
            {
                _RunninTicker = _CurrentTicker;
                return TICKRESULT.RUNNING;
            }

            if (_Queue.Count == 0)
                return TICKRESULT.SUCCESS;
            
            return TICKRESULT.RUNNING;
        }

        private void _Reload()
        {
            
            foreach (var ticker in _Childs)
            {
                
                _Queue.Enqueue(ticker);
            }
        }


        public void Add(ITicker ticker)
        {
            _Childs.Add(ticker);
        }
    }
}
