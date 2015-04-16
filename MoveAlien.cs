using UnityEngine;
using System.Collections;

public class MoveAlien : MonoBehaviour {
	
	public float moveSpeed = 8.0f;
	public AudioClip breathingSound;
	private float direction = -1.0f;
	private float health;
	private Animator anim;

	void Awake () 
	{
		health = 100f;
		anim = GetComponent<Animator>();
	}

	void Update () 
	{
		Run ();
		isDead ();
	}
	
	void Run () 
	{
		rigidbody2D.velocity =  new Vector2 (moveSpeed * direction, rigidbody2D.velocity.y);
		transform.localScale = new Vector3 (-direction, 1, 1);
		anim.SetFloat("moveSpeed", Mathf.Abs(moveSpeed));
		audio.clip = breathingSound;
		if (!audio.isPlaying)
			audio.Play ();
	}

	void Stop()
	{
		rigidbody2D.velocity = new Vector2 (0, 0);
		anim.SetFloat("moveSpeed", 0);
	}

	void isDead()
	{
		if (health <=0) 
			Destroy (this.gameObject);
	}


}