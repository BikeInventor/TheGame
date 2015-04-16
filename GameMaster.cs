using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour {
		
	public static GameMaster gm;
	public Transform playerPrefab;
	public Transform spawnPoint;
	public int spawnDelay = 2;
	public Transform spawnPrefab;

	//================================WEAPONS LIST==========================
	public Texture pistolTexture;
	public Texture rifleTexture;
	public Texture machineGunTexture;
	//======================================================================
	public Transform pistolPrefab;
	private Inventory inventory;

	void Start () 
	{
		Screen.showCursor = false;
		if (gm == null) 
		{
			gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
		}
		inventory = GameObject.Find ("_GM").GetComponent<Inventory> ();
	}

	public IEnumerator RespawnPlayer () 
	{
		yield return new WaitForSeconds (spawnDelay);

		if (GameObject.FindObjectOfType<Player> () == null) 
		{
			Instantiate (playerPrefab, spawnPoint.position, spawnPoint.rotation);
			inventory.SetValueByName("Pistol", 50);
			GameObject clone = Instantiate (spawnPrefab, spawnPoint.position, spawnPoint.rotation) as GameObject;
			Destroy (GameObject.Find("SpawnParticles(Clone)"), 3f);
		}
	}

	public static void KillPlayer(Player player) 
	{
		Destroy (player.gameObject);
		gm.StartCoroutine (gm.RespawnPlayer ());
	}

	void OnGUI()
	{
		int itemCount = inventory.GetActiveItemValue ();
		Texture itemTexture = new Texture();
		int bulletsInMagazine = 0;
		int currentMagazine = 0;
		int otherMagazines = 0;
		System.String activeItemName = inventory.GetActiveItemName ();

		bulletsInMagazine = GameObject.Find (activeItemName).GetComponent<Weapon>().bulletsInMagazine;
		currentMagazine = itemCount % bulletsInMagazine;
		otherMagazines = itemCount - currentMagazine;
		if (currentMagazine == 0 && otherMagazines != 0) 
		{
			currentMagazine += bulletsInMagazine;
			otherMagazines -= bulletsInMagazine;
		}

		switch (inventory.GetActiveItemName()) 
		{
			case "Pistol":
			{
				itemTexture = pistolTexture;
				break;
			}
			case "Rifle":
			{
				itemTexture = rifleTexture;
				break;
			}
			case "MachineGun":
			{
				itemTexture = machineGunTexture;
				break;
			}
		}
		GUI.skin.box.fontSize = 20;
		GUI.skin.box.imagePosition = ImagePosition.ImageAbove;
		GUI.Box (new Rect (1, 1, 250f, 150f), itemTexture);
		GUI.Box (new Rect (1, 150, 250f, 30f), "Ammo: " + currentMagazine + "/" + otherMagazines);
	}
	
}
