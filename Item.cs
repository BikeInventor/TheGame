
using System;

	public enum ItemType
	{
		Weapon,
		NotWeapon
	}
	public class Item
	{
		public int itemID;
		public String itemName;
		public int itemValue;
	    public ItemType itemType;
	    public bool isActive;

		public Item (int _itemID, ItemType _itemType, String _itemName, int _itemValue, bool _isActive)
		{
			this.itemID = _itemID;
			this.itemType = _itemType;
			this.itemName = _itemName;
			this.itemValue = _itemValue;
			this.isActive = _isActive;
		}

	}

