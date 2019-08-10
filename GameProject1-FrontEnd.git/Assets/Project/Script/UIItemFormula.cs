using System;
using System.Linq;


using UnityEngine;
using System.Collections;


using Regulus.Project.GameProject1.Data;

public class UIItemFormula : MonoBehaviour
{

    public UnityEngine.UI.Text Name;
    public UnityEngine.UI.Text Item;
    public UnityEngine.UI.Image ItemImage;

    
    public UnityEngine.UI.Image[] MaterialImages;
    public UnityEngine.UI.Text[] MaterialAmountTexts;
    public UnityEngine.UI.Slider[] MaterialAmounts;

    public Action<string, int[]> MakeEvent;
    
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    for(int i = 0; i < MaterialAmountTexts.Length; i++)
	    {
            MaterialAmountTexts[i].text = ((int)MaterialAmounts[i].value).ToString();
	    }
	}

    public void Set(ItemFormulaLite formula_lite)
    {
        
        Name.text = formula_lite.Id;
        Item.text = formula_lite.Item;
        ItemImage.sprite = (Sprite)UnityEngine.Resources.Load("Icon/Item/" + formula_lite.Item , typeof (Sprite));
        int i = 0;
        for (; i < formula_lite.NeedItems.Length ; i++)
        {
            var item = formula_lite.NeedItems[i];
            
            MaterialAmounts[i].minValue = item.Min;
            MaterialAmounts[i].maxValue = formula_lite.NeedLimit;
            MaterialAmountTexts[i].text = item.Min.ToString();
            MaterialImages[i].sprite = (Sprite)UnityEngine.Resources.Load("Icon/Item/" + item.Item , typeof(Sprite));
        }
        for(; i < MaterialAmountTexts.Length ; i++)
        {
            
            MaterialAmounts[i].enabled = false;            
            MaterialAmountTexts[i].enabled = false;
            MaterialImages[i].enabled = false;
        }
    }

    public void Make()
    {
        var amounts = (from amount in MaterialAmounts where amount.enabled select (int)amount.value).ToArray();
        MakeEvent(Name.text , amounts);
    }
}
