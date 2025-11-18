using UnityEngine;
namespace SceneScript
{
    // This leftover file was created during a previous rename operation.
    // Renamed class to avoid collisions; file can be removed if not needed.
    public class ScenePlayerMovement_Old : MonoBehaviour
    {
        public float moveSpeed = 5f; // Player movement speed
        private Rigidbody2D rb; // Reference to the Rigidbody2D for physics-based movement

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component attached to the object
        }

        private void Update()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            Vector2 movement = new Vector2(horizontal, vertical) * moveSpeed;
            rb.linearVelocity = movement;
        }
    }
}
