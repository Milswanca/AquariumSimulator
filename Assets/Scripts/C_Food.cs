using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Food : MonoBehaviour, I_Purchasable
{
	public float FoodValue;
	public float LifeTime;

    private C_GameManager gameManager;

	// Use this for initialization
	void Start () 
	{
		Destroy (gameObject, LifeTime);
	}

	void OnTriggerEnter(Collider other)
	{
		C_Fish fish = other.GetComponent<C_Fish> ();

		if(fish != null)
		{
			if(fish.IsHungry)
			{
				fish.Hunger += FoodValue;
				Destroy (gameObject);
			}
		}
	}

	public void OnPurchased()
	{
        gameManager = C_GameManager.Instance;

        gameManager.CurrentAquarium.SpawnFood(gameObject);
	}
}