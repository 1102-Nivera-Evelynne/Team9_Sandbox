using UnityEngine;

public class XRGravityController : MonoBehaviour
{
    public float gravityMultiplier = 9.81f;  // Standard gravity value
    public float moveSpeed = 3.0f;  // Walking speed
    public Terrain terrain;  // Reference to terrain
    private CharacterController characterController;
    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        // Optionally set terrain if not set in inspector
        if (!terrain)
            terrain = Terrain.activeTerrain;
    }

    void Update()
    {
        // Check if the character is grounded
        isGrounded = characterController.isGrounded;

        // If grounded, reset the vertical velocity
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;  // Small value to keep on the ground
        }

        // Apply gravity manually if not grounded
        if (!isGrounded)
        {
            velocity.y += gravityMultiplier * Time.deltaTime;
        }
        else
        {
            velocity.y = 0;  // Stop gravity effect when on the ground
        }

        // Get the character's current position and find the terrain height
        Vector3 currentPosition = transform.position;
        float terrainHeight = terrain.SampleHeight(currentPosition);  // Returns height of terrain at current position

        // Set the character's y position to be just above the terrain height
        currentPosition.y = terrainHeight + characterController.radius;

        // Get input movement (optional if using your own input system)
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(horizontal, 0, vertical);

        // Convert movement direction to world space
        moveDirection = transform.TransformDirection(moveDirection);

        // Move the character based on user input
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);

        // Apply the vertical velocity (gravity)
        characterController.Move(velocity * Time.deltaTime);

        // Update the character position, ensuring it follows terrain surface height
        transform.position = currentPosition;
    }
}
