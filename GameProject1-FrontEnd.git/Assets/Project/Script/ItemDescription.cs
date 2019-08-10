using System;


using UnityEngine;
using System.Collections;

using Regulus.Project.GameProject1.Data;

public class ItemDescription : MonoBehaviour {

    Client _Client;

    public UnityEngine.UI.Text Name;
    public UnityEngine.UI.Text Effect;

    private IInventoryController _Controller;
    

    private System.Guid _Id ;

    void OnDestroy()
    {
        if (_Client != null)
        {
            _Client.User.InventoryControllerProvider.Supply -= _SetController;            
        }
    }

    

    void Start () {
        _Client = Client.Instance;
        if (_Client != null)
        {
            _Client.User.InventoryControllerProvider.Supply += _SetController;            
        }
    }

    private void _SetController(IInventoryController obj)
    {
        _Controller = obj;
    }

    
    void Update ()
    {
	        
	}

    public void Set(Item item)
    {
        _Id = item.Id;
        Name.text = item.Name;
        string effectText = "";

        if(item.Effects != null)
        {
            foreach (var effect in item.Effects)
            {
                effectText += effect.Type.ToString() + ":" + effect.Value;
            }
            Effect.text = effectText;
        }
        else
        {
            Effect.text = "";
        }

    }

    public void Unequip()
    {
        _Controller.Unequip(_Id);
    }
    public void Equip()
    {
        _Controller.Equip(_Id);
    }
    public void Discard()
    {
        _Controller.Discard(_Id);
    }

    public void Use()
    {
        _Controller.Use(_Id);
    }
}
