using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour 
{
	public System.String weaponType;
	private int bulletsCount;
	public float fireRate = 0;
	public float damage = 10;
	public int bulletsInMagazine = 0;
	public LayerMask whatToHit;
	public AudioClip shootingSound;
	public AudioClip outOfAmmoSound;
	public AudioClip reloadingSound;

	public Transform BulletTrailPrefab;
	public Transform MuzzleFlashPrefab;
	private float timeToSpawnEffect = 0;
	public float effectSpawnRate = 10;

	float timeToFire = 0;
	Transform firePoint;
	private bool isReloading = false;
	private bool isReloaded = true;

	private Inventory inventory;

	void Awake() 
	{
		firePoint = transform.FindChild ("FirePoint");
		if (firePoint == null) 
		{
			Debug.LogError("No firepoint!!1");
		}
		inventory = GameObject.Find ("_GM").GetComponent<Inventory> ();
		// Кол-во боеприпасов берётся из инвертаря
		bulletsCount = inventory.GetValueByName (weaponType);
	}
	
	void Update () 
	{
		HandleShooting ();
	}

	void HandleShooting()
	{
		bool isAiming = GameObject.FindObjectOfType<Player>().GetComponent<UnitySampleAssets._2D.PlatformerCharacter2D> ().isAiming;
		bool isValidAngle = GameObject.Find ("AsArm").GetComponent<ArmRotation>().isValidAngle();
		bulletsCount = inventory.GetValueByName (weaponType);

		if (isValidAngle && isAiming) 
		{
			if (fireRate == 0) 
			{
				if (Input.GetMouseButtonDown (0)) 
				{
					Shoot ();
				}
			} 
			else 
			{         
				if (Input.GetMouseButton (0) && Time.time > timeToFire) 
				{
					timeToFire = Time.time + 1 / fireRate;
					Shoot ();
				}
			}
		}
	}

	void Shoot () 
	{
		if (bulletsCount == 0)
		{
			NoAmmmoLeft();
			return;
		}

		if (isReloading && !isReloaded)
			return;

		isReloaded = false;

		Vector2 mousePosition = new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x + 100, Camera.main.ScreenToWorldPoint (Input.mousePosition).y);
		Vector2 firePointPosition = new Vector2 (firePoint.position.x, firePoint.position.y);
		RaycastHit2D hit = Physics2D.Raycast (firePointPosition, mousePosition - firePointPosition, 100, whatToHit);
		if (Time.time >= timeToSpawnEffect) 
		{
			Effect ();
			audio.clip = shootingSound;
			if (audio != null) 
			{
				audio.Play();
			}
			timeToSpawnEffect = Time.time + 1/effectSpawnRate;
		}
		Debug.DrawLine (firePointPosition, (mousePosition - firePointPosition) * 100, Color.cyan);
		if (hit.collider != null) 
		{
			Debug.DrawLine (firePointPosition, hit.point, Color.red);
			Debug.Log ("We hit" + hit.collider.name + "and did" + damage + " damage!");
		}
		bulletsCount--;
		inventory.SetValueByName (weaponType, bulletsCount); 

		if (bulletsCount != 0 && bulletsCount % bulletsInMagazine == 0 && !isReloading && !isReloaded) 
		{
			StartCoroutine (Reloading());
		}
	}

	void Effect () 
	{
		Instantiate (BulletTrailPrefab, firePoint.position, firePoint.rotation);
		Transform clone = (Transform)Instantiate (MuzzleFlashPrefab, firePoint.position, firePoint.rotation);
		clone.parent = firePoint;
		float size = Random.Range (0.6f, 0.9f);
		clone.localScale = new Vector3 (size, size, size);
		Destroy (clone.gameObject, 0.02f);
	}

	void NoAmmmoLeft()
	{
		audio.clip = outOfAmmoSound;
		if (!audio.isPlaying)
			audio.Play ();
	}
	
	void HandleReloading ()
	{
		if (bulletsCount != 0 && bulletsCount % bulletsInMagazine == 0) 
		{
			StartCoroutine (Reloading ());
		} 
	}

	IEnumerator Reloading()
	{
		isReloading = true;
		yield return new WaitForSeconds (0.3f);
		audio.clip = reloadingSound;
		audio.Play ();
		yield return new WaitForSeconds (2);
		isReloading = false;
		isReloaded = true;
	}
}














