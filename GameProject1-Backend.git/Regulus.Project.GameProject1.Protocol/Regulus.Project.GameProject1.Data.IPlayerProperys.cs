   
    using System;  
    
    using System.Collections.Generic;
    
    namespace Regulus.Project.GameProject1.Data.Ghost 
    { 
        public class CIPlayerProperys : Regulus.Project.GameProject1.Data.IPlayerProperys , Regulus.Remote.IGhost
        {
            readonly bool _HaveReturn ;
            
            readonly Guid _GhostIdName;
            
            
            
            public CIPlayerProperys(Guid id, bool have_return )
            {
                _HaveReturn = have_return ;
                _GhostIdName = id;            
            }
            

            Guid Regulus.Remote.IGhost.GetID()
            {
                return _GhostIdName;
            }

            bool Regulus.Remote.IGhost.IsReturnType()
            {
                return _HaveReturn;
            }
            object Regulus.Remote.IGhost.GetInstance()
            {
                return this;
            }

            private event Regulus.Remote.CallMethodCallback _CallMethodEvent;

            event Regulus.Remote.CallMethodCallback Regulus.Remote.IGhost.CallMethodEvent
            {
                add { this._CallMethodEvent += value; }
                remove { this._CallMethodEvent -= value; }
            }
            
            

                public System.String _Realm;
                System.String Regulus.Project.GameProject1.Data.IPlayerProperys.Realm { get{ return _Realm;} }

                public System.Guid _Id;
                System.Guid Regulus.Project.GameProject1.Data.IPlayerProperys.Id { get{ return _Id;} }

                public System.Single _Strength;
                System.Single Regulus.Project.GameProject1.Data.IPlayerProperys.Strength { get{ return _Strength;} }

                public System.Single _Health;
                System.Single Regulus.Project.GameProject1.Data.IPlayerProperys.Health { get{ return _Health;} }

            
        }
    }
