
using System;
using System.Linq;

using Regulus.Project.GameProject1.Data;
using Regulus.Remote;
using Regulus.Utility;

namespace Regulus.Project.GameProject1.Game.Play
{
    internal class MakeStatus : IStatus , IMakeSkill
    {
        private readonly IBinder _Binder;

        private readonly Entity _Player;

        public event Action DoneEvent;

        private ItemFormula[] _Formulas;
        
        

        public MakeStatus(IBinder binder, Entity player)
        {            
            _Binder = binder;   
            _Player = player;            
        }

        void IStatus.Enter()
        {
            _Player.Make();
            _Formulas = _Player.GetFormulas();
            
            _Binder.Bind<IMakeSkill>(this);
            
        }

        void IStatus.Leave()
        {
            _Binder.Unbind<IMakeSkill>(this);
        }

        void IStatus.Update()
        {
            
        }

        private ItemFormulaLite[] _BuildLite(ItemFormula[] formulas)
        {
            return (from f in formulas
                    select new ItemFormulaLite
                    {
                        Item = f.Item,
                        Id = f.Id,
                        NeedLimit =  f.NeedLimit,
                        NeedItems = (from need in f.NeedItems
                                     select new ItemFormulaNeedLite
                                     {
                                         Item = need.Item,
                                         Min = need.Min
                                     }).ToArray()
                    }).ToArray();
        }

        void IMakeSkill.Create(string name, int[] amounts)
        {
            var formula = (from f in _Formulas where f.Id == name select f).FirstOrDefault();
            if (formula == null)
                return;
            var needItems = formula.NeedItems;
            if(needItems.Length != amounts.Length)
                return;            

            for (int i = 0; i < amounts.Length  ; ++i)
            {
                if(needItems[i].Min > amounts[i])
                    return;
            }

            for(int i = 0; i < needItems.Length; i++)
            {
                var amount = _Player.Bag.GetItemAmount(needItems[i].Item);
                if(amount < amounts[i])
                    return;

                
            }
            for (int i = 0; i < needItems.Length; i++)
                _Player.Bag.Remove(needItems[i].Item, amounts[i]);


            _Create(formula , amounts);
        }

        private void _Create(ItemFormula key , int[] amounts)
        {
            var quality = ItemProvider.GetQuality(key, amounts);

            var itemProvider = new ItemProvider();
            var item = itemProvider.BuildItem(quality , key.Item , key.Effects  );
            _Player.Bag.Add(item);

        }
        

        void IMakeSkill.Exit()
        {
            DoneEvent();
        }

        void IMakeSkill.QueryFormula()
        {
            _FormulasEvent(_BuildLite(_Formulas));
        }

        private event Action<ItemFormulaLite[]> _FormulasEvent;
        event Action<ItemFormulaLite[]> IMakeSkill.FormulasEvent
        {
            add { _FormulasEvent += value; }
            remove { _FormulasEvent -= value; }
        }
    }
}