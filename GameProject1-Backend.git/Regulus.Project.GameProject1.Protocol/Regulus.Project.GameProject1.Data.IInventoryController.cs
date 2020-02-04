   
    using System;  
    
    using System.Collections.Generic;
    
    namespace Regulus.Project.GameProject1.Data.Ghost 
    { 
        public class CIInventoryController : Regulus.Project.GameProject1.Data.IInventoryController , Regulus.Remote.IGhost
        {
            readonly bool _HaveReturn ;
            
            readonly Guid _GhostIdName;
            
            
            
            public CIInventoryController(Guid id, bool have_return )
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
            
            
                void Regulus.Project.GameProject1.Data.IInventoryController.Refresh()
                {                    

                    Regulus.Remote.IValue returnValue = null;
                    var info = typeof(Regulus.Project.GameProject1.Data.IInventoryController).GetMethod("Refresh");
                    _CallMethodEvent(info , new object[] {} , returnValue);                    
                    
                }

                
 

                void Regulus.Project.GameProject1.Data.IInventoryController.Discard(System.Guid _1)
                {                    

                    Regulus.Remote.IValue returnValue = null;
                    var info = typeof(Regulus.Project.GameProject1.Data.IInventoryController).GetMethod("Discard");
                    _CallMethodEvent(info , new object[] {_1} , returnValue);                    
                    
                }

                
 

                void Regulus.Project.GameProject1.Data.IInventoryController.Equip(System.Guid _1)
                {                    

                    Regulus.Remote.IValue returnValue = null;
                    var info = typeof(Regulus.Project.GameProject1.Data.IInventoryController).GetMethod("Equip");
                    _CallMethodEvent(info , new object[] {_1} , returnValue);                    
                    
                }

                
 

                void Regulus.Project.GameProject1.Data.IInventoryController.Use(System.Guid _1)
                {                    

                    Regulus.Remote.IValue returnValue = null;
                    var info = typeof(Regulus.Project.GameProject1.Data.IInventoryController).GetMethod("Use");
                    _CallMethodEvent(info , new object[] {_1} , returnValue);                    
                    
                }

                
 

                void Regulus.Project.GameProject1.Data.IInventoryController.Unequip(System.Guid _1)
                {                    

                    Regulus.Remote.IValue returnValue = null;
                    var info = typeof(Regulus.Project.GameProject1.Data.IInventoryController).GetMethod("Unequip");
                    _CallMethodEvent(info , new object[] {_1} , returnValue);                    
                    
                }

                



                public System.Action<Regulus.Project.GameProject1.Data.Item[]> _BagItemsEvent;
                event System.Action<Regulus.Project.GameProject1.Data.Item[]> Regulus.Project.GameProject1.Data.IInventoryController.BagItemsEvent
                {
                    add { _BagItemsEvent += value;}
                    remove { _BagItemsEvent -= value;}
                }
                

                public System.Action<Regulus.Project.GameProject1.Data.Item[]> _EquipItemsEvent;
                event System.Action<Regulus.Project.GameProject1.Data.Item[]> Regulus.Project.GameProject1.Data.IInventoryController.EquipItemsEvent
                {
                    add { _EquipItemsEvent += value;}
                    remove { _EquipItemsEvent -= value;}
                }
                
            
        }
    }
