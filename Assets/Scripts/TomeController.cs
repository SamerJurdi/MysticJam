using UnityEngine;

public class TomeController : MonoBehaviour
{
    public Transform player;
    public Transform floatingObject;
    public float distanceFromPlayer = 2f;
    public float hoverHeight = 0.5f;
    public float hoverSpeed = 2f;
    public float transitionSpeed = 2f;

    private Vector3 offset;
    private Vector3 targetDirection;       // The current direction the floating object is facing
    private Vector3 currentDirection;      // The current direction the object is moving in

    void Start()
    {
        // Calculate the initial offset of the floating object from the player
        offset = floatingObject.position - player.position;
        targetDirection = offset.normalized;
        currentDirection = targetDirection;
    }

    void Update()
    {
        // Get the mouse position in world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        // Calculate the difference between mouse position and player position
        Vector3 directionToMouse = mousePosition - player.position;

        // Create a vector that only keeps the X or Y direction, ensuring movement is only horizontal or vertical
        Vector3 newDirection = Vector3.zero;

        if (Mathf.Abs(directionToMouse.x) > Mathf.Abs(directionToMouse.y))
        {
            // Move horizontally
            newDirection.x = directionToMouse.x > 0 ? 1 : -1;
        }
        else
        {
            // Move vertically
            newDirection.y = directionToMouse.y > 0 ? 1 : -1;
        }

        // If the direction has changed, transition smoothly to the new direction
        if (newDirection != currentDirection)
        {
            targetDirection = newDirection;
        }

        // Smoothly transition towards the new direction
        currentDirection = Vector3.Lerp(currentDirection, targetDirection, transitionSpeed * Time.deltaTime);

        // Apply movement in the determined direction, at a fixed distance from the player
        floatingObject.position = player.position + currentDirection * distanceFromPlayer;

        // Apply hovering effect
        float hover = Mathf.Sin(Time.time * hoverSpeed) * hoverHeight;
        floatingObject.position = new Vector3(floatingObject.position.x, floatingObject.position.y + hover, floatingObject.position.z);
    }
}
