using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

	public List<Item> itemList;
	// Use this for initialization

	void Awake()
	{
		itemList = new List<Item>();
		itemList.Add (new Item (1, ItemType.Weapon, "Pistol", 54, true));
		itemList.Add (new Item (2, ItemType.Weapon, "Rifle", 100, false));
		itemList.Add (new Item (3, ItemType.Weapon, "MachineGun", 200, false));
	}

	public void SetActiveByName(System.String itemName)
	{
		foreach (var item in itemList)
		{
			if (item.itemName == itemName)
				item.isActive = true;
			else 
				item.isActive = false;
		}
	}

	public void SetActiveByID(int itemID)
	{
		foreach (var item in itemList)
		{
			item.isActive = false;
			if (item.itemID == itemID)
				item.isActive = true;
		}
	}

	public void AddItem(Item newItem)
	{
		itemList.Add (newItem);
	}

	public int GetValueByName(System.String itemName)
	{
		foreach (var item in itemList)
		{
			if (item.itemName == itemName)
				return item.itemValue;
		}
		return -1337;
	}

	public bool SetValueByName(System.String itemName, int itemValue)
	{
		foreach (var item in itemList)
		{
			if (item.itemName == itemName)
			{
				item.itemValue = itemValue;
				return true;
			}
		}
		return false;
	}

	public int GetActiveItemValue ()
	{
		foreach (var item in itemList)
		{
			if (item.isActive)
			{
				return item.itemValue;
			}
		}
		return -1337;
	}

	public System.String GetActiveItemName()
	{
		foreach (var item in itemList)
		{
			if (item.isActive)
			{
				return item.itemName;
			}
		}
		return "No active item found";
	}

	public bool DeleteItemByName(System.String itemName)
	{
		foreach (var item in itemList) 
		{
			if (item.itemName == itemName)
			{
				itemList.Remove(item);
				return true;
			}
		}
		return false;
	}

	public bool IsExist (System.String itemName)
	{
		foreach (var item in itemList)
		{
			if (item.itemName == itemName)
			{
				return true;
			}
		}
		return false;
	}
}
