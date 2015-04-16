using UnityEngine;
using System.Collections;

public class MedKitPickUp: MonoBehaviour {
	
	public AudioClip medKitPickedUp;
	
	void OnTriggerEnter2D (Collider2D info) 
	{
		
		if (info.tag == "Player") 
		{
			int currentHealth = GameObject.FindObjectOfType<Player>().GetComponent<Player>().playerStats.health;
			if (currentHealth == 100) return;
			int newHealth = currentHealth + Random.Range(30,75);
			newHealth = (newHealth > 100) ? 100 : newHealth;
			GameObject.FindObjectOfType<Player>().GetComponent<Player>().playerStats.health = newHealth;
			AudioSource.PlayClipAtPoint(medKitPickedUp, this.transform.position);
			Destroy(this.gameObject);
		}
		
	}
}
