   
    using System;  
    
    using System.Collections.Generic;
    
    namespace Regulus.Project.GameProject1.Data.Ghost 
    { 
        public class CIEmotion : Regulus.Project.GameProject1.Data.IEmotion , Regulus.Remote.IGhost
        {
            readonly bool _HaveReturn ;
            
            readonly Guid _GhostIdName;
            
            
            
            public CIEmotion(Guid id, bool have_return )
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
            
            
                void Regulus.Project.GameProject1.Data.IEmotion.Talk(System.String _1)
                {                    

                    Regulus.Remote.IValue returnValue = null;
                    var info = typeof(Regulus.Project.GameProject1.Data.IEmotion).GetMethod("Talk");
                    _CallMethodEvent(info , new object[] {_1} , returnValue);                    
                    
                }

                



            
        }
    }
