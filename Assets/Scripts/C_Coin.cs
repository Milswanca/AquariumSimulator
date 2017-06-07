using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class C_Coin : MonoBehaviour 
{
	public int CoinValue;
	public float FallSpeed;

	private Rigidbody rb;
	private C_GameManager gameManager;

	// Use this for initialization
	void Start () 
	{
		rb = GetComponent<Rigidbody> ();
		rb.velocity = new Vector3(0, -Mathf.Abs (FallSpeed), 0);

		gameManager = C_GameManager.Instance;
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	void OnMouseOver()
	{
		if(Input.GetMouseButtonDown(0))
		{
			Pickup ();
		}
	}

	public void Pickup()
	{
		gameManager.Money += CoinValue;
		Destroy (gameObject);
	}
}
