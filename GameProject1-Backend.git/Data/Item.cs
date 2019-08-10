using System;
using System.Linq;

namespace Regulus.Project.GameProject1.Data
{
    
    public class Item
    {
        public int RefCount;

        public Guid Id;


        public string Name;


        public int Weight;


        public Effect[] Effects;


        public int Count;


        public float Life;

        public Item()
        {
            Effects = new Effect[0];
        }

        public Item Clone()
        {
            var newItem = new Item();
            newItem.Id = Guid.NewGuid();
            newItem.Count = Count;            
            newItem.Effects = Regulus.Utility.ValueHelper.DeepCopy(Effects);
            newItem.Name = Name;
            newItem.Weight = Weight;
            return newItem;
        }

        public static bool IsValid(Item src)
        {            

            return src != null && src.Id != Guid.Empty && src.Count > 0 && src.Effects != null;
        }

        public ItemPrototype GetPrototype()
        {
            var result = Resource.Instance.FindItem(Name);
            return result;
        }

        public bool IsEquipable()
        {
            var i = GetPrototype();
            return i.EquipPart != EQUIP_PART.NONE;
        }

        public EQUIP_PART GetEquipPart()
        {
            var i = GetPrototype();
            return i.EquipPart;
        }

        public bool UpdateLife(float last_delta_time)
        {
            if (Life > 0)
            {
                Life -= last_delta_time;
            }
            return Life > 0;
        }

        public bool IsLife()
        {
            return Life > 0;
        }

        public float GetAid()
        {
            return (from e in Effects where e.Type == EFFECT_TYPE.AID select e.Value).Sum();
        }
    }
}