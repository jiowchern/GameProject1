using System;
using System.Collections.Generic;
using System.Linq;

using Regulus.CustomType;
using Regulus.Project.GameProject1.Data;


namespace Regulus.Project.GameProject1.Game.Play
{
    public class Equipment : IEquipmentNotifier
    {
        private readonly Entity _Entity;

        private readonly Dictionary<EQUIP_PART, Item> _Items;

        public event Action<Item> AddEvent;
        public event Action<Guid> RemoveEvent;

        private readonly List<Skill> _Skills;
        public Equipment(Entity entity)
        {            
            this._Entity = entity;
            this._Items = new Dictionary<EQUIP_PART, Item>();

            _Skills = new List<Skill>();
            _Skills.Add(new Skill(EFFECT_TYPE.SKILL_MELEE1, ITEM_FEATURES.NONE, ITEM_FEATURES.NONE, ACTOR_STATUS_TYPE.MELEE_IDLE , ACTOR_STATUS_TYPE.DAMAGE1 , ACTOR_STATUS_TYPE.KNOCKOUT1));
            _Skills.Add(new Skill(EFFECT_TYPE.SKILL_AXE1 ,ITEM_FEATURES.AXE , ITEM_FEATURES.NONE, ACTOR_STATUS_TYPE.BATTLE_AXE_IDLE, ACTOR_STATUS_TYPE.DAMAGE1, ACTOR_STATUS_TYPE.KNOCKOUT1));
            _Skills.Add(new Skill(EFFECT_TYPE.SKILL_CLAYMORE1, ITEM_FEATURES.CLAYMORE, ITEM_FEATURES.NONE, ACTOR_STATUS_TYPE.SWORD_IDLE, ACTOR_STATUS_TYPE.DAMAGE1, ACTOR_STATUS_TYPE.KNOCKOUT1));
            _Skills.Add(new Skill(EFFECT_TYPE.SKILL_DUALSWORD1, ITEM_FEATURES.SWORD, ITEM_FEATURES.SWORD, ACTOR_STATUS_TYPE.SWORD_IDLE, ACTOR_STATUS_TYPE.DAMAGE1, ACTOR_STATUS_TYPE.KNOCKOUT1));
            _Skills.Add(new Skill(EFFECT_TYPE.SKILL_SWORD1, ITEM_FEATURES.SWORD, ITEM_FEATURES.NONE, ACTOR_STATUS_TYPE.SWORD_IDLE, ACTOR_STATUS_TYPE.SWORD_INJURY, ACTOR_STATUS_TYPE.KNOCKOUT1));
            _Skills.Add(new Skill(EFFECT_TYPE.SKILL_SWORDSHIELD1, ITEM_FEATURES.SHIELD, ITEM_FEATURES.SWORD, ACTOR_STATUS_TYPE.SWORD_IDLE, ACTOR_STATUS_TYPE.DAMAGE1, ACTOR_STATUS_TYPE.KNOCKOUT1));



        }

        public Item[] Unequip(EQUIP_PART equip_type)
        {
            Item item;
            _Items.TryGetValue(equip_type, out item);
            if (Item.IsValid(item))
            {
                _Items.Remove(equip_type);
                RemoveEvent(item.Id);
            }
            if (equip_type == EQUIP_PART.RIGHT_HAND)
            {
                Item leftItem;
                if (_Items.TryGetValue(EQUIP_PART.LEFT_HAND, out leftItem) && Item.IsValid(leftItem))
                {
                    _Items.Remove(EQUIP_PART.LEFT_HAND);
                    RemoveEvent(leftItem.Id);
                    return new[] { item, leftItem };
                }                
            }

            return new [] { item } ;
        }

