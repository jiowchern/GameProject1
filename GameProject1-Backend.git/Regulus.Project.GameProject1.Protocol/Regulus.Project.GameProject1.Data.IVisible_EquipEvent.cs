
    using System;  
    using System.Collections.Generic;
    
    namespace Regulus.Project.GameProject1.Data.Invoker.IVisible 
    { 
        public class EquipEvent : Regulus.Remote.IEventProxyCreator
        {

            Type _Type;
            string _Name;
            
            public EquipEvent()
            {
                _Name = "EquipEvent";
                _Type = typeof(Regulus.Project.GameProject1.Data.IVisible);                   
            
            }
            Delegate Regulus.Remote.IEventProxyCreator.Create(Guid soul_id,int event_id, Regulus.Remote.InvokeEventCallabck invoke_Event)
            {                
                var closure = new Regulus.Remote.GenericEventClosure<Regulus.Project.GameProject1.Data.EquipStatus[]>(soul_id , event_id , invoke_Event);                
                return new Action<Regulus.Project.GameProject1.Data.EquipStatus[]>(closure.Run);
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
                