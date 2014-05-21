using UnityEngine;
using System.Collections;

public class CraftingBook : MonoBehaviour {

	public float boxRadius = 5f;
	public float headHeight = 20f;
	public float boxWidth = 280f;
	public float bottombuffer = 30f;
	public float recipeHeight = 20f;
	public float scrollBarWidth = 20f;

	public Vector2 scrollPosition = Vector2.zero;

	private Recipes recipes;

	// Use this for initialization
	void Start () {
		recipes = GameObject.FindGameObjectWithTag ("Recipes").GetComponent<Recipes>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI()
	{
		//print ("RNum: " + recipes.recipes.Count);//check foreach
		Vector2 invLoc = new Vector2 ((Screen.width - boxRadius - boxWidth), boxRadius);
		Vector2 invDim = new Vector2 ((Screen.width - invLoc.x - boxRadius), 
		                              (Screen.height / 2 - (2 * boxRadius)) - bottombuffer);
		Rect invRect = new Rect (invLoc.x, invLoc.y, invDim.x, invDim.y);

		float listHeight = (recipes.recipes.Count * (recipeHeight + boxRadius)) - boxRadius;

		scrollPosition = GUI.BeginScrollView(new Rect(invLoc.x + boxRadius, invLoc.y + boxRadius + headHeight, 
		                                              invDim.x - (2 * boxRadius), invDim.y - (2 * boxRadius) - headHeight), 
		                                     scrollPosition, 
		                                     new Rect(0, 0, invDim.x - (2 * boxRadius) - scrollBarWidth, listHeight));
		int currRecipe = 0;
		foreach(Recipe recipe in recipes.recipes)
		{
			//print(recipe.result);
			Rect recipeRect = new Rect(invLoc.x + boxRadius, 
			                           invLoc.y + boxRadius + headHeight + (currRecipe * (recipeHeight + boxRadius)),
			                           invDim.x - (2 * boxRadius),
			                           recipeHeight);
			//print(recipeRect.ToString());
			GUI.Button(new Rect(0, 
			                    currRecipe * (recipeHeight + boxRadius),
			                    invDim.x - (2 * boxRadius),
			                    recipeHeight), recipe.result);
			currRecipe++;
		}
		/*
		GUI.Button(new Rect(0, 0, 100, 20), "Top-left");
		GUI.Button(new Rect(120, 0, 100, 20), "Top-right");
		GUI.Button(new Rect(0, 180, 100, 20), "Bottom-left");
		GUI.Button(new Rect(120, 180, 100, 20), "Bottom-right");
		*/
		GUI.EndScrollView();

		GUI.Box (invRect, "Recipes");
	}
}
