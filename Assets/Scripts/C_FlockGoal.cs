using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnGoalDestroyedSignature(C_FlockGoal goal);

public class C_FlockGoal : C_FlockBound 
{
	public string GoalType;

	public OnGoalDestroyedSignature OnGoalDestroyed;
     
	// Use this for initialization
	void Start () 
	{
		OnGoalDestroyed = GoalDestroyed;
	}

	void OnDestroy()
	{
		OnGoalDestroyed (this);
	}

	void GoalDestroyed(C_FlockGoal goal)
	{
		
	}
}
