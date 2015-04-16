using UnityEngine;

namespace UnitySampleAssets._2D
{

    public class PlatformerCharacter2D : MonoBehaviour
    {   
        [SerializeField] private float maxSpeed = 10f; // The fastest the player can travel in the x axis.
        [SerializeField] private float jumpForce = 400f; // Amount of force added when the player jumps.	
		[Range(0, 1)] [SerializeField] private float crouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
        [SerializeField] private bool airControl = false; // Whether or not a player can steer while jumping;
        [SerializeField] private LayerMask whatIsGround; // A mask determining what is ground to the character
		[SerializeField] public AudioClip jumpSound;

		//====================================PLAYER_STATE===================
        private Transform groundCheck; // A position marking where to check if the player is grounded.
        private float groundedRadius = .2f; // Radius of the overlap circle to determine if grounded
        public bool grounded = false; // Whether or not the player is grounded.
        private Transform ceilingCheck; // A position marking where to check for ceilings
        private float ceilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
		public bool facingRight = true; // For determining which way the player is currently facing.
		private bool isSitting;
		public bool isAiming;
		private Transform playerGraphics; 		// Ссылка на graphics для измен. направл.
		public Animator anim; // Reference to the player's animator component.
		private BoxCollider2D boxCol;
		private CircleCollider2D circleCol;

		private Inventory inventory;

		//==================================SPRITES==========================
		private GameObject asArmWithWeapon;
		private GameObject noWeaponHand;
		private GameObject noWeaponFist;
		private GameObject crossHair;

		//================================WEAPONS_SPRITES====================
		private GameObject pistol;
		private GameObject idlePistol;

		private GameObject rifle;
		private GameObject idleRifle;

		private GameObject machineGun;
		private GameObject idleMachineGun;
		//===================================================================

      private void Awake()
        {
			SetUpReferences (); 
        }

		void Start()
		{
			SetPistol ();
		}

		private void SetUpReferences()
		{
			groundCheck = transform.Find("GroundCheck");
			ceilingCheck = transform.Find("CeilingCheck");
			playerGraphics = transform.FindChild ("Graphics");		
			asArmWithWeapon = GameObject.Find ("AsArm");
			noWeaponHand = GameObject.FindGameObjectWithTag ("NoWeapon1");
			noWeaponFist = GameObject.FindGameObjectWithTag ("NoWeapon2");

			anim = playerGraphics.GetComponent<Animator> ();
			inventory = GameObject.Find ("_GM").GetComponent<Inventory> ();
			boxCol = this.GetComponent<BoxCollider2D>();
			circleCol = this.GetComponent<CircleCollider2D>();
			crossHair = GameObject.Find ("Crosshair");

			//==================INIT WEAPONS===========================
			pistol = GameObject.Find ("Pistol");
			idlePistol = GameObject.FindGameObjectWithTag ("IdlePistol");
			
			rifle = GameObject.Find ("Rifle");
			idleRifle = GameObject.Find ("IdleRifle");

			machineGun = GameObject.Find ("MachineGun");
			idleMachineGun = GameObject.Find ("IdleMachineGun");
			//==========================================================
		}

        private void FixedUpdate()
        {
            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
            grounded = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsGround);
            anim.SetBool("Ground", grounded);
			anim.SetBool("FacingRight", facingRight);
			anim.SetFloat("vSpeed", Mathf.Abs(rigidbody2D.velocity.y));		
		}

		public void Update()
		{
			HideDisabledItems ();
			WeaponChoose ();
			Aiming ();
			Sitting ();
		}

        public void Move(float move, bool crouch, bool jump)
        {
			isSitting = crouch;

            if (!crouch && anim.GetBool("Crouch"))
            {
                if (Physics2D.OverlapCircle(ceilingCheck.position, ceilingRadius, whatIsGround))
                    crouch = true;
            }
            anim.SetBool("Crouch", crouch);
            if (grounded || airControl)
            {
                move = (crouch ? /*move*crouchSpeed*/0 : move);
                anim.SetFloat("Speed", Mathf.Abs(move));
                rigidbody2D.velocity = new Vector2(move*maxSpeed, rigidbody2D.velocity.y);
                if (move > 0 && !facingRight)
                    Flip();
                else if (move < 0 && facingRight)
                    Flip();
            }
            if (grounded && jump && anim.GetBool("Ground"))
            {
                grounded = false;
                anim.SetBool("Ground", false);
                rigidbody2D.AddForce(new Vector2(0f, jumpForce));
				audio.clip = jumpSound;
				if (!audio.isPlaying) 
				{
					audio.Play();
				}
            }
        } 

        private void Flip()
        {
			//Флипаем руку
			asArmWithWeapon.transform.localScale = 
				new Vector3 (
						asArmWithWeapon.transform.localScale.x, 
						asArmWithWeapon.transform.localScale.y * (-1),
						asArmWithWeapon.transform.localScale.z
					);

			//Смещение руки относительно тела при повороте (чтоб не отваливалась от плеча)
			float diff = asArmWithWeapon.transform.localPosition.x * -1f;
			asArmWithWeapon.transform.localPosition = 
				new Vector3 (
					diff,
					asArmWithWeapon.transform.localPosition.y,
					asArmWithWeapon.transform.localPosition.z
					);

			facingRight = !facingRight;

			Vector3 playerScale = playerGraphics.localScale;
			playerScale.x *= -1;
			playerGraphics.localScale = playerScale;
		}

