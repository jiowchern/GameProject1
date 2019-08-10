using System;

using UnityEngine;
using System.Collections;

using Regulus.Project.GameProject1.Data;



public class GameItem : MonoBehaviour
{
    public UnityEngine.UI.Image Image;
    public UnityEngine.UI.Text Name;
    private Regulus.Project.GameProject1.Data.Item _Item;

    public Guid Id {
        get { return _Item.Id; }
    }

    void OnDestroy()
    {
        
    }
    void Start () {
        
    }

    private void InventoryNotifierProvider_Supply(Regulus.Project.GameProject1.Data.IInventoryNotifier obj)
    {
        
        
        
        
    }

    

    // Update is called once per frame
    public void Update () {
	
	}

    public void Set(Regulus.Project.GameProject1.Data.Item item)
    {
        _Item = item;
        Name.text =  item.Count.ToString();
         
        Image.sprite = (Sprite)UnityEngine.Resources.Load("Icon/Item/" + _Item.Name , typeof(Sprite));
    }
}
