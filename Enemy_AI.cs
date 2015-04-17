using UnityEngine;
using System.Collections;

public class Enemy_AI : MonoBehaviour {

	public Transform target;
	public float health;
	public float maxHealth = 100;
	public float speed = 300f;
	public int attackDistance = 20;
	public int jumpForce = 3;
	public AudioClip alienHit;

	private Rigidbody2D rb;
	private Animator anim;
	public ForceMode2D fMode;
	private Vector3 targetDirection;
	private Inventory inventory;

	private float distanceToPlayer = 50;

	void Awake()
	{
		inventory = GameObject.FindObjectOfType<Inventory>();
		anim = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D> ();
	}

	void Start () 
	{
		if (target == null) 
		{
			FindPlayer();
		}
	}

	void Update()
	{
		if (target != null) 
		{
			StartCoroutine (GetDirectionAndDistance ());
			MoveToPlayer ();
		} 
		else 
		{
			FindPlayer ();
		}

	}

	IEnumerator GetDirectionAndDistance()
	{
		yield return new WaitForSeconds (1);
		if (target != null) 
		{
			distanceToPlayer = Vector2.Distance (target.transform.position, this.transform.position);
			targetDirection = (target.position - this.transform.position);
		}
	}

	void MoveToPlayer()
	{
		if (distanceToPlayer < attackDistance) 
		{
			SetFacingDirection();
			targetDirection.Normalize();
			rb.AddForce (targetDirection.normalized * speed, fMode);
			anim.SetFloat ("moveSpeed", 100);
		} 
		else 
		{
			anim.SetFloat ("moveSpeed", 0);
		}
	}

	void SetFacingDirection()
	{
		int direction = 1;
		if (targetDirection.x > 0) direction = -1;
		this.transform.localScale = new Vector3 (direction, 1, 1);
	}

	void FindPlayer()
	{
		GameObject targetObj = GameObject.FindGameObjectWithTag("Player");
		if (targetObj != null) 
		{
			target = targetObj.transform;
		} 
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.tag == "BulletTrail") 
		{
			System.String currentWeapon = inventory.GetActiveItemName ();
			float damage = GameObject.Find (currentWeapon).GetComponent<Weapon> ().damage;
			health -= damage;

			audio.clip = alienHit;
			if (!audio.isPlaying)
				audio.Play ();

			if (health <= 0)
				Death ();
		} 
		if (col.gameObject.tag == "Obstacle") 
		{
			rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
		}
	}

	void Death()
	{
		Destroy (this.gameObject);
	}













}