using UnityEngine;
using System;
using System.Collections;

public class gui : MonoBehaviour {

	public float boxRadius = 10f;
	public float headHeight = 25f;
	public int invRows = 5;
	public float invSide = 40f;

	private bool[,] invButtons;
	private string lastClicked;
	private Texture[] gridTextures;
	private static int gridSelected;
	private bool updateInv;

	void Start () {
		invButtons = new bool[invRows, invRows];
		gridTextures = new Texture[invRows*invRows];
		lastClicked = "";
		updateInv = false;
	}

	public void setTexture(Texture texture, int coord)
	{
		gridTextures [coord] = texture;
	}

	public void updateInventory()
	{
		updateInv = true;
	}

	// Update is called once per frame
	void OnGUI() {
		//GridManager gm = GameObject.Find ("Main Camera").GetComponent<GridManager>();
		Tom tom = GameObject.FindGameObjectWithTag ("Player").GetComponent<Tom>();
		Vector2[,] inv = tom.getInventory ();

		Vector2 invDim = new Vector2((boxRadius + (invRows * (invSide + boxRadius))),
		                             (headHeight + boxRadius + (invRows * (invSide + boxRadius))));
		Vector2 invLoc = new Vector2 ((Screen.width - boxRadius - invDim.x),
		                              (Screen.height - boxRadius - invDim.y));
		GUI.Box (new Rect (invLoc.x, invLoc.y, invDim.x, invDim.y), "Inventory");
		float currX = invLoc.x + boxRadius;
		float currY = 0;
		for(int i = 0; i < invRows; i++)
		{
			currY = invLoc.y + boxRadius + headHeight;
			for(int j = 0; j < invRows; j++)
			{
				GUI.SetNextControlName("inv" + i + ":" + j);
				invButtons[i,j] = GUI.Button(new Rect(currX, currY, invSide, invSide), "");
				currY += (invSide + boxRadius);
			}
			currX += (invSide + boxRadius);
		}

		if(updateInv)
		{
		for(int i = 0; i < invRows; i++)
		{
			for(int j = 0; i < invRows; i++)
			{
				switch((int)inv[i,j].x)
				{
				case 0:
					break;
				case 1:
					Debug.Log("Stick at " + i + ":" + j);
					int coord = (j * invRows) + i;
					gridTextures[coord] = (Texture)Resources.Load("Grass&Rock");
					break;
				default:
					break;
				}
			}
		}
		}

		//Texture.
		GUI.SelectionGrid (new Rect (5, 100, 250, 250), 500, gridTextures, invRows);
		//Debug.Log(GUI.GetNameOfFocusedControl ());
		/*
		if(GUI.GetNameOfFocusedControl() != lastClicked)
		{
			lastClicked = GUI.GetNameOfFocusedControl;
			Debug.Log(lastClicked);
		}
		*/

		GUI.TextArea(new Rect(5, Screen.height - 25, 85, 20), "Health: " + tom.getHealth().ToString());
		GUI.TextArea (new Rect(5, Screen.height - 50, 85, 20), "Ammo: " + tom.getAmmo().ToString());
		GUI.TextArea (new Rect(5, 5, 85, 20), "Kills: " + tom.getKills().ToString());
		GUI.TextArea (new Rect(Screen.width - 90, 5, 85, 20), "Time: " + tom.getTime().ToString());

		//bool testButtonTwo = false;
		if (GameObject.FindGameObjectWithTag ("Player").GetComponent<Tom>().getHealth() <= 0) 
		{
			GUIStyle myButtonStyle = new GUIStyle(GUI.skin.button);
			myButtonStyle.fontSize = 75;

			myButtonStyle.normal.textColor = Color.red;
			myButtonStyle.hover.textColor = Color.red;

			int time = tom.getTime();

			if(GUI.Button(new Rect(Screen.width/2-400, Screen.height/2-100, 800, 200), "Game Over!\n Kills: " 
			              + tom.getKills ().ToString() + ", Time: " + time.ToString(), myButtonStyle))
				Application.LoadLevel("Z_scene");
		}
	}
}
