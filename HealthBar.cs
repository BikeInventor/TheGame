using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {

	public Player player;
	public Transform foregroundSprite;
	public SpriteRenderer foregroundRenderer;
	public Color maxHealthColor = new Color (255/255f, 63/255f, 63/255f);
	public Color minHealthColor = new Color (64/255f, 137/255f, 255/255f);

	void Update () 
	{
		if (player == null)
			return;
		float healthPercent = player.playerStats.health / (float)player.playerStats.maxHealth;

		foregroundSprite.localScale = new Vector3 (healthPercent, 1, 1);
		foregroundRenderer.color = Color.Lerp (maxHealthColor, minHealthColor, healthPercent);
	}
}
