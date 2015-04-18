using UnityEngine;
using System.Collections;

public class HealthBarStaticGun : MonoBehaviour {
	
	public Transform foregroundSprite;
	public SpriteRenderer foregroundRenderer;
	public Color maxHealthColor = new Color (255/255f, 63/255f, 63/255f);
	public Color minHealthColor = new Color (64/255f, 137/255f, 255/255f);
	private PlayerSearching searchingScript;

	void Awake () 
	{
		searchingScript = this.transform.parent.gameObject.GetComponentInChildren <PlayerSearching> ();
	}
		
	void Update () 
	{
		float healthPercent = searchingScript.health / (float)searchingScript.maxHealth;

		foregroundSprite.localScale = new Vector3 (healthPercent, 1, 1);
		foregroundRenderer.color = Color.Lerp (maxHealthColor, minHealthColor, healthPercent);
	}
}
