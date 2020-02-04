   
    using System;  
    
    using System.Collections.Generic;
    
    namespace Regulus.Project.GameProject1.Data.Ghost 
    { 
        public class CICastSkill : Regulus.Project.GameProject1.Data.ICastSkill , Regulus.Remote.IGhost
        {
            readonly bool _HaveReturn ;
            
            readonly Guid _GhostIdName;
            
            
            
            public CICastSkill(Guid id, bool have_return )
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
            
            
                void Regulus.Project.GameProject1.Data.ICastSkill.Cast(Regulus.Project.GameProject1.Data.ACTOR_STATUS_TYPE _1)
                {                    

                    Regulus.Remote.IValue returnValue = null;
                    var info = typeof(Regulus.Project.GameProject1.Data.ICastSkill).GetMethod("Cast");
                    _CallMethodEvent(info , new object[] {_1} , returnValue);                    
                    
                }

                


                public Regulus.Project.GameProject1.Data.ACTOR_STATUS_TYPE _Id;
                Regulus.Project.GameProject1.Data.ACTOR_STATUS_TYPE Regulus.Project.GameProject1.Data.ICastSkill.Id { get{ return _Id;} }

                public Regulus.Project.GameProject1.Data.ACTOR_STATUS_TYPE[] _Skills;
                Regulus.Project.GameProject1.Data.ACTOR_STATUS_TYPE[] Regulus.Project.GameProject1.Data.ICastSkill.Skills { get{ return _Skills;} }

                public System.Action<Regulus.Project.GameProject1.Data.ACTOR_STATUS_TYPE[]> _HitNextsEvent;
                event System.Action<Regulus.Project.GameProject1.Data.ACTOR_STATUS_TYPE[]> Regulus.Project.GameProject1.Data.ICastSkill.HitNextsEvent
                {
                    add { _HitNextsEvent += value;}
                    remove { _HitNextsEvent -= value;}
                }
                
            
        }
    }
