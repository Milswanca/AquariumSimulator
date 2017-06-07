using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct FGoalPriority
{
	public string GoalType;
	public int Priority;

	public FGoalPriority(string goalType, int priority)
	{
		GoalType = goalType;
		Priority = priority;
	}
}

[RequireComponent(typeof(Collider))]
public class C_GoalPriorityManager : MonoBehaviour 
{
	public List<C_FlockGoal> Goals { get; set; }
	public List<FGoalPriority> GoalPriorities;

	public C_FlockGoal BestGoal
	{
		get
		{
			C_FlockGoal bestGoal = null;
			int bestPriority = 0;

			foreach(C_FlockGoal goal in Goals)
			{
				foreach(FGoalPriority priority in GoalPriorities)
				{
					if(priority.GoalType == goal.GoalType && priority.Priority > bestPriority)
					{
						bestGoal = goal;
						bestPriority = priority.Priority;
					}
				}
			}

			return bestGoal;
		}
	}

	// Use this for initialization
	void Start () 
	{
		Goals = new List<C_FlockGoal> ();
	}

	void OnTriggerEnter(Collider collider)
	{
		C_FlockGoal goal = collider.GetComponent<C_FlockGoal> ();

		if(goal != null)
		{
			Goals.Add (goal);
			goal.OnGoalDestroyed += OnGoalDestroyed;
		}
	}

	void OnTriggerExit(Collider other)
	{
		C_FlockGoal goal = other.GetComponent<C_FlockGoal> ();

		if(goal != null)
		{
			Goals.Remove (goal);
		}
	}

	public void SetPriority(string name, int priority)
	{
		for(int i = 0; i < GoalPriorities.Count; ++i)
		{
			if(GoalPriorities[i].GoalType == name)
			{
				GoalPriorities[i] = new FGoalPriority(name, priority);
			}
		}
	}

	void OnGoalDestroyed(C_FlockGoal goal)
	{
		Goals.Remove (goal);
	}
}
