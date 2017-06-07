using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Fish : C_Attraction, I_Purchasable
{
	public string Name;
	public float HungerMax;
	public float FeedThreshold;

	public C_Aquarium Aquarium { get; set; }

	public float Hunger 
	{
		get
		{
			return m_hunger;
		}
		set
		{
			m_hunger = value;
			Mathf.Clamp (m_hunger, 0, HungerMax);

			if(priorityManager != null)
			{
				if(HungerRatio <= FeedThreshold)
				{
					IsHungry = true;
					priorityManager.SetPriority ("Food", 1);
				}	
				else
				{
					IsHungry = false;
					priorityManager.SetPriority ("Food", 0);
				}
			}
		}
	}

	private float m_hunger;

	public float HungerRatio
	{
		get
		{
			return Hunger / HungerMax;
		}

		set
		{
			Hunger = HungerMax * value;
		}
	}

	public bool IsHungry
	{
		set
		{
			m_isHungry = value;

			if(m_isHungry)
			{
				foreach(MeshRenderer m in meshRenderers)
				{
					m.material.color = Color.green;
				}
			}
			else
			{
				foreach(MeshRenderer m in meshRenderers)
				{
					m.material.color = Color.white;
				}
			}
		}

		get
		{
			return m_isHungry;
		}
	}

	private bool m_isHungry;

	//Scaling
	//private Vector3 originalScale;
	private C_GameManager gameManager;
	private C_GoalPriorityManager priorityManager;
	private MeshRenderer[] meshRenderers;

	// Use this for initialization
	void Start ()
    {
		//originalScale = transform.localScale;

		priorityManager = GetComponent<C_GoalPriorityManager> ();
		meshRenderers = GetComponentsInChildren<MeshRenderer> ();

		Hunger = HungerMax;
	}

	void Update()
	{
		Hunger -= Time.deltaTime;
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Food")
		{
			if (IsHungry) 
			{
				C_Food foodComp = other.GetComponent<C_Food> ();

				if (foodComp != null) 
				{
					Hunger += foodComp.FoodValue;
					Destroy (other.gameObject);
				}
			}
		}
	}

	public void OnPurchased()
	{
		gameManager = C_GameManager.Instance;
		gameManager.SpawnFish (gameObject);
	}
}