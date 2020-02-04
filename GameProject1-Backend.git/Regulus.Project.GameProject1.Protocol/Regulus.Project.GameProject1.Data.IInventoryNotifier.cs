   
    using System;  
    
    using System.Collections.Generic;
    
    namespace Regulus.Project.GameProject1.Data.Ghost 
    { 
        public class CIInventoryNotifier : Regulus.Project.GameProject1.Data.IInventoryNotifier , Regulus.Remote.IGhost
        {
            readonly bool _HaveReturn ;
            
            readonly Guid _GhostIdName;
            
            
            
            public CIInventoryNotifier(Guid id, bool have_return )
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
            
            


                public System.Action<Regulus.Project.GameProject1.Data.Item> _AddEvent;
                event System.Action<Regulus.Project.GameProject1.Data.Item> Regulus.Project.GameProject1.Data.IInventoryNotifier.AddEvent
                {
                    add { _AddEvent += value;}
                    remove { _AddEvent -= value;}
                }
                

                public System.Action<System.Guid> _RemoveEvent;
                event System.Action<System.Guid> Regulus.Project.GameProject1.Data.IInventoryNotifier.RemoveEvent
                {
                    add { _RemoveEvent += value;}
                    remove { _RemoveEvent -= value;}
                }
                
            
        }
    }
