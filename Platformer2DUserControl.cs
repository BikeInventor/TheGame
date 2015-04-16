using UnityEngine;
using UnitySampleAssets.CrossPlatformInput;

namespace UnitySampleAssets._2D
{

    [RequireComponent(typeof (PlatformerCharacter2D))]
    public class Platformer2DUserControl : MonoBehaviour
    {
        private PlatformerCharacter2D character;
        private bool jump;

        private void Awake()
        {
            character = GetComponent<PlatformerCharacter2D>();
        }

        private void Update()
        {
            if (!jump)
            	jump = CrossPlatformInputManager.GetButtonDown("Jump");
        }

        private void FixedUpdate()
        {
            bool crouch = Input.GetKey(KeyCode.LeftControl);
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            character.Move(h, crouch, jump);
            jump = false;
        }
    }
}