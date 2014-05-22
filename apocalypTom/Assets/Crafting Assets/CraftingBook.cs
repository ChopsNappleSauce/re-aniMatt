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
	private bool showBook;
	private bool showToolTip;
	private string toolTip;

	// Use this for initialization
	void Start () {
		recipes = GameObject.FindGameObjectWithTag ("Recipes").GetComponent<Recipes>();
		showBook = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.C))
		{
			showBook = !showBook;
		}
	}

	void OnGUI()
	{
		showToolTip = false;
		toolTip = "";

		if(showBook)
		{
			DrawBook ();
			if(showToolTip)
			{
				GUI.Box(new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, 175, 150), toolTip);
			}
		}
	}

	void DrawBook()
	{
		Event e = Event.current;

		Vector2 invLoc = new Vector2 ((Screen.width - boxRadius - boxWidth), boxRadius);
		Vector2 invDim = new Vector2 ((Screen.width - invLoc.x - boxRadius), 
		                              (Screen.height / 2 - (2 * boxRadius)) - bottombuffer);
		Rect invRect = new Rect (invLoc.x, invLoc.y, invDim.x, invDim.y);
		GUI.Box (invRect, "Recipes");
		
		float listHeight = (recipes.recipes.Count * (recipeHeight + boxRadius)) - boxRadius;
		
		scrollPosition = GUI.BeginScrollView(new Rect(invLoc.x + boxRadius, invLoc.y + boxRadius + headHeight, 
		                                              invDim.x - (2 * boxRadius), invDim.y - (2 * boxRadius) - headHeight), 
		                                     scrollPosition, 
		                                     new Rect(0, 0, invDim.x - (2 * boxRadius) - scrollBarWidth, listHeight));
		int currRecipe = 0;
		foreach(Recipe recipe in recipes.recipes)
		{
			Rect recipeRect = new Rect(0, currRecipe * (recipeHeight + boxRadius),
			                           invDim.x - (2 * boxRadius),
			                           recipeHeight);
			GUI.Button(recipeRect, recipe.result);

			if(recipeRect.Contains(e.mousePosition))
			{
				toolTip = CreateToolTip(recipe);
				//print(toolTip);
				showToolTip = true;
			}
				
				currRecipe++;
		}
		GUI.EndScrollView();
	}

	string CreateToolTip(Recipe recipe)
	{
		toolTip = "<color=#FFFFFF>Required items:</color>\n\n<color=#BFC272>";

		foreach(string ingredient in recipe.ingredients)
		{
			toolTip += ingredient + "\n";
		}

		toolTip += "</color>";
		return toolTip;
	}
}