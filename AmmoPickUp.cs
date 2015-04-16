using UnityEngine;
using System.Collections;

public class AmmoPickUp : MonoBehaviour {

	public AudioClip ammoPickedUp;
	Inventory inventory;

	void Awake()
	{
		inventory = GameObject.Find ("_GM").GetComponent<Inventory>();
	}

	void OnTriggerEnter2D (Collider2D info) 
	{
		if (info.tag == "Player") 
		{
			var Guns = GameObject.FindGameObjectsWithTag("Gun");
			foreach(var gun in Guns)
			{
				int bulletsFromTheBox = 0;
				System.String wType = gun.GetComponent<Weapon>().weaponType;

				switch (wType)
				{
					case "Pistol":
					{
					    bulletsFromTheBox =  Random.Range(2,5)*10;
						break;
					}
					case "Rifle":
					{
						bulletsFromTheBox =  Random.Range(3,5)*10;
						break;
					}
					case "MachineGun":
					{
						bulletsFromTheBox =  Random.Range(5,7)*10;
						break;
					}
				}
				inventory.SetValueByName(wType, inventory.GetValueByName(wType) + bulletsFromTheBox);
			}
			//TODO:определять какому оружию сколько добавить по атрибуту weaponType в скрипте Weapon.cs
			AudioSource.PlayClipAtPoint(ammoPickedUp, this.transform.position);
			Destroy(this.gameObject);
		}
	}
}
