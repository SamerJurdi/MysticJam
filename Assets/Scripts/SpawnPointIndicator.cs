using UnityEngine;

public class SpawnPointIndicator : MonoBehaviour
{
    public Color gizmoColor = Color.red;
    public float gizmoRadius = 0.2f;

    // This method is called only in the editor and draws the gizmo
    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawSphere(transform.position, gizmoRadius);
    }
}
