using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {
	public GUISkin skin;
	public int slotsX, slotsY;
	public float boxRadius = 5f;
	public float headHeight = 15f;
	public int invRows = 5;
	public float invSide = 50f;
	public float grabRadius = 3f;
	public List<Item> inventory = new List<Item>();
	public List<Item> slots = new List<Item> ();
	private ItemDatabase database;
	private bool showInv;
	private bool showToolTip;
	private string toolTip;
	private Rect invRect;
	private bool mouseOnInv;
	private bool draggingItem;
	private Item draggedItem;
	private int prevIndex;

	// Use this for initialization
	void Start () {
		for(int i = 0; i < (invRows * invRows); i++)
		{
			slots.Add(new Item());
			inventory.Add(new Item());
		}
		database = GameObject.FindGameObjectWithTag ("ItemDatabase").GetComponent<ItemDatabase>();
		AddItem (13);
		AddItem (7);
		AddItem (8);

		invRect = new Rect ();
		mouseOnInv = false;
		showInv = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Inventory"))
		{
			showInv = !showInv;
		}

		if(Input.GetKeyDown(KeyCode.R))
		{
			RemoveItem(13);
		}

		if(Input.GetKey(KeyCode.Space))
		{
			//Get all sticks and pick up ones close enough
			GameObject[] sticks = GameObject.FindGameObjectsWithTag("stick");
			foreach(GameObject stick in sticks)
			{
				float xSDif = stick.transform.position.x - this.transform.position.x;
				float ySDif = stick.transform.position.z - this.transform.position.z;
				float sDist = Mathf.Sqrt(Mathf.Pow(xSDif, 2f) + Mathf.Pow(ySDif, 2f));
				if(sDist < grabRadius)
				{
					bool foundSpot = false;
					if(AddItem(13))
						foundSpot = true;
					
					if(foundSpot)
					{
						Destroy(stick);
					}
				}
			}

			GameObject[] rocks = GameObject.FindGameObjectsWithTag("rock");
			foreach(GameObject rock in rocks)
			{
				float xSDif = rock.transform.position.x - this.transform.position.x;
				float ySDif = rock.transform.position.z - this.transform.position.z;
				float sDist = Mathf.Sqrt(Mathf.Pow(xSDif, 2f) + Mathf.Pow(ySDif, 2f));
				if(sDist < grabRadius)
				{
					bool foundSpot = false;
					if(AddItem(2))
						foundSpot = true;
					
					if(foundSpot)
					{
						Destroy(rock);
					}
				}
			}

			GameObject[] flaxes = GameObject.FindGameObjectsWithTag("flax");
			foreach(GameObject flax in flaxes)
			{
				float xSDif = flax.transform.position.x - this.transform.position.x;
				float ySDif = flax.transform.position.z - this.transform.position.z;
				float sDist = Mathf.Sqrt(Mathf.Pow(xSDif, 2f) + Mathf.Pow(ySDif, 2f));
				if(sDist < grabRadius)
				{
					bool foundSpot = false;
					if(AddItem(5))
						foundSpot = true;
					
					if(foundSpot)
					{
						Destroy(flax);
					}
				}
			}

			GameObject[] feathers = GameObject.FindGameObjectsWithTag("feather");
			foreach(GameObject feather in feathers)
			{
				float xSDif = feather.transform.position.x - this.transform.position.x;
				float ySDif = feather.transform.position.z - this.transform.position.z;
				float sDist = Mathf.Sqrt(Mathf.Pow(xSDif, 2f) + Mathf.Pow(ySDif, 2f));
				if(sDist < grabRadius)
				{
					bool foundSpot = false;
					if(AddItem(9))
						foundSpot = true;
					
					if(foundSpot)
					{
						Destroy(feather);
					}
				}
			}
		}
	}

	void OnGUI()
	{
		showToolTip = false;
		toolTip = "";

		GUI.skin = skin;
		if(showInv)
		{
			DrawInventory();
			if(showToolTip)
			{
				GUI.Box(new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, 200, 200), toolTip,
				        skin.GetStyle("Tooltip"));
			}
		}
		else
		{
			mouseOnInv = false;
		}

		if(draggingItem)
		{
			GUI.DrawTexture(new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, invSide, invSide),
			                draggedItem.itemIcon);
		}
	}

	void DrawInventory()
	{
		Event e = Event.current;

		Vector2 invDim = new Vector2((boxRadius + (invRows * (invSide + boxRadius))),
		                             (headHeight + boxRadius + (invRows * (invSide + boxRadius))));
		Vector2 invLoc = new Vector2 ((Screen.width - boxRadius - invDim.x),
		                              (Screen.height - boxRadius - invDim.y));
		invRect = new Rect (invLoc.x, invLoc.y, invDim.x, invDim.y);
		GUI.Box (invRect, "<size=14>Inventory</size>");
		if (invRect.Contains (e.mousePosition))
				mouseOnInv = true;
		else
				mouseOnInv = false;

		float currX = invLoc.x + boxRadius;
		float currY = invLoc.y + boxRadius + headHeight;
		for(int y = 0; y < invRows; y++)
		{
			for(int x = 0; x < invRows; x++)
			{
				int flatIndex = (y * invRows) + x;
				Rect slotRect = new Rect(currX, currY, invSide, invSide);
				GUI.Box(slotRect, "", skin.GetStyle("Slot"));
				slots[flatIndex] = inventory[flatIndex];
				if(slots[flatIndex].itemName != null)
				{
					GUI.DrawTexture(slotRect, slots[flatIndex].itemIcon);
					if(slotRect.Contains(e.mousePosition))
					{
						toolTip = CreateToolTip(slots[flatIndex]);
						showToolTip = true;

						if(e.button == 0 && e.type == EventType.MouseDrag && !draggingItem)
						{
							draggingItem = true;
							prevIndex = flatIndex;
							draggedItem = slots[flatIndex];
							inventory[flatIndex] = new Item();
						}

						if(e.button == 0 && e.type == EventType.MouseUp && draggingItem)
						{
							inventory[prevIndex] = inventory[flatIndex];
							inventory[flatIndex] = draggedItem;
							draggingItem = false;
							draggedItem = null;
						}
					}
				}
				else
				{
					if(slotRect.Contains(e.mousePosition))
					{
						if(e.button == 0 && e.type == EventType.MouseUp && draggingItem)
						{
							inventory[flatIndex] = draggedItem;
							draggingItem = false;
							draggedItem = null;
						}
					}
				}

				currX += (invSide + boxRadius);
			}
			currY += (invSide + boxRadius);
			currX = invLoc.x + boxRadius;
		}
	}

	string CreateToolTip(Item item)
	{
		toolTip = "<color=#FFFFFF>" + item.itemName + "</color>\n\n" + 
			"<color=#BFC272>" + item.itemDesc + "</color>";
		return toolTip;
	}

	bool AddItem(int ID)
	{
		for(int i = 0; i < inventory.Count; i++)
		{
			if(inventory[i].itemName == null)
			{
				for(int j = 0; j < database.items.Count; j++)
				{
					if(database.items[j].itemID == ID)
					{
						inventory[i] = database.items[j];
						return true;
					}
				}
				break;
			}
		}
		return false;
	}

	bool RemoveItem(int ID)
	{
		for(int i = 0; i < inventory.Count; i++)
		{
			if(inventory[i].itemID == ID)
			{
				inventory[i] = new Item();
				return true;
			}
		}
		return false;
	}

	bool Contains(int ID)
	{
		for(int i = 0; i < inventory.Count; i++)
		{
			if(inventory[i].itemID == ID)
				return true;
		}
		return false;
	}

	public bool cursorInInv()
	{
		return mouseOnInv;
	}
}