        public bool Equip(Item item)
        {

            var part = item.GetEquipPart();
            if (_Items.ContainsKey(part) == false)
            {
                _Items.Add(part, item);
                AddEvent(item);
                return true;
            }

            var rightItem = _Items.FirstOrDefault(i => i.Key == EQUIP_PART.RIGHT_HAND);            
            if (part == EQUIP_PART.RIGHT_HAND  
                && _Items.ContainsKey(EQUIP_PART.LEFT_HAND) == false
                && rightItem.Value.GetPrototype().Features != ITEM_FEATURES.CLAYMORE)
            {
                _Items.Add(EQUIP_PART.LEFT_HAND, item);
                AddEvent(item);

                return true;
            }

            

            return false;
        }


        public Item Find(EQUIP_PART part)
        {
            if (_Items.ContainsKey(part))
                return _Items[part];

            return null;
        }
        public Item[] GetItems()
        {
            return _Items.Values.ToArray();
        }
        

        public Item[] Unequip(Guid id)
        {
            var part = (from itemPart in _Items
                        where itemPart.Value.Id == id
                        select itemPart.Key).FirstOrDefault();
            return Unequip(part);
        }

        public void UpdateEffect(float last_delta_time)
        {
            var view = 0.0f;
            var parts = Regulus.Utility.EnumHelper.GetEnums<EQUIP_PART>();

            foreach (var part in parts)
            {
                if (_Items.ContainsKey(part))
                {
                    var item = _Items[part];
                    if (item.UpdateLife(last_delta_time))
                    {
                        foreach (var effect in item.Effects)
                        {
                            if (effect.Type == EFFECT_TYPE.ILLUMINATE_ADD)
                            {
                                view += effect.Value;
                            }
                        }
                        _Items[part] = item;
                    }
                }


            }

            _Entity.SetEquipView(view);
        }

        public Skill GetSkill()
        {
            var rightItem = Find(EQUIP_PART.RIGHT_HAND);
            var leftItem = Find(EQUIP_PART.LEFT_HAND);
            if (rightItem != null)
            {
                var rightFeatures = rightItem.GetPrototype().Features;
                ITEM_FEATURES leftFeatures = ITEM_FEATURES.NONE;
                if (leftItem != null)
                    leftFeatures = leftItem.GetPrototype().Features;
                var effect = rightItem.Effects.FirstOrDefault( e=> _Skills.Any( s => s.Effect == e.Type));

                var val = (from skill in _Skills where skill.Conform(effect.Type, rightFeatures, leftFeatures) select skill).FirstOrDefault();
                if (val != default(Skill))
                    return val;                
            }
            return _Skills[0];
        }

        public EquipStatus[] GetStatus()
        {
            var status = new EquipStatus[_Items.Count];
            int i = 0;
            foreach (var item in _Items)
            {
                status[i++] = new EquipStatus() { Item = item.Value.Name , Part = item.Key};
            }

            return status;
        }
    }

    public class Skill
    {
        private readonly EFFECT_TYPE _SkillEffect;

        private readonly ACTOR_STATUS_TYPE _Idle;

        private readonly ACTOR_STATUS_TYPE _Injury;

        private readonly ACTOR_STATUS_TYPE _Knockout; 

        private readonly ITEM_FEATURES _Right;

        private readonly ITEM_FEATURES _Left;

        public Skill(EFFECT_TYPE skill_effect, ITEM_FEATURES right, ITEM_FEATURES left , ACTOR_STATUS_TYPE idle , ACTOR_STATUS_TYPE injury, ACTOR_STATUS_TYPE knockout)
        {

            _SkillEffect = skill_effect;
            _Idle = idle;
            _Right = right;
            _Left = left;
            _Knockout = knockout;
            _Injury = injury;
        }

        public ACTOR_STATUS_TYPE Idle { get {return _Idle;} }

        public EFFECT_TYPE Effect { get { return _SkillEffect; } }

        public ACTOR_STATUS_TYPE Injury { get { return _Injury; } }

        public ACTOR_STATUS_TYPE Knockout { get { return _Knockout; } }

        public bool Conform(EFFECT_TYPE effect, ITEM_FEATURES right_features, ITEM_FEATURES left_features)
        {
            return effect == _SkillEffect && right_features == _Right && left_features == _Left;
        }
    }
}