		private void Sitting()
		{
			if (isSitting) 
			{
				circleCol.center = new Vector2(-0.12f,-1.8f);
				circleCol.radius = 0.42f;
				boxCol.size = new Vector2(0.88f, 1.5f);
				asArmWithWeapon.transform.localPosition = new Vector3(facingRight?0.3f:-0.3f,-0.1f,0);
				
			} 
			else 
			{
				circleCol.center = new Vector2(-0.12f,-2.22f);
				circleCol.radius = 0.47f;
				boxCol.size = new Vector2(0.88f, 2.86f);
				if (asArmWithWeapon)
					asArmWithWeapon.transform.localPosition = new Vector3(facingRight?-0.13f:0.13f,0.2f,0);
			}
		}

		private void Aiming()
		{
			//Если зажата ПКМ
			if (Input.GetMouseButton (1)) 
			{
				ShootingEnabled ();
			} 
			else 
			{
				ShootingDisabled ();
			}
		}

		public void ShootingEnabled()
		{
			isAiming = true;
			if (asArmWithWeapon!=null)
			{
				asArmWithWeapon.renderer.enabled = true;
				crossHair.renderer.enabled = true;
				ShowActiveWeapon();
			}
			noWeaponHand.renderer.enabled = false;
			noWeaponFist.renderer.enabled = false;
		}
		
		public void ShootingDisabled()
		{
			isAiming = false;
			if (asArmWithWeapon!=null)
			{
				asArmWithWeapon.renderer.enabled = false;
				crossHair.renderer.enabled = false;
				//====================================VISIBLE WEAPONS HERE=========================
				pistol.renderer.enabled = false;
				rifle.renderer.enabled = false;
				machineGun.renderer.enabled = false;
				//=================================================================================
			}
			noWeaponHand.renderer.enabled = true;
			noWeaponFist.renderer.enabled = true;

			ShowUnActiveWeapon ();
		}

		private void ShowActiveWeapon()
		{
			System.String activeItemName = inventory.GetActiveItemName ();
			switch (activeItemName)
			{
				case "Pistol" :
				{
					pistol.renderer.enabled = true;
					idlePistol.renderer.enabled = false;
					break;
				}
				case "Rifle" :
				{
					rifle.renderer.enabled = true;
					idleRifle.renderer.enabled = false;
					break;
				}
				case "MachineGun":
				{
					machineGun.renderer.enabled = true;
					idleMachineGun.renderer.enabled = false;
					break;
				}
			}
		}

		public void ShowUnActiveWeapon()
		{
			System.String activeItemName = inventory.GetActiveItemName ();
			switch (activeItemName)
			{
				case "Pistol" :
				{
					idlePistol.renderer.enabled = true;
					break;
				}
				case "Rifle" :
				{
					if (inventory.IsExist("Rifle"))
						idleRifle.renderer.enabled = true;
					break;
				}
				case "MachineGun" :
				{
					if (inventory.IsExist("MachineGun"))
						idleMachineGun.renderer.enabled = true;
					break;
				}
			}
		}

		private void WeaponChoose()
		{
			if (isAiming) return;

			if (Input.GetKeyUp ("1")) 
			{
				if (inventory.IsExist("Pistol"))
					SetPistol();
			}
			if (Input.GetKeyUp ("2")) 
			{
				if (inventory.IsExist("Rifle"))
					SetRifle();
			}
			if (Input.GetKeyUp ("3")) 
			{
				if (inventory.IsExist("MachineGun"))
					SetMachineGun();
			}
		}

		private void DisableAllWeapons()
		{
			GameObject.Find("Pistol").GetComponent<Weapon>().enabled = false;
			GameObject.Find("Rifle").GetComponent<Weapon>().enabled = false;
			GameObject.Find("MachineGun").GetComponent<Weapon>().enabled = false;
		}

		private void SetPistol()
		{
			inventory.SetActiveByName("Pistol");
			DisableAllWeapons();
			GameObject.Find("Pistol").GetComponent<Weapon>().enabled = true;
		}

		private void SetRifle()
		{
			inventory.SetActiveByName ("Rifle");
			DisableAllWeapons();
			GameObject.Find("Rifle").GetComponent<Weapon>().enabled = true;
		}

		private void SetMachineGun()
		{
			inventory.SetActiveByName ("MachineGun");
			DisableAllWeapons();
			GameObject.Find("MachineGun").GetComponent<Weapon>().enabled = true;
		}

		private void HideDisabledItems ()
		{
			if (!inventory.IsExist("Pistol"))
				idlePistol.renderer.enabled = false;

			if (!inventory.IsExist("Rifle"))
				idleRifle.renderer.enabled = false;

			if (!inventory.IsExist("MachineGun"))
				machineGun.renderer.enabled = false;
		}
	}

}


