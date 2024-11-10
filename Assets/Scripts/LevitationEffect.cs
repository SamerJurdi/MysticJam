using UnityEngine;

public class LevitationEffect : MonoBehaviour
{
    // Adjustable parameters for levitation
    [Tooltip("The maximum height it moves up/down.")]
    public float levitationHeight = 0.125f;
    [Tooltip("The speed at which it levitates")]
    public float levitationSpeed = 1.25f;

    private Vector3 initialPosition;

    void Start()
    {
        // Store the initial position of the item
        initialPosition = transform.position;
    }

    void Update()
    {
        // Calculate the new Y position using sine wave
        float newY = initialPosition.y + Mathf.Sin(Time.time * levitationSpeed) * levitationHeight;

        // Apply the new position to the item
        transform.position = new Vector3(initialPosition.x, newY, initialPosition.z);
    }
}
