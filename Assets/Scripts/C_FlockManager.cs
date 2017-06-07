using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct FFlockInitGroup
{
    public GameObject FlockPrefab;
    public int SpawnCount;
}

[System.Serializable]
public class C_FlockGroup
{
	[SerializeField]
	public List<C_Flock> Flockers;

    [SerializeField]
    public List<FFlockInitGroup> InitialSpawns;

    private C_FlockManager manager;

	public void InitGroup(C_FlockManager inManager)
	{
        manager = inManager;

		foreach(C_Flock i in Flockers)
		{
			i.FlockGroup = this;
		}
	}

	public void RegisterFlocker(C_Flock flocker)
	{
		if(flocker != null)
		{
			flocker.FlockGroup = this;
			Flockers.Add(flocker);
		}
	}
}

public class C_FlockManager : MonoBehaviour 
{
	[SerializeField]
	public List<C_FlockGroup> FlockGroups;

	public List<C_Flock> AllFlockers
	{
		get
		{
			List<C_Flock> flockers = new List<C_Flock> ();

			foreach(C_FlockGroup flockGroup in FlockGroups)
			{
				flockers.AddRange (flockGroup.Flockers);
			}

			return flockers;
		}
	}

	// Use this for initialization
	void Start () 
	{
		foreach(C_FlockGroup i in FlockGroups)
		{
			i.InitGroup (this);
		}

		C_GameManager.Instance.OnFishSpawnedDelegate += FishSpawned;
	}

    private void FishSpawned(C_Fish fish)
    {
		C_Flock flocker = fish.GetComponent<C_Flock> ();
		FlockGroups [0].RegisterFlocker (flocker);
    }
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
