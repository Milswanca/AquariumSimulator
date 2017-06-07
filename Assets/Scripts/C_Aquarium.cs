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
    private BoxCollider tankBoundsBox;

	// Use this for initialization
	void Start () 
	{
		cameraViewPos = transform.Find ("CameraPos");
        tankBoundsBox = transform.Find("TankBoundary").GetComponent<BoxCollider>();
    }
	
	// Update is called once per frame
	void Update () 
	{
		
	}

    public virtual C_Fish SpawnFish(GameObject FishPrefab)
    {
        GameObject newFish = Instantiate(FishPrefab, transform);

        C_Fish fishScript = newFish.GetComponent<C_Fish>();

        if (fishScript != null)
        {
            fishScript.Aquarium = this;
        }

        return fishScript;
    }

    public virtual C_Food SpawnFood(GameObject FoodPrefab)
    {
        Vector3 randVec = GetTankBounds();
        Vector3 spawnLoc = transform.position + new Vector3(Random.Range(-1.0f, 1.0f) * randVec.x, Random.Range(-1.0f, 1.0f) * randVec.y, Random.Range(-1.0f, 1.0f) * randVec.z);

        GameObject newFood = Instantiate(FoodPrefab, spawnLoc, Quaternion.identity);

        C_Food foodScript = newFood.GetComponent<C_Food>();

        return foodScript;
    }

    protected virtual Vector3 GetTankBounds()
    {
        return tankBoundsBox.size * 0.5f;
    }
}
