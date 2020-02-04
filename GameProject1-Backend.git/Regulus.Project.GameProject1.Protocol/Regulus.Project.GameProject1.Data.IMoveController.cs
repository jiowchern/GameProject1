   
    using System;  
    
    using System.Collections.Generic;
    
    namespace Regulus.Project.GameProject1.Data.Ghost 
    { 
        public class CIMoveController : Regulus.Project.GameProject1.Data.IMoveController , Regulus.Remote.IGhost
        {
            readonly bool _HaveReturn ;
            
            readonly Guid _GhostIdName;
            
            
            
            public CIMoveController(Guid id, bool have_return )
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
            
            
                void Regulus.Project.GameProject1.Data.IMoveController.RunForward()
                {                    

                    Regulus.Remote.IValue returnValue = null;
                    var info = typeof(Regulus.Project.GameProject1.Data.IMoveController).GetMethod("RunForward");
                    _CallMethodEvent(info , new object[] {} , returnValue);                    
                    
                }

                
 

                void Regulus.Project.GameProject1.Data.IMoveController.Forward()
                {                    

                    Regulus.Remote.IValue returnValue = null;
                    var info = typeof(Regulus.Project.GameProject1.Data.IMoveController).GetMethod("Forward");
                    _CallMethodEvent(info , new object[] {} , returnValue);                    
                    
                }

                
 

                void Regulus.Project.GameProject1.Data.IMoveController.Backward()
                {                    

                    Regulus.Remote.IValue returnValue = null;
                    var info = typeof(Regulus.Project.GameProject1.Data.IMoveController).GetMethod("Backward");
                    _CallMethodEvent(info , new object[] {} , returnValue);                    
                    
                }

                
 

                void Regulus.Project.GameProject1.Data.IMoveController.StopMove()
                {                    

                    Regulus.Remote.IValue returnValue = null;
                    var info = typeof(Regulus.Project.GameProject1.Data.IMoveController).GetMethod("StopMove");
                    _CallMethodEvent(info , new object[] {} , returnValue);                    
                    
                }

                
 

                void Regulus.Project.GameProject1.Data.IMoveController.TrunLeft()
                {                    

                    Regulus.Remote.IValue returnValue = null;
                    var info = typeof(Regulus.Project.GameProject1.Data.IMoveController).GetMethod("TrunLeft");
                    _CallMethodEvent(info , new object[] {} , returnValue);                    
                    
                }

                
 

                void Regulus.Project.GameProject1.Data.IMoveController.TrunRight()
                {                    

                    Regulus.Remote.IValue returnValue = null;
                    var info = typeof(Regulus.Project.GameProject1.Data.IMoveController).GetMethod("TrunRight");
                    _CallMethodEvent(info , new object[] {} , returnValue);                    
                    
                }

                
 

                void Regulus.Project.GameProject1.Data.IMoveController.StopTrun()
                {                    

                    Regulus.Remote.IValue returnValue = null;
                    var info = typeof(Regulus.Project.GameProject1.Data.IMoveController).GetMethod("StopTrun");
                    _CallMethodEvent(info , new object[] {} , returnValue);                    
                    
                }

                



            
        }
    }
