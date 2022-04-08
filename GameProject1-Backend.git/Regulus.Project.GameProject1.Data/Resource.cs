using System;
using System.Linq;
namespace Regulus.Project.GameProject1.Data
{
    public class Resource 
    {
        
        public EntityData[] Entitys;
        public SkillData[] SkillDatas;

        public ItemPrototype[] Items;

        public ItemFormula[] Formulas;

        public EntityGroupLayout[] EntityGroupLayouts;

        public static Resource Instance { get { return 
                    Regulus.Utility.Singleton<Resource>.Instance
                    ; } }    

        public Resource()
        {
            
            EntityGroupLayouts = new EntityGroupLayout[0];
            Entitys = new EntityData[0];
            SkillDatas = new SkillData[0];
            Items = new ItemPrototype[0];
            Formulas = new ItemFormula[0];
        }
        
        public EntityData FindEntity(ENTITY name)
        {
            return (from e in this.Entitys where e.Name == name select e).Single();
        }

        public SkillData FindSkill(ACTOR_STATUS_TYPE name)
        {
            return (from e in this.SkillDatas where e.Id == name select e).Single();
        }

        public ItemPrototype FindItem(string name)
        {
            return (from e in this.Items where e.Id == name select e).Single();
        }

        public EntityGroupLayout FindEntityGroupLayout(string id)
        {
            return (from e in this.EntityGroupLayouts where e.Id == id select e).Single();
        }
    }
}