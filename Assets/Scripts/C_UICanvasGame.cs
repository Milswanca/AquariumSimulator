using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class C_UICanvasGame : MonoBehaviour 
{
	private Text txtMoney;
	private C_GameManager gameManager;

	// Use this for initialization
	void Start () 
	{
		txtMoney = transform.Find ("txt_money").GetComponent<Text>();
		gameManager = C_GameManager.Instance;
	}
	
	// Update is called once per frame
	void Update () 
	{
		txtMoney.text = "Coins: " + gameManager.Money;	
	}
}
