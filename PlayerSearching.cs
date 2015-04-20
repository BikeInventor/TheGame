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
	public float hackingDelay;
	bool lowerRiched = false;
	bool higherRiched = true;
	Transform firePoint;
	Transform directionPoint;
	public LayerMask whatToHit;
	public AudioClip shootingSound;
	public AudioClip hackingSound;
	public AudioClip explosionSound;
	public Transform explosionPrefab;

	public Transform BulletTrailPrefab;
	public Transform MuzzleFlashPrefab;
	[HideInInspector]
	public float maxHealth = 200;
	public bool isTargetVisible = false;
	private float distanceToPlayer = 20;
	private Transform target;
	public float effectSpawnRate = 10;
	private float timeToSpawnEffect = 0;
	private float timeToFire = 0;
	private Inventory inventory;
	public bool isHacked = false;

	void Start () {
		inventory = GameObject.FindObjectOfType<Inventory> ();
		firePoint = transform.FindChild ("FirePoint");
		directionPoint = transform.FindChild ("DirectionPoint");
		SetGunColor ();
	}
	
	void Update () {
		if (!isTargetVisible)
			Rotate ();
		DetectTarget ();
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

	void DetectTarget()
	{
		Vector2 firePointPosition = new Vector2 (firePoint.position.x, firePoint.position.y);
		Vector2 directionPointPosition = new Vector2 (directionPoint.position.x,  directionPoint.position.y);
		RaycastHit2D hit = Physics2D.Raycast (firePointPosition, firePointPosition - directionPointPosition, -shootingDistance, whatToHit);

		isTargetVisible = false;
		if (hit.collider == null)
			return;
		if (hit.collider.name != null) 
		{
			if (!isHacked && hit.collider.name.Contains("Player"))
			{
				Debug.DrawLine (firePointPosition, hit.point, Color.blue);
				isTargetVisible = true;
				Shoot(hit.transform.gameObject);
			}
			if (isHacked && hit.collider.name.Contains("Alien"))
			{
				Debug.DrawLine (firePointPosition, hit.point, Color.red);
				isTargetVisible = true;
				Shoot (hit.transform.gameObject);
			}
		}
	}

	void Shoot (GameObject targetObj) 
	{
		if (Time.time <= timeToFire)
			return;
		timeToFire = Time.time + 1 / fireRate;

		if (!isHacked) 
		{
			targetObj.GetComponent<Player> ().DamagePlayer (damage);
		} 
		else 
		{
			targetObj.GetComponent<Enemy_AI> ().DamageAlien (damage);
		}

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
			this.DamageGun(damage);
		} 
		if (col.gameObject.tag.Contains("Player"))
		{
			if (!isHacked)
				StartCoroutine(Hack ());
		}
	}

	void DamageGun(float damage)
	{
		health -= damage;		
		if (health <= 0)
			Death ();
	}

	IEnumerator Hack()
	{
		isHacked = true;
		SetGunColor ();
		audio.clip = hackingSound;
		audio.Play ();
		yield return new WaitForSeconds (hackingDelay);
		isHacked = false;
		SetGunColor ();
		this.DamageGun (maxHealth / 4);
	}

	void OnGUI()
	{
		Player player = GameObject.FindObjectOfType<Player> ();
		if (player != null) 
		{
			distanceToPlayer = Vector2.Distance (this.transform.position, player.transform.position);
			if (distanceToPlayer < 5 && !isHacked) 
			{
				GUI.Box (new Rect (0, Screen.height - 30, Screen.width, 30), "Press <E> to hack enemy gun");
			}
		}
	}

	void SetGunColor ()
	{
		if (isHacked)
			this.GetComponent<SpriteRenderer> ().color = Color.green;
		else
			this.GetComponent<SpriteRenderer> ().color = Color.red;
	}
	
	void Death()
	{
		Destroy (transform.parent.gameObject);
		GameObject explosion = Instantiate (explosionPrefab, this.transform.position, this.transform.rotation) as GameObject;
		AudioSource.PlayClipAtPoint(explosionSound, this.transform.position, 100);
	}
}










