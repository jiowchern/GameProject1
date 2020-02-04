   
    using System;  
    
    using System.Collections.Generic;
    
    namespace Regulus.Project.GameProject1.Data.Ghost 
    { 
        public class CIAccountManager : Regulus.Project.GameProject1.Data.IAccountManager , Regulus.Remote.IGhost
        {
            readonly bool _HaveReturn ;
            
            readonly Guid _GhostIdName;
            
            
            
            public CIAccountManager(Guid id, bool have_return )
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
            
            
                Regulus.Remote.Value<Regulus.Project.GameProject1.Data.ACCOUNT_REQUEST_RESULT> Regulus.Project.GameProject1.Data.IAccountManager.Create(Regulus.Project.GameProject1.Data.Account _1)
                {                    

                    
    var returnValue = new Regulus.Remote.Value<Regulus.Project.GameProject1.Data.ACCOUNT_REQUEST_RESULT>();
    

                    var info = typeof(Regulus.Project.GameProject1.Data.IAccountManager).GetMethod("Create");
                    _CallMethodEvent(info , new object[] {_1} , returnValue);                    
                    return returnValue;
                }

                
 

                Regulus.Remote.Value<Regulus.Project.GameProject1.Data.Account[]> Regulus.Project.GameProject1.Data.IAccountManager.QueryAllAccount()
                {                    

                    
    var returnValue = new Regulus.Remote.Value<Regulus.Project.GameProject1.Data.Account[]>();
    

                    var info = typeof(Regulus.Project.GameProject1.Data.IAccountManager).GetMethod("QueryAllAccount");
                    _CallMethodEvent(info , new object[] {} , returnValue);                    
                    return returnValue;
                }

                
 

                Regulus.Remote.Value<Regulus.Project.GameProject1.Data.ACCOUNT_REQUEST_RESULT> Regulus.Project.GameProject1.Data.IAccountManager.Delete(System.String _1)
                {                    

                    
    var returnValue = new Regulus.Remote.Value<Regulus.Project.GameProject1.Data.ACCOUNT_REQUEST_RESULT>();
    

                    var info = typeof(Regulus.Project.GameProject1.Data.IAccountManager).GetMethod("Delete");
                    _CallMethodEvent(info , new object[] {_1} , returnValue);                    
                    return returnValue;
                }

                
 

                Regulus.Remote.Value<Regulus.Project.GameProject1.Data.ACCOUNT_REQUEST_RESULT> Regulus.Project.GameProject1.Data.IAccountManager.Update(Regulus.Project.GameProject1.Data.Account _1)
                {                    

                    
    var returnValue = new Regulus.Remote.Value<Regulus.Project.GameProject1.Data.ACCOUNT_REQUEST_RESULT>();
    

                    var info = typeof(Regulus.Project.GameProject1.Data.IAccountManager).GetMethod("Update");
                    _CallMethodEvent(info , new object[] {_1} , returnValue);                    
                    return returnValue;
                }

                



            
        }
    }
