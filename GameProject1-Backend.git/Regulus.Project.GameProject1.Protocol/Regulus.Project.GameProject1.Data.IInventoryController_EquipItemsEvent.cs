
    using System;  
    using System.Collections.Generic;
    
    namespace Regulus.Project.GameProject1.Data.Invoker.IInventoryController 
    { 
        public class EquipItemsEvent : Regulus.Remote.IEventProxyCreator
        {

            Type _Type;
            string _Name;
            
            public EquipItemsEvent()
            {
                _Name = "EquipItemsEvent";
                _Type = typeof(Regulus.Project.GameProject1.Data.IInventoryController);                   
            
            }
            Delegate Regulus.Remote.IEventProxyCreator.Create(Guid soul_id,int event_id, Regulus.Remote.InvokeEventCallabck invoke_Event)
            {                
                var closure = new Regulus.Remote.GenericEventClosure<Regulus.Project.GameProject1.Data.Item[]>(soul_id , event_id , invoke_Event);                
                return new Action<Regulus.Project.GameProject1.Data.Item[]>(closure.Run);
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
                