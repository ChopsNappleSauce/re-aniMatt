using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Recipe {
	public string result;
	public int resultCode;
	public int resultQuantity;
	public List<string> ingredients;
	public List<int> codes;
	public List<int> quantities;

	public Recipe(string res, int resCode, int resQuant, List<string> ing, List<int> code, List<int> quant)
	{
		result = res;
		resultCode = resCode;
		resultQuantity = resQuant;
		ingredients = ing;
		codes = code;
		quantities = quant;
	}

	public Recipe() {}
}
