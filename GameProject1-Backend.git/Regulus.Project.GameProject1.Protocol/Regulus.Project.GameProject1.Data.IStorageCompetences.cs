   
    using System;  
    
    using System.Collections.Generic;
    
    namespace Regulus.Project.GameProject1.Data.Ghost 
    { 
        public class CIStorageCompetences : Regulus.Project.GameProject1.Data.IStorageCompetences , Regulus.Remote.IGhost
        {
            readonly bool _HaveReturn ;
            
            readonly Guid _GhostIdName;
            
            
            
            public CIStorageCompetences(Guid id, bool have_return )
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
            
            
                Regulus.Remote.Value<Regulus.Project.GameProject1.Data.Account.COMPETENCE[]> Regulus.Project.GameProject1.Data.IStorageCompetences.Query()
                {                    

                    
    var returnValue = new Regulus.Remote.Value<Regulus.Project.GameProject1.Data.Account.COMPETENCE[]>();
    

                    var info = typeof(Regulus.Project.GameProject1.Data.IStorageCompetences).GetMethod("Query");
                    _CallMethodEvent(info , new object[] {} , returnValue);                    
                    return returnValue;
                }

                
 

                Regulus.Remote.Value<System.Guid> Regulus.Project.GameProject1.Data.IStorageCompetences.QueryForId()
                {                    

                    
    var returnValue = new Regulus.Remote.Value<System.Guid>();
    

                    var info = typeof(Regulus.Project.GameProject1.Data.IStorageCompetences).GetMethod("QueryForId");
                    _CallMethodEvent(info , new object[] {} , returnValue);                    
                    return returnValue;
                }

                



            
        }
    }
