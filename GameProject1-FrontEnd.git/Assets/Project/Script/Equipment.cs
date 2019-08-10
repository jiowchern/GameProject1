using System;


using UnityEngine;
using System.Collections;


using Regulus.Project.GameProject1.Data;

public class Equipment : Inventory {

	private Client _Client;

	private IEquipmentNotifier _Notifier;
	private IInventoryController _InventoryController;
	void OnDestroy()
	{
		if (_Client != null)
		{
			_ReleaseNotifier();
			_ReleaseController();
			_Client.User.InventoryControllerProvider.Supply -= _SetInventoryController;
			_Client.User.EquipmentNotifierProvider.Supply -= EquipmentNotifierProviderOnSupply;
		}
	}

	private void _ReleaseNotifier()
	{
		if (_Notifier != null)
		{
			_Notifier.RemoveEvent -= _RemoveItem;
			_Notifier.AddEvent -= _AddEvent;
		}
	}

	private void _ReleaseController()
	{
		if (_InventoryController != null)
		{
			_InventoryController.EquipItemsEvent -= _Reflush;
		}
	}

	// Use this for initialization
	void Start () {
		_Client = Client.Instance;
		if (_Client != null)
		{
			_Client.User.EquipmentNotifierProvider.Supply += EquipmentNotifierProviderOnSupply;
			_Client.User.InventoryControllerProvider.Supply += _SetInventoryController;
		}
	}
	private void _SetInventoryController(IInventoryController obj)
	{
		_ReleaseController();
		_InventoryController = obj;
		_InventoryController.EquipItemsEvent += _Reflush;
		_InventoryController.Refresh();
	}
	private void EquipmentNotifierProviderOnSupply(IEquipmentNotifier equipment_notifier)
	{
		_ReleaseNotifier();
		_Notifier = equipment_notifier;
		
		
		_Notifier.RemoveEvent += _RemoveItem;
		_Notifier.AddEvent += _AddEvent;        
	}

	// Update is called once per frame
	void Update () {
	
	}
}
