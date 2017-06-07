using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_PointOfInterest : C_Attraction 
{
	public List<C_Attraction> Attractions;

	public override int AttractionRating 
	{
		get 
		{
			int totalAttraction = BaseAttractionRating;

			foreach(C_Attraction i in Attractions)
			{
				totalAttraction += i.AttractionRating;
			}	

			return totalAttraction;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public bool AddAttraction(C_Attraction attraction)
	{
		if(attraction == null) { return false; }

		Attractions.Add (attraction);

		Debug.Log (AttractionRating);

		return true;
	}
}
