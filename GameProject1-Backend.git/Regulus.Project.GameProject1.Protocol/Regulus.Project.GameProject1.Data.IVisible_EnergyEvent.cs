
    using System;  
    using System.Collections.Generic;
    
    namespace Regulus.Project.GameProject1.Data.Invoker.IVisible 
    { 
        public class EnergyEvent : Regulus.Remote.IEventProxyCreator
        {

            Type _Type;
            string _Name;
            
            public EnergyEvent()
            {
                _Name = "EnergyEvent";
                _Type = typeof(Regulus.Project.GameProject1.Data.IVisible);                   
            
            }
            Delegate Regulus.Remote.IEventProxyCreator.Create(Guid soul_id,int event_id, Regulus.Remote.InvokeEventCallabck invoke_Event)
            {                
                var closure = new Regulus.Remote.GenericEventClosure<Regulus.Project.GameProject1.Data.Energy>(soul_id , event_id , invoke_Event);                
                return new Action<Regulus.Project.GameProject1.Data.Energy>(closure.Run);
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
                