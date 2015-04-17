using UnityEngine;
using System.Collections;

public class PlayerSearching : MonoBehaviour {
	
	public float health = 200;
	public float higherBound = 120;
	public float lowerBound = 200;
	public float rotationSpeed = 0.1f;
	public float shootingDistance = 15;
	public int damage = 10;
	public float fireRate = 5;
	bool lowerRiched = false;
	bool higherRiched = true;
	Transform firePoint;
	Transform directionPoint;
	public LayerMask whatToHit;
	public AudioClip shootingSound;

	public Transform BulletTrailPrefab;
	public Transform MuzzleFlashPrefab;
	[HideInInspector]
	public float maxHealth = 200;
	public bool isPlayerVisible = false;
	private float distanceToPlayer = 20;
	private Transform target;
	public float effectSpawnRate = 10;
	private float timeToSpawnEffect = 0;
	private float timeToFire = 0;
	private Inventory inventory;
	private GameObject targetObj;


	void Start () {
		FindPlayer ();
		inventory = GameObject.FindObjectOfType<Inventory> ();
		firePoint = transform.FindChild ("FirePoint");
		directionPoint = transform.FindChild ("DirectionPoint");
	}
	
	void Update () {
		if (!isPlayerVisible)
			Rotate ();
		DetectPlayer ();
	}

	void Rotate()
	{
		float rotZ = transform.eulerAngles.z;
		if (higherRiched) {
			this.transform.Rotate (new Vector3 (0f, 0f, rotationSpeed * Time.deltaTime));
			if (rotZ >= lowerBound) {
				higherRiched = false;
				lowerRiched = true;
			}
		}
		if (lowerRiched) {
			this.transform.Rotate (new Vector3 (0f, 0f, -rotationSpeed * Time.deltaTime));
			if (rotZ <= higherBound) {
				higherRiched = true;
				lowerRiched = false;
			}
		}
	}

	void DetectPlayer()
	{
		Vector2 firePointPosition = new Vector2 (firePoint.position.x, firePoint.position.y);
		Vector2 directionPointPosition = new Vector2 (directionPoint.position.x,  directionPoint.position.y);
		RaycastHit2D hit = Physics2D.Raycast (firePointPosition, firePointPosition - directionPointPosition, -100, whatToHit);

		isPlayerVisible = false;

		if (hit.collider.name != null) 
		{
			Debug.Log (hit.collider.name);
			if (hit.collider.name.Contains("Player")) 
			{
				Debug.DrawLine (firePointPosition, hit.point, Color.blue);
				GetDistance ();
			}
		}
	}

	void GetDistance()
	{
		FindPlayer ();
		distanceToPlayer = Vector2.Distance (target.transform.position, this.transform.position);

		if (distanceToPlayer <= shootingDistance) {
			isPlayerVisible = true;
			Shoot ();
		} 
		else 
		{
			isPlayerVisible = false;
		}
	}

	void FindPlayer()
	{
		targetObj = GameObject.FindGameObjectWithTag("Player");
		if (targetObj != null) 
		{
			target = targetObj.transform;
		} 
	}

	void Shoot () 
	{
		if (Time.time <= timeToFire)
			return;
		timeToFire = Time.time + 1 / fireRate;

		GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ().DamagePlayer (damage);
		Effect ();
		audio.clip = shootingSound;
		if (audio != null) 
		{
			audio.Play();
		}
		timeToSpawnEffect = Time.time + 1/effectSpawnRate;
	}
	
	void Effect () 
	{
		Instantiate (BulletTrailPrefab, firePoint.position, firePoint.rotation);
		Transform clone = (Transform)Instantiate (MuzzleFlashPrefab, firePoint.position, firePoint.rotation);
		clone.parent = firePoint;
		float size = Random.Range (0.7f, 1f);
		clone.localScale = new Vector3 (size, size, size);
		Destroy (clone.gameObject, 0.02f);
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.tag == "BulletTrail") 
		{
			System.String currentWeapon = inventory.GetActiveItemName ();
			float damage = GameObject.Find (currentWeapon).GetComponent<Weapon> ().damage;
			health -= damage;

			if (health <= 0)
				Death ();
		} 
	}
	
	void Death()
	{
		Destroy (transform.parent.gameObject);
	}
}










