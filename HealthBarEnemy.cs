using UnityEngine;
using System.Collections;

public class HealthBarEnemy : MonoBehaviour {
	
	public Transform foregroundSprite;
	public SpriteRenderer foregroundRenderer;
	public Color maxHealthColor = new Color (255/255f, 63/255f, 63/255f);
	public Color minHealthColor = new Color (64/255f, 137/255f, 255/255f);
	private Enemy_AI aiScript;
	// Use this for initialization
	void Start () 
	{
		aiScript = this.GetComponentInParent<Enemy_AI> ();
	}
		
	// Update is called once per frame
	void Update () 
	{
		float healthPercent = aiScript.health / (float)aiScript.maxHealth;

		foregroundSprite.localScale = new Vector3 (healthPercent, 1, 1);
		foregroundRenderer.color = Color.Lerp (maxHealthColor, minHealthColor, healthPercent);
	}
}
