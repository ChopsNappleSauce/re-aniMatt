using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Recipe {
	public string result;
	public List<string> ingredients;
	public List<int> codes;

	public Recipe(string res, List<string> ing, List<int> code)
	{
		result = res;
		ingredients = ing;
		codes = code;
	}

	public Recipe() {}
}
