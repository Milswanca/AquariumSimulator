using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_PassiveRotate : MonoBehaviour 
{
	public Vector3 RotateAxis;
	public float RotateSpeed;
	
	// Update is called once per frame
	void Update () 
	{
		transform.Rotate (RotateAxis, RotateSpeed);
	}
}
