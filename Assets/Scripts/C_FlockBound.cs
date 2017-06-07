using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class C_FlockBound : MonoBehaviour 
{
	protected C_FlockManager flockManager;
	protected Collider trigger;

	// Use this for initialization
	void Start () 
	{
		trigger = GetComponent<Collider> ();
		flockManager = GameObject.FindGameObjectWithTag ("FlockManager").GetComponent<C_FlockManager> ();
	}
}
