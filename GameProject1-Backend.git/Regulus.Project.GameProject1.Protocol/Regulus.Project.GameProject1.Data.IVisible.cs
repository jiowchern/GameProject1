   
    using System;  
    
    using System.Collections.Generic;
    
    namespace Regulus.Project.GameProject1.Data.Ghost 
    { 
        public class CIVisible : Regulus.Project.GameProject1.Data.IVisible , Regulus.Remote.IGhost
        {
            readonly bool _HaveReturn ;
            
            readonly Guid _GhostIdName;
            
            
            
            public CIVisible(Guid id, bool have_return )
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
            
            
                void Regulus.Project.GameProject1.Data.IVisible.QueryStatus()
                {                    

                    Regulus.Remote.IValue returnValue = null;
                    var info = typeof(Regulus.Project.GameProject1.Data.IVisible).GetMethod("QueryStatus");
                    _CallMethodEvent(info , new object[] {} , returnValue);                    
                    
                }

                


                public Regulus.Project.GameProject1.Data.ENTITY _EntityType;
                Regulus.Project.GameProject1.Data.ENTITY Regulus.Project.GameProject1.Data.IVisible.EntityType { get{ return _EntityType;} }

                public System.Guid _Id;
                System.Guid Regulus.Project.GameProject1.Data.IVisible.Id { get{ return _Id;} }

                public System.String _Name;
                System.String Regulus.Project.GameProject1.Data.IVisible.Name { get{ return _Name;} }

                public System.Single _View;
                System.Single Regulus.Project.GameProject1.Data.IVisible.View { get{ return _View;} }

                public System.Single _Direction;
                System.Single Regulus.Project.GameProject1.Data.IVisible.Direction { get{ return _Direction;} }

                public Regulus.Project.GameProject1.Data.ACTOR_STATUS_TYPE _Status;
                Regulus.Project.GameProject1.Data.ACTOR_STATUS_TYPE Regulus.Project.GameProject1.Data.IVisible.Status { get{ return _Status;} }

                public Regulus.Utility.Vector2 _Position;
                Regulus.Utility.Vector2 Regulus.Project.GameProject1.Data.IVisible.Position { get{ return _Position;} }

                public System.Action<Regulus.Project.GameProject1.Data.EquipStatus[]> _EquipEvent;
                event System.Action<Regulus.Project.GameProject1.Data.EquipStatus[]> Regulus.Project.GameProject1.Data.IVisible.EquipEvent
                {
                    add { _EquipEvent += value;}
                    remove { _EquipEvent -= value;}
                }
                

                public System.Action<Regulus.Project.GameProject1.Data.VisibleStatus> _StatusEvent;
                event System.Action<Regulus.Project.GameProject1.Data.VisibleStatus> Regulus.Project.GameProject1.Data.IVisible.StatusEvent
                {
                    add { _StatusEvent += value;}
                    remove { _StatusEvent -= value;}
                }
                

                public System.Action<Regulus.Project.GameProject1.Data.Energy> _EnergyEvent;
                event System.Action<Regulus.Project.GameProject1.Data.Energy> Regulus.Project.GameProject1.Data.IVisible.EnergyEvent
                {
                    add { _EnergyEvent += value;}
                    remove { _EnergyEvent -= value;}
                }
                

                public System.Action<System.String> _TalkMessageEvent;
                event System.Action<System.String> Regulus.Project.GameProject1.Data.IVisible.TalkMessageEvent
                {
                    add { _TalkMessageEvent += value;}
                    remove { _TalkMessageEvent -= value;}
                }
                
            
        }
    }
