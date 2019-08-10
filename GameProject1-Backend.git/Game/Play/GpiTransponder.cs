using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Regulus.Remote;

namespace Regulus.Project.GameProject1.Game.Play
{
    public class GpiTransponder : ISoulBinder
    {

        
        private readonly Dictionary<Type, IGpiProvider> _Gpis;

        public GpiTransponder()
        {
            _Gpis = new Dictionary<Type, IGpiProvider>();
        }
        void ISoulBinder.Return<TSoul>(TSoul soul)
        {
            _Add(soul);
        }

        private void _Add<TSoul>(TSoul soul) 
        {
            var key = typeof (TSoul);
            if (_Gpis.ContainsKey(key))
            {
                _Gpis[key].Add(soul);
            }
            else
            {
                var porvider = new TGpiProvider<TSoul>();
                porvider.Add(soul);
                _Gpis.Add(key, porvider);
            }
        }

        void ISoulBinder.Bind<TSoul>(TSoul soul) 
        {
            _Add(soul);
        }

        void ISoulBinder.Unbind<TSoul>(TSoul soul) 
        {
            var key = typeof(TSoul);
            if (_Gpis.ContainsKey(key))
            {
                _Gpis[key].Remove(soul);
            }
            
        }

        private event Action _BreakEvent;
        event Action ISoulBinder.BreakEvent
        {
            add { _BreakEvent += value; }
            remove { _BreakEvent -= value; }
        }

        public TGpiProvider<T> Query<T>()
        {
            var key = typeof (T);
            if (_Gpis.ContainsKey(key))
            {
                return _Gpis[key] as TGpiProvider<T>;
            }
            var provider = new TGpiProvider<T>();
            _Gpis.Add( key , provider);
            return provider;
        }
    }

    interface IGpiProvider
    {
        void Add(object soul);

        void Remove(object soul);
    }
    public class TGpiProvider<T> : IGpiProvider 
    {
        private List<T> _Gpis;


        
        private event Action<T> _Supply;
        public event Action<T> Supply
        {
            add
            {
                foreach (var gpi in _Gpis)
                {
                    value(gpi);
                }
                _Supply += value;
            }

            remove
            {
                _Supply -= value;
            }
        }


        public event Action<T> Unsupply;
        public TGpiProvider()
        {
            _Gpis = new List<T>();
        }

        public void Add(object soul)
        {
            _Gpis.Add((T)soul);
            if (_Supply != null)
            {
                _Supply((T)soul);
            }
        }

        public void Remove(object soul)
        {
            _Gpis.Remove((T)soul );
            if (Unsupply != null)
            {
                Unsupply((T)soul);
            }
        }

        public T FirstOrDefault()
        {
            return _Gpis.FirstOrDefault();
        }
    }
}

