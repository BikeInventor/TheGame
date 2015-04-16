using UnityEngine;
using Pathfinding;
using System.Collections;

public class Enemy_AI : MonoBehaviour {

	public Transform target;
	public float health;
	public float speed = 300f;
	public int attackDistance = 20;

	private Rigidbody2D rb;
	private Animator anim;
	public ForceMode2D fMode;
	private Vector3 targetDirection;

	private float distanceToPlayer = 50;

	void Awake()
	{
		anim = GetComponent<Animator>();
		anim = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D> ();
	}

	void Start () 
	{
		if (target == null) 
		{
			Debug.Log ("No target found!");
			return;
		}
		GetDirectionAndDistance ();
		MoveToPlayer ();
	}

	void GetDirectionAndDistance()
	{
		distanceToPlayer = Vector3.Distance (target.transform.position, this.transform.position);

		targetDirection = (target.position - this.transform.position).Normalize ();

		Debug.Log ("DIST: " + distanceToPlayer);
	}

	void MoveToPlayer()
	{
		if (distanceToPlayer < attackDistance) 
		{
			rb.AddForce (targetDirection * speed, fMode);
			anim.SetFloat ("moveSpeed", 100);
		} 
		else 
		{
			anim.SetFloat ("moveSpeed", 0);
		}

	}















}