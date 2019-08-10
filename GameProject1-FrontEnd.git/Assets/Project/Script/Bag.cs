using System;

using UnityEngine;
using System.Collections;
using Regulus.Project.GameProject1;
using System.Linq;

using Regulus.Project.GameProject1.Data;
using Regulus.Remote;

using UnityEngine.UI;

public class Bag : Inventory
{

    
    private Client _Client;
    private Regulus.Project.GameProject1.Data.IInventoryNotifier _InventoryNotifier;

    private IInventoryController _InventoryController;

    // Use this for initialization
    void Start () {
        _Client = Client.Instance;
        if(_Client != null)
        {
            _Client.User.BagNotifierProvider.Supply += InventoryNotifierProvider_Supply;
            _Client.User.InventoryControllerProvider.Supply += _SetInventoryController;
        }
	}

    

    void OnDestroy()
    {
        if (_Client != null)
        {
            _ReleaseNotifier();
            _ReleaseController();
            _Client.User.BagNotifierProvider.Supply -= InventoryNotifierProvider_Supply;
            _Client.User.InventoryControllerProvider.Supply -= _SetInventoryController;
        }
    }

    private void _ReleaseNotifier()
    {
        if (_InventoryNotifier != null)
        {
            _InventoryNotifier.RemoveEvent -= _RemoveItem;
            _InventoryNotifier.AddEvent -= _AddEvent;
        }
    }

    private void _ReleaseController()
    {
        if (_InventoryController != null)
        {
            _InventoryController.BagItemsEvent -= _InventoryNotifier_AllItemEvent;
        }
    }

    private void InventoryNotifierProvider_Supply(Regulus.Project.GameProject1.Data.IInventoryNotifier obj)
    {
        _ReleaseNotifier();
        _InventoryNotifier = obj;
        
        _InventoryNotifier.RemoveEvent += _RemoveItem;
        _InventoryNotifier.AddEvent += _AddEvent;        
    }

    private void _SetInventoryController(IInventoryController obj)
    {
        _ReleaseController();
        _InventoryController = obj;
        _InventoryController.BagItemsEvent += _InventoryNotifier_AllItemEvent;
        _InventoryController.Refresh();
    }

    void OnEnable()
    {
        if(_InventoryController != null)
            _InventoryController.Refresh();
    }


    private void _InventoryNotifier_AllItemEvent(Regulus.Project.GameProject1.Data.Item[] items)
    {
        _Reflush(items);
    }

    

    

    

    

    // Update is called once per frame
    void Update () {
	
	}
}
