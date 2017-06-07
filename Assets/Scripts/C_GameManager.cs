using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameStatics;

[System.Serializable]
public class FPurchaseData
{
	//Identifiers
	public string PurchaseName;
	public GameObject PurchaseObject;

	//Saves having to build a graph for basic items like food
	public bool UseConstantCost;
	public int ConstantCost;
	public AnimationCurve CostGraph;

	//Cost/Purchase data
	public int CurrentPurchases { get; set; }
	public bool InfinitePurchases;
	public int MaxPurchases;

	public int Cost 
	{
		get
		{
			if (UseConstantCost) 
			{
				return ConstantCost;	
			} 
			else 
			{
				if (CostGraph != null) 
				{
					return (int)CostGraph.Evaluate (CurrentPurchases);
				}

				return 0;
			}
		}
	}
}

[System.Serializable]
public class FCoinData
{
	public int Value;
	public GameObject CoinPrefab;
}

public delegate void OnFishSpawnedSignature(C_Fish Fish);

public class C_GameManager : MonoBehaviour
{
	public int Money;

	public C_Aquarium CurrentAquarium
    {
        get
        {
            if(Aquariums != null && Aquariums.IsValidIndex(currentAquariumIndex))
            {
                return Aquariums[currentAquariumIndex];
            }

            return null;
        }
    }

	private int currentAquariumIndex;

	[SerializeField]
	public List<FPurchaseData> Purchasables;

	[SerializeField]
	public List<FCoinData> CoinData;

	public OnFishSpawnedSignature OnFishSpawnedDelegate;

	public List<C_Aquarium> Aquariums;

	private Camera mainCamera;

	//----------------------------------------------Game Instance Singleton -------------------------------------------------//
	private static C_GameManager instance = null;

	// Game Instance Singleton
	public static C_GameManager Instance
	{
		get
		{ 
			return instance; 
		}
	}

	private void Awake()
	{
		// if the singleton hasn't been initialized yet
		if (instance != null && instance != this) 
		{
			Destroy(this.gameObject);
		}

		instance = this;
		DontDestroyOnLoad( this.gameObject );
	}
	//-------------------------------------------------------------------------------------------------------------------------//

	void Start()
	{
		CoinData.Sort (SortCoinDataByValue);

		OnFishSpawnedDelegate = OnFishSpawned;

		mainCamera = Camera.main;
		currentAquariumIndex = 0;
	}

	void Update()
	{
		if(mainCamera != null && CurrentAquarium != null && CurrentAquarium.CameraViewPos != null)
		{
			mainCamera.transform.position = Vector3.Lerp (mainCamera.transform.position, CurrentAquarium.CameraViewPos.position, 0.2f);
			mainCamera.transform.rotation = Quaternion.Lerp (mainCamera.transform.rotation, CurrentAquarium.CameraViewPos.rotation, 0.2f);
		}
	}

	public void SpawnFish(GameObject Fish)
	{
		C_Fish fish = CurrentAquarium.SpawnFish(Fish);
		OnFishSpawnedDelegate (fish);
	}

    public void SpawnFood(GameObject Food)
    {
        C_Food fish = CurrentAquarium.SpawnFood(Food);
    }

	public void PurchaseItem(string itemName)
	{
		foreach (FPurchaseData p in Purchasables)
		{
			if (itemName == p.PurchaseName && CanPurchaseItem(p))
			{
				I_Purchasable purchasable = p.PurchaseObject.GetComponent<I_Purchasable>();

				Money -= p.Cost;
				purchasable.OnPurchased ();
				p.CurrentPurchases++;
			}
		}
	}

	public bool CanPurchaseItem(FPurchaseData purchasable)
	{
		return ((purchasable.CurrentPurchases < purchasable.MaxPurchases || purchasable.InfinitePurchases) && Money >= purchasable.Cost);
	}

	public void GenerateMoneyAtLocation(int MoneyValue, Vector3 Location, float SpawnRadius = 0.0f)
	{
		int toPay = MoneyValue;
		CoinData.Sort (SortCoinDataByValue);

		FCoinData toSpawn = null;

		while ((toSpawn = GetFirstAffordableCoin (toPay)) != null)
		{
			Vector3 randOffset = Random.insideUnitSphere * SpawnRadius;

			GameObject coinObj = Instantiate(toSpawn.CoinPrefab, Location + randOffset, Quaternion.identity);

			if(coinObj != null)
			{
				C_Coin coinScript = coinObj.GetComponent<C_Coin> ();

				if(coinScript != null)
				{
					coinScript.CoinValue = toSpawn.Value;
				}
			}

			toPay -= toSpawn.Value;
		}
	}

	private FCoinData GetFirstAffordableCoin (int money)
	{
		foreach(FCoinData i in CoinData)
		{
			if(i.Value <= money)
			{
				return i;
			}
		}

		return null;
	}

	public static int SortCoinDataByValue(FCoinData a, FCoinData b)
	{
		return -a.Value.CompareTo (b.Value);
	}

	private void OnFishSpawned(C_Fish fish)
	{
		
	}

	public void GoToNextAquarium()
	{
        if (Aquariums.IsValidIndex(currentAquariumIndex + 1))
        {
            currentAquariumIndex++;
        }
    }

    public void GoToPreviousAquarium()
    {
        if (Aquariums.IsValidIndex(currentAquariumIndex - 1))
        {
		    currentAquariumIndex--;
        }
	}
}