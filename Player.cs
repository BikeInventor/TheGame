using UnityEngine;
using System.Collections;
using UnitySampleAssets.CrossPlatformInput;

public class Player : MonoBehaviour {

	[System.Serializable]
	public class PlayerStats {
		public int maxHealth = 100;
		public int health = 100;
	}

	public PlayerStats playerStats = new PlayerStats();
	// значение по y, нахождение ниже которого смертельно для игрока
	public int fallBoundary = -20;

	private Animator anim;

	// сколько лежит, когда помер
	private float delayBeforeDead = 1f;
	// на сколько фризит при попадании в игрока
	private float takingDamageDelay = 0.3f;

	public AudioClip deathSound;
	public AudioClip damageSound;

	UnitySampleAssets._2D.PlatformerCharacter2D controlScript;
		
	void Awake()
	{
		anim = this.transform.FindChild ("Graphics").GetComponent<Animator> ();
		controlScript = this.GetComponent<UnitySampleAssets._2D.PlatformerCharacter2D> ();
	}

	void Update () 
	{
		if (transform.position.y <= fallBoundary) 
		{
			DamagePlayer (999);
		}
	}    

	public void DamagePlayer (int damage) 
	{
		playerStats.health -= damage;
		if (playerStats.health > 0)
			StartCoroutine (TakingDamage ());
		if (playerStats.health <= 0) 
		{
			playerStats.health = 0;
			StartCoroutine(KillPlayer());
		}
	}
	
	void OnCollisionEnter2D(Collision2D col)
	{
	if (col.collider.CompareTag("Alien"))
		 DamagePlayer(10);
	}


	public IEnumerator KillPlayer()
	{
		// Отключаем управление
		this.GetComponent<UnitySampleAssets._2D.Platformer2DUserControl> ().enabled = false;
		controlScript.enabled = false;
		controlScript.ShootingDisabled ();

		audio.clip = deathSound;
		if (!audio.isPlaying)
			audio.Play();

		anim.SetBool("Dead",true);

		Destroy (GameObject.Find ("AsArm"));

		yield return new WaitForSeconds (delayBeforeDead);

		GameMaster.KillPlayer(this);
	}

	public IEnumerator TakingDamage()
	{
		audio.clip = damageSound;
		if (!audio.isPlaying)
			audio.Play();

		controlScript.ShootingDisabled ();
		controlScript.enabled = false;
		anim.SetBool ("Damaged", true);

		yield return new WaitForSeconds (takingDamageDelay);

		controlScript.enabled = true;
		anim.SetBool ("Damaged", false);

	}

}

