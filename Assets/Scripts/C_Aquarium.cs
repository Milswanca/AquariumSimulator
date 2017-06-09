using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

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
    private C_GameManager gameManager;

	// Use this for initialization
	void Start () 
	{
		cameraViewPos = transform.Find ("CameraPos");
        tankBoundsBox = transform.Find("TankBoundary").GetComponent<BoxCollider>();
        gameManager = C_GameManager.Instance;
    }
	
	// Update is called once per frame
	void Update () 
	{

	}

    public virtual C_Fish SpawnFish(string FishName)
    {
        GameObject spawnPrefab = gameManager.GetPurchaseDataByName(FishName).PurchaseObject;

        GameObject newFish = Instantiate(spawnPrefab, transform);

        C_Fish fishScript = newFish.GetComponent<C_Fish>();

        if (fishScript != null)
        {
            fishScript.Aquarium = this;
            AquariumFlockManager.FlockGroups[0].RegisterFlocker(fishScript.GetComponent<C_Flock>());
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

    public virtual void SaveAquariumData()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/saveData.dat", FileMode.Create);

        I_Savable[] saveableObjects = gameObject.GetComponentsInChildren<I_Savable>();

        List<FAquariumSaveData> saveData = new List<FAquariumSaveData>();

        foreach(I_Savable i in saveableObjects)
        {
            saveData.Add(i.GetSaveData());
        }

        bf.Serialize(file, saveData);

        file.Close();
    }

    public virtual void LoadAquariumData()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/saveData.dat", FileMode.OpenOrCreate);

        List<FAquariumSaveData> deserializedData = (List<FAquariumSaveData>)bf.Deserialize(file);

        foreach(FAquariumSaveData i in deserializedData)
        {
            SpawnFish(i.ObjectName).GetComponent<I_Savable>().LoadFromData(i);
        }

        file.Close();
    }
}

[System.Serializable]
public class FAquariumSaveData
{
    public int SaveID;
    public string ObjectName;

    public FAquariumSaveData(string objName)
    {
        ObjectName = objName;
        SaveID = -1;
    }
}