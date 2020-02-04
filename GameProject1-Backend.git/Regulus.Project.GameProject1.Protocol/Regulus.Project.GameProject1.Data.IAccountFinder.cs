   
    using System;  
    
    using System.Collections.Generic;
    
    namespace Regulus.Project.GameProject1.Data.Ghost 
    { 
        public class CIAccountFinder : Regulus.Project.GameProject1.Data.IAccountFinder , Regulus.Remote.IGhost
        {
            readonly bool _HaveReturn ;
            
            readonly Guid _GhostIdName;
            
            
            
            public CIAccountFinder(Guid id, bool have_return )
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
            
            
                Regulus.Remote.Value<Regulus.Project.GameProject1.Data.Account> Regulus.Project.GameProject1.Data.IAccountFinder.FindAccountByName(System.String _1)
                {                    

                    
    var returnValue = new Regulus.Remote.Value<Regulus.Project.GameProject1.Data.Account>();
    

                    var info = typeof(Regulus.Project.GameProject1.Data.IAccountFinder).GetMethod("FindAccountByName");
                    _CallMethodEvent(info , new object[] {_1} , returnValue);                    
                    return returnValue;
                }

                
 

                Regulus.Remote.Value<Regulus.Project.GameProject1.Data.Account> Regulus.Project.GameProject1.Data.IAccountFinder.FindAccountById(System.Guid _1)
                {                    

                    
    var returnValue = new Regulus.Remote.Value<Regulus.Project.GameProject1.Data.Account>();
    

                    var info = typeof(Regulus.Project.GameProject1.Data.IAccountFinder).GetMethod("FindAccountById");
                    _CallMethodEvent(info , new object[] {_1} , returnValue);                    
                    return returnValue;
                }

                



            
        }
    }
