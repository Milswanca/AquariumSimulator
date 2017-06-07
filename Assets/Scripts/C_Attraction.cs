using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Attraction : MonoBehaviour 
{
	//defines how interested people will be in this aquarium
	public virtual int AttractionRating 
	{ 
		get
		{
			return BaseAttractionRating;
		}
	}

	public int BaseAttractionRating;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
