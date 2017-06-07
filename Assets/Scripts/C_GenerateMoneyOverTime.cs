using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_GenerateMoneyOverTime : MonoBehaviour 
{
	public int Money;
	public float TimeMin;
	public float TimeMax;

	private C_GameManager gameManager;

	void Start()
	{
		gameManager = C_GameManager.Instance;

		StartCoroutine ("GenerateMoney");
	}

	IEnumerator GenerateMoney()
	{
		while (true) 
		{
			//Wait
			float rand = Random.Range (TimeMin, TimeMax);
			yield return new WaitForSeconds (rand);

			//Generate
			gameManager.GenerateMoneyAtLocation(Money, transform.position, 0.2f);
		}
	}
}
