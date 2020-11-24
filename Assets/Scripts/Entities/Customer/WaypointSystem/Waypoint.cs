using UnityEngine;
using System.Collections;

public class Waypoint : MonoBehaviour
{
    public Waypoint next;
    public float nearDistance = 0.1f;

    public Waypoint Next { get { return next; } }
    public float NearDistance { get { return nearDistance; } }

    public bool IsNear(Vector3 position)
    {
        return (position - transform.position).sqrMagnitude < nearDistance * nearDistance;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, nearDistance);

        if (next != null)
            Gizmos.DrawLine(transform.position, next.transform.position);
    }
}
