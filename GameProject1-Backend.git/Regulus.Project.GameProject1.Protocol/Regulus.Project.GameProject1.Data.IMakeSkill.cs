   
    using System;  
    
    using System.Collections.Generic;
    
    namespace Regulus.Project.GameProject1.Data.Ghost 
    { 
        public class CIMakeSkill : Regulus.Project.GameProject1.Data.IMakeSkill , Regulus.Remote.IGhost
        {
            readonly bool _HaveReturn ;
            
            readonly Guid _GhostIdName;
            
            
            
            public CIMakeSkill(Guid id, bool have_return )
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
            
            
                void Regulus.Project.GameProject1.Data.IMakeSkill.Create(System.String _1,System.Int32[] _2)
                {                    

                    Regulus.Remote.IValue returnValue = null;
                    var info = typeof(Regulus.Project.GameProject1.Data.IMakeSkill).GetMethod("Create");
                    _CallMethodEvent(info , new object[] {_1,_2} , returnValue);                    
                    
                }

                
 

                void Regulus.Project.GameProject1.Data.IMakeSkill.Exit()
                {                    

                    Regulus.Remote.IValue returnValue = null;
                    var info = typeof(Regulus.Project.GameProject1.Data.IMakeSkill).GetMethod("Exit");
                    _CallMethodEvent(info , new object[] {} , returnValue);                    
                    
                }

                
 

                void Regulus.Project.GameProject1.Data.IMakeSkill.QueryFormula()
                {                    

                    Regulus.Remote.IValue returnValue = null;
                    var info = typeof(Regulus.Project.GameProject1.Data.IMakeSkill).GetMethod("QueryFormula");
                    _CallMethodEvent(info , new object[] {} , returnValue);                    
                    
                }

                



                public System.Action<Regulus.Project.GameProject1.Data.ItemFormulaLite[]> _FormulasEvent;
                event System.Action<Regulus.Project.GameProject1.Data.ItemFormulaLite[]> Regulus.Project.GameProject1.Data.IMakeSkill.FormulasEvent
                {
                    add { _FormulasEvent += value;}
                    remove { _FormulasEvent -= value;}
                }
                
            
        }
    }
