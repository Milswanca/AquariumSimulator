using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Flock : MonoBehaviour 
{
	public float NeighbourDistance = 3.0f;
    public float SeperationDistance = 0.6f;
    public float SpeedMin = 0.5f;
    public float SpeedMax = 2.0f;
    public int RerollSpeedChance = 500;
    public float RotateSpeed = 2.0f;

    //Avoidance Values
	public bool UseAvoidance = true;
    public float AvoidanceLength = 2.0f;
    public float AvoidancePrecisionStep = 20.0f;

	//Boundaries
	public bool UseBoundaries = true;
	public C_FlockBoundary LastBoundary { get; set; }
	public List<C_FlockBoundary> Boundaries { get; set; }

	public C_FlockGroup FlockGroup { get; set; }

    private float speed;

	public List<C_Flock> Neighbours 
	{
		get
		{
			List<C_Flock> neighbours = new List<C_Flock> ();

			if(FlockGroup != null)
			{
				foreach(C_Flock i in FlockGroup.Flockers)
				{
					float dist = (transform.position - i.transform.position).magnitude;

					if(dist <= NeighbourDistance)
					{
						neighbours.Add (i);
					}
				}
			}

			return neighbours;
		}
	}

	public List<C_Flock> TooClose 
	{
		get
		{
			List<C_Flock> close = new List<C_Flock> ();

			foreach(C_Flock i in Neighbours)
			{
				if((transform.position - i.transform.position).magnitude <= SeperationDistance)
				{
					close.Add (i);
				}
			}

			return close;
		}
	}

	public bool IsInBoundary
	{
		get
		{
			if(Boundaries == null) {return true;}

			return Boundaries.Count != 0;
		}
	}

	private Rigidbody rb;
	private C_GoalPriorityManager goalPriorityManager;

	void Awake()
	{
		Boundaries = new List<C_FlockBoundary> ();
		goalPriorityManager = GetComponent<C_GoalPriorityManager> ();
	}

	// Use this for initialization
	void Start () 
	{
        speed = Random.Range (SpeedMin, SpeedMax);

		rb = GetComponent<Rigidbody> ();
		transform.LookAt(transform.position + Random.insideUnitSphere);
	}
	
	// Update is called once per frame
	void Update() 
	{
        if(Random.Range(0, RerollSpeedChance) == 1)
        {
            speed = Random.Range(SpeedMin, SpeedMax);
        }

        List<C_Flock> neighbours = Neighbours;

		Vector3 alignment, cohesion, seperation, avoidance, goal, boundary;
		alignment = cohesion = seperation = avoidance = goal = boundary = Vector3.zero;

		alignment = ComputeAlignment (neighbours);
		cohesion = ComputeCohesion (neighbours);
		seperation = ComputeSeperation (neighbours);
		goal = ComputeGoal ();
		boundary = ComputeToBoundary ();

		if(UseAvoidance)
		{
			avoidance = ComputeAvoidance();
		}

		//new facing direction
        Vector3 outFacing = Vector3.zero;
		outFacing = alignment + cohesion + seperation + avoidance + goal + (boundary * 4.0f);
        outFacing.Normalize();

		Vector3 slerpedForward = Vector3.Slerp (this.transform.forward, outFacing, Time.deltaTime * RotateSpeed);

		if (slerpedForward != Vector3.zero)
        {
			this.transform.forward = slerpedForward;
		}

        rb.velocity = transform.forward * speed;
	}

	//Returns the average direction of all agents
	Vector3 ComputeAlignment(List<C_Flock> neighbours)
	{
		Vector3 alignment = Vector3.zero;

		if (neighbours.Count == 0) {
			return alignment;
		}

		foreach (C_Flock boid in neighbours) {
			alignment += boid.transform.forward;
		}
		alignment /= neighbours.Count;
		alignment.Normalize(); // Unnecessary since the average will be a unit vector

		return alignment;
	}

	//returns direction to the center of the group
	Vector3 ComputeCohesion(List<C_Flock> neighbours)
	{
		Vector3 centerOfMass = Vector3.zero;

		// There's no point in doing the calculation if there are no nearby boids
		if (neighbours.Count == 0) {
			return centerOfMass;
		}

		foreach (C_Flock boid in Neighbours) {
			centerOfMass += boid.transform.position;
		}
		centerOfMass /= neighbours.Count;
		Vector3 cohesion = centerOfMass - this.transform.position;
		cohesion.Normalize();

		return cohesion;
	}

	Vector3 ComputeSeperation(List<C_Flock> neighbours)
	{
		Vector3 separation = Vector3.zero;

		if (TooClose.Count == 0) {
			return separation;
		}

		foreach (C_Flock boid in TooClose) 
		{
			Vector3 separate = this.transform.position - boid.transform.position;
			float sqrDistance = separate.sqrMagnitude;
			sqrDistance = Mathf.Max(sqrDistance, 0.001f);
			float distMultiplier = 1/sqrDistance;
			separation += separate * distMultiplier;
		}

		separation /= TooClose.Count;

		return separation;
	}

    Vector3 ComputeAvoidance()
    {
        if(!AvoidanceCheck(transform.forward))
        {
            return Vector3.zero;
        }

        float steppedRadiusCheck = 0.0f;

        //Flat avoidance
        while(steppedRadiusCheck <= 360)
        {
            float deltaAngle = steppedRadiusCheck;

            for(int j = 0; j <= 360; j += 30)
            {
                float ang = (float)j;
                float rad = Mathf.Tan(deltaAngle * Mathf.Deg2Rad / 2);

                Vector3 direction = transform.rotation * new Vector3(rad * Mathf.Cos(ang), rad * Mathf.Sin(ang), 1.0f);

                if(!AvoidanceCheck(direction))
                {
                    Debug.DrawLine(transform.position, transform.position + direction * AvoidanceLength, Color.green);

                    return direction;
                }
                else
                {
                    Debug.DrawLine(transform.position, transform.position + direction * AvoidanceLength, Color.red);
                }
            }

            steppedRadiusCheck += AvoidancePrecisionStep;
        }

        return Vector3.zero;
    }

    bool AvoidanceCheck(Vector3 direction)
    {
        return Physics.Raycast(transform.position, direction, AvoidanceLength);
    }

	Vector3 ComputeGoal()
	{
		if(goalPriorityManager == null) { return Vector3.zero; }
		if(goalPriorityManager.BestGoal == null) { return Vector3.zero; }

		return (goalPriorityManager.BestGoal.transform.position - transform.position).normalized;
	}

	Vector3 ComputeToBoundary()
	{
		if(LastBoundary == null) { return Vector3.zero; }
		if(IsInBoundary) { return Vector3.zero; }

		Vector3 toBound = LastBoundary.transform.position - transform.position;
		toBound.Normalize ();
		return toBound;
	}

	public void OnEnterBoundary(C_FlockBoundary boundary)
	{
		Boundaries.Add (boundary);
	}

	public void OnLeftBoundary(C_FlockBoundary boundary)
	{
		LastBoundary = boundary;
		Boundaries.Remove (boundary);
	}
}