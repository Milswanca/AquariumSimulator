using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_FlockBoundary : C_FlockBound {

	void OnTriggerEnter(Collider other)
	{
		C_Flock flocker = other.GetComponent<C_Flock> ();

		if(flocker != null)
		{
			flocker.OnEnterBoundary (this);	
		}
	}

	void OnTriggerExit(Collider other)
	{
		C_Flock flocker = other.GetComponent<C_Flock> ();

		if(flocker != null)
		{
			flocker.OnLeftBoundary (this);	
		}
	}
}
