using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Aquarium : C_PointOfInterest 
{
	public C_FlockManager AquariumFlockManager;

	public Transform CameraViewPos
	{
		get
		{
			return cameraViewPos;
		}
	}

	private Transform cameraViewPos;

	// Use this for initialization
	void Start () 
	{
		cameraViewPos = transform.Find ("CameraPos");
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public virtual C_Fish SpawnFish(GameObject FishPrefab)
	{
		GameObject newFish = Instantiate (FishPrefab, transform);

		C_Fish fishScript = newFish.GetComponent<C_Fish> ();

		if(fishScript != null)
		{
			fishScript.Aquarium = this;
		}

		return fishScript;
	}
}
