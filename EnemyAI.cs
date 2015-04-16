using UnityEngine;
using Pathfinding;
using System.Collections;

[RequireComponent (typeof(Rigidbody2D))]
[RequireComponent (typeof(Seeker))]

public class EnemyAI : MonoBehaviour {

	public Transform target;
	public float updateRate = 2f;
	private Seeker seeker;
	private Rigidbody2D rb;
	private Animator anim;
		
	public Path path;
	public float health;
	public float speed = 300f;
	public int attackDistance = 20;
	public ForceMode2D fMode;

	[HideInInspector]
	public bool pathIsEnded = false;
	public float nextWaypointDistance = 3;
	private int currentWaypoint = 0;
	private float distanceToPlayer = 50;

	void Awake()
	{
		anim = GetComponent<Animator>();
		anim = GetComponent<Animator>();
		seeker = GetComponent<Seeker> ();
		rb = GetComponent<Rigidbody2D> ();
	}
	
	void Start () 
	{
		health = 100f;
		if (target == null) 
		{
			Debug.Log ("No target found!");
			return;
		}
		seeker.StartPath (transform.position, target.position, OnPathComplete);		
		StartCoroutine (UpdatePath ());
	}

	void Update()
	{
		GetDistance ();
	}
	
	public void OnPathComplete (Path p) 
	{
		if (!p.error) 
		{
			path = p;
			currentWaypoint = 0;
		}
	}
	
	IEnumerator UpdatePath()
	{
		if (target == null) 
		{
			if (distanceToPlayer > attackDistance)
				FindPlayer();
			return false; 
		}
		seeker.StartPath (transform.position, target.position, OnPathComplete);
		yield return new WaitForSeconds(1f/updateRate);
		StartCoroutine(UpdatePath());
	}

	void GetDistance()
	{
		distanceToPlayer = Vector3.Distance (target.transform.position, this.transform.position);
		Debug.Log ("DIST: " + distanceToPlayer);
	}

	void FixedUpdate()
	{
		if (target == null) 
		{
			FindPlayer();
			return; 
		}
		if (path == null || distanceToPlayer > attackDistance) 
		{
			anim.SetFloat ("moveSpeed", 0);
			return;
		}
		if (currentWaypoint >= path.vectorPath.Count) 
		{
			if (pathIsEnded)
				return;
			pathIsEnded = true;
			return;
		}
		pathIsEnded = false;

		Vector3 dir = (path.vectorPath [currentWaypoint] - transform.position).normalized;
		SetFacingDirection (dir);
		dir *= speed * Time.fixedDeltaTime;
		anim.SetFloat ("moveSpeed", Mathf.Abs (speed));
		rb.AddForce (dir, fMode);
		float dist = Vector3.Distance (transform.position, path.vectorPath [currentWaypoint]);
		if (dist < nextWaypointDistance) 
		{
			currentWaypoint++;
			return;
		}
	}

	void FindPlayer(){
		GameObject targetObj = GameObject.FindGameObjectWithTag("Player");
		if (targetObj != null) 
		{
			target = targetObj.transform;
			Debug.Log ("Player FOUND!");
			StartCoroutine (UpdatePath ());
		} 
		else 
		{
			Debug.Log ("Player NOT FOUND!");
			return;
		}
	}

	void SetFacingDirection(Vector3 dir)
	{
		int direction = 1;
		if (currentWaypoint != 0) 
		{
			if (dir.x > 0) direction = -1;
			transform.localScale = new Vector3 (direction, 1, 1);
		}
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.tag == "BulletTrail") 
		{
			health -= 10;
			if (health <= 0)
				Death();
		}
	}

	void Death()
	{
		Destroy (this.gameObject);
	}

}
