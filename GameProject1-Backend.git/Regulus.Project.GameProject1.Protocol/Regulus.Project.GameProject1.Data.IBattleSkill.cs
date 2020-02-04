   
    using System;  
    
    using System.Collections.Generic;
    
    namespace Regulus.Project.GameProject1.Data.Ghost 
    { 
        public class CIBattleSkill : Regulus.Project.GameProject1.Data.IBattleSkill , Regulus.Remote.IGhost
        {
            readonly bool _HaveReturn ;
            
            readonly Guid _GhostIdName;
            
            
            
            public CIBattleSkill(Guid id, bool have_return )
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
            
            
                void Regulus.Project.GameProject1.Data.IBattleSkill.Disarm()
                {                    

                    Regulus.Remote.IValue returnValue = null;
                    var info = typeof(Regulus.Project.GameProject1.Data.IBattleSkill).GetMethod("Disarm");
                    _CallMethodEvent(info , new object[] {} , returnValue);                    
                    
                }

                



            
        }
    }
