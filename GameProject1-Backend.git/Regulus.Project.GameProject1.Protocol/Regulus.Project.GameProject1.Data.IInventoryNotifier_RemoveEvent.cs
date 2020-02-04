
    using System;  
    using System.Collections.Generic;
    
    namespace Regulus.Project.GameProject1.Data.Invoker.IInventoryNotifier 
    { 
        public class RemoveEvent : Regulus.Remote.IEventProxyCreator
        {

            Type _Type;
            string _Name;
            
            public RemoveEvent()
            {
                _Name = "RemoveEvent";
                _Type = typeof(Regulus.Project.GameProject1.Data.IInventoryNotifier);                   
            
            }
            Delegate Regulus.Remote.IEventProxyCreator.Create(Guid soul_id,int event_id, Regulus.Remote.InvokeEventCallabck invoke_Event)
            {                
                var closure = new Regulus.Remote.GenericEventClosure<System.Guid>(soul_id , event_id , invoke_Event);                
                return new Action<System.Guid>(closure.Run);
            }
        

            Type Regulus.Remote.IEventProxyCreator.GetType()
            {
                return _Type;
            }            

            string Regulus.Remote.IEventProxyCreator.GetName()
            {
                return _Name;
            }            
        }
    }
                