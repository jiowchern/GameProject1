using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Regulus.Framework;
using Regulus.Game;
using Regulus.Project.GameProject1.Data;
using Regulus.Remote;
using Regulus.Utility;

namespace Regulus.Project.GameProject1.Game.Storage
{
    public class Center : IUpdatable , IBinderProvider
    {
        private readonly Hall _Hall;

        private readonly IStorage _Stroage;

        private readonly Updater _Update;

        public Center(IStorage storage)
        {
            this._Stroage = storage;
            this._Hall = new Hall();
            this._Update = new Updater();
        }

        void IBinderProvider.AssignBinder(ISoulBinder binder)
        {
            this._Hall.PushUser(new User(binder, this._Stroage));
        }

        bool IUpdatable.Update()
        {
            this._Update.Working();
            return true;
        }

        void IBootable.Launch()
        {
            this._Update.Add(this._Hall);
        }

        void IBootable.Shutdown()
        {
            this._Update.Shutdown();
        }
    }
}
