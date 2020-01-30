using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Regulus.Project.GameProject1.Data
{
    [Serializable]
    public struct ItemPrototype
    {
        public string Id;
        
        public ITEM_FEATURES Features;            

        public string Description;

        public EQUIP_PART EquipPart;
    }
}
