using UnityEngine;
using System.Collections;

[System.Serializable]
public class Item {
	public string itemName;
	public int itemID;
	public string itemDesc;
	public Texture2D itemIcon;
	public int itemPower;
	public int itemSpeed;
	public ItemType itemType;

	public enum ItemType {
		Weapon,
		Tool,
		Resource,
		Consumable
	}

	public Item(string name, int ID, string desc, int power, int speed, ItemType type)
	{
		itemName = name;
		itemID = ID;
		itemDesc = desc;
		itemIcon = Resources.Load<Texture2D> (type.ToString () + " Assets/" + name);
		itemPower = power;
		itemSpeed = speed;
		itemType = type;
	}

	public Item()
	{

	}
}
