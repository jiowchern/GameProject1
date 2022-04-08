using System;
using Regulus.Remote;

namespace Regulus.Project.GameProject1.Game.Play
{
    class UnbindHelper : IDisposable
    {
        private readonly IBinder _Binder;
        readonly System.Collections.Generic.List<ISoul> _Souls;

        public UnbindHelper(IBinder binder)
        {
            _Souls = new System.Collections.Generic.List<ISoul>();
            this._Binder = binder;
        }
        public static UnbindHelper operator + (UnbindHelper  h, ISoul soul)
        {
            h._Souls.Add(soul);
            return h;
        }

        void IDisposable.Dispose()
        {
            Release();
        }

        internal void Release()
        {
            foreach (var soul in _Souls)
            {
                _Binder.Unbind(soul);
            }
            _Souls.Clear();
        }
    }
}