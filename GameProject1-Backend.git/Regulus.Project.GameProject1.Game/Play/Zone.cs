using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

using Regulus.Utility;

using Regulus.Utility;
using Regulus.Project.GameProject1.Data;
namespace Regulus.Project.GameProject1.Game.Play
{
    public class Zone :IUpdatable
    {
        

        private readonly Dictionary<string, Realm> _Realms;

        private TimesharingUpdater _Updater;
        

        public Zone(RealmInfomation[] realm_infomations)
        {
            this._Realms = new Dictionary<string, Realm>();
            _Updater = new TimesharingUpdater(1f/10f);
            if (realm_infomations == null)
                throw new System.NullReferenceException();

            


            foreach (var realm_infomation in realm_infomations)
            {
                var realm = new Realm(realm_infomation);
                this._Realms.Add(realm_infomation.Name, realm);
                _Updater.Add(realm);
            }

        }

        public Realm FindRealm(string name)
        {
            Realm realm;
            return this._Realms.TryGetValue(name, out realm)
                ? realm
                : null;
        }        
        
        void IBootable.Launch()
        {
            
        }

        
        void IBootable.Shutdown()
        {
            _Updater.Shutdown();
        }

        bool IUpdatable.Update()
        {
            _Updater.Working();
            return true;
        }
    }
}