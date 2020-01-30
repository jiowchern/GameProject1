using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Regulus.Project.GameProject1.Data
{
    public class ItemFormula
    {
        public ItemFormulaNeed[] NeedItems;
        public ItemEffect[] Effects;

        public string Id;
        public string Item;

        public int NeedLimit;

        public ItemFormula()
        {
            NeedItems = new ItemFormulaNeed[0];
            Effects = new ItemEffect[0];
        }
    }

    
    public class ItemFormulaLite
    {
    
        public string Id;
    
        public string Item;
    
        public ItemFormulaNeedLite[] NeedItems;
    
        public int NeedLimit;

        public ItemFormulaLite()
        {
            NeedItems = new ItemFormulaNeedLite[0];            
        }
    }
}
