   
    using System;  
    
    using System.Collections.Generic;
    
    namespace Regulus.Project.GameProject1.Data.Ghost 
    { 
        public class CIVerify : Regulus.Project.GameProject1.Data.IVerify , Regulus.Remote.IGhost
        {
            readonly bool _HaveReturn ;
            
            readonly Guid _GhostIdName;
            
            
            
            public CIVerify(Guid id, bool have_return )
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
            
            
                Regulus.Remote.Value<System.Boolean> Regulus.Project.GameProject1.Data.IVerify.Login(System.String _1,System.String _2)
                {                    

                    
    var returnValue = new Regulus.Remote.Value<System.Boolean>();
    

                    var info = typeof(Regulus.Project.GameProject1.Data.IVerify).GetMethod("Login");
                    _CallMethodEvent(info , new object[] {_1,_2} , returnValue);                    
                    return returnValue;
                }

                



            
        }
    }
