using UnityEngine;
using System.Collections;


public class RemoveAfterTime : MonoBehaviour {

	public float delayBeforeRemoved = 1;
	
	void Start () {
		Destroy (this.gameObject, delayBeforeRemoved);
	}

}
