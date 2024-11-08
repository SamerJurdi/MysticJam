using UnityEngine;

public class TomeController : MonoBehaviour
{
    public Transform player;
    public Transform floatingObject;
    public float moveSpeed = 5f;
    public float distanceFromPlayer = 1.5f;

    private Vector3 offset;

    void Start()
    {
        // Calculate the initial offset of the floating object from the player
        offset = floatingObject.position - player.position;
    }

    void Update()
    {
        // Get the mouse position in world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;  // Make sure we're working in 2D, so set Z to 0

        // Calculate the direction from the player to the mouse
        Vector3 directionToMouse = mousePosition - player.position;

        // Normalize the direction to get only the direction vector (ignoring the magnitude)
        directionToMouse.Normalize();

        // Update the floating object's position relative to the player, offset by the calculated direction
        floatingObject.position = player.position + directionToMouse * distanceFromPlayer;

        // Optionally, smooth the movement for a floating effect
        floatingObject.position = Vector3.Lerp(floatingObject.position, player.position + directionToMouse * distanceFromPlayer, moveSpeed * Time.deltaTime);
    }
}
