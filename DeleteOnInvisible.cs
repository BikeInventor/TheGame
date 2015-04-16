using UnityEngine;
using System.Collections;

public class DeleteOnInvisible : MonoBehaviour {
	
	void OnBecameInvisible()
	{
		Destroy (this.gameObject);
	}

}
