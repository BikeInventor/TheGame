using UnityEngine;
using System.Collections;

public class MoveTrail : MonoBehaviour {

	public float moveSpeed = 200f;

	void Start () {
	}

	void Update () {
		transform.Translate (Vector3.right * Time.deltaTime * moveSpeed);
		if (this.gameObject != null)
			Destroy (gameObject, 0.7f);
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.collider.CompareTag ("Obstacle")) 
		{
			this.renderer.enabled = false;
		}
		Destroy (this.gameObject);

	}
}