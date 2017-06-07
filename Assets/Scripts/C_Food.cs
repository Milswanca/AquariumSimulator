using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Food : MonoBehaviour, I_Purchasable
{
	public float FoodValue;
	public float LifeTime;

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
		float randPosX = Random.Range (-7.0f, 7.0f);
		float randPosY = Random.Range (-2.75f, 2.75f);
		float randPosZ = Random.Range (-2.5f, 2.5f);

		Instantiate(this, new Vector3(randPosX, randPosY, randPosZ), Quaternion.identity);
	}
}