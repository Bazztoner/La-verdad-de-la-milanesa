using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class SmoothWaypointWalker : MonoBehaviour
{
    public Waypoint current;
    public float movementSpeed;

    Rigidbody _rb;

    Vector3 lastWaypointPosition;
    Vector3 bezierStart;
    Vector3 bezierEnd;
    float bezierT;
    float bezierSpeed;

    Vector3 handle1;
    Vector3 handle2;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        lastWaypointPosition = transform.position;
        UpdateBezierParams();
    }

    void Update()
    {
        if (current == null)
            return;

        var movement = current.IsNear(transform.position) ?
            BezierMovement() : RegularMovement();

        //Move
        _rb.MovePosition(_rb.position + movement);
        transform.forward = movement;

        //Get next waypoint if necessary
        if (bezierT >= 1.0f && current.Next != null)
        {
            lastWaypointPosition = current.transform.position;
            current = current.Next;
            UpdateBezierParams();
        }
        else if (bezierT >= 1.0f && current.Next == null)
        {
            current = null;
        }
    }

    Vector3 BezierMovement()
    {
        bezierT += Mathf.Min(1.0f, bezierSpeed * Time.deltaTime);

        handle1 = Vector3.Lerp(bezierStart, current.transform.position, bezierT);
        handle2 = Vector3.Lerp(current.transform.position, bezierEnd, bezierT);
        var bezierPoint = Vector3.Lerp(handle1, handle2, bezierT);

        return bezierPoint - transform.position;
    }

    Vector3 RegularMovement()
    {
        var toWaypoint = current.transform.position - transform.position;
        var direction = toWaypoint.normalized;
        var movementDelta = direction * movementSpeed * Time.deltaTime;

        return movementDelta.sqrMagnitude > toWaypoint.sqrMagnitude ? toWaypoint : movementDelta;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(lastWaypointPosition, 0.5f);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(bezierStart, 0.5f);
        Gizmos.DrawWireSphere(bezierEnd, 0.5f);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(handle1, 0.25f);
        Gizmos.DrawWireSphere(handle2, 0.25f);
    }

    void UpdateBezierParams()
    {
        var currWaypointPosition = current.transform.position;

        bezierT = 0;
        bezierStart = currWaypointPosition + (lastWaypointPosition - currWaypointPosition).normalized * current.NearDistance;

        if (current.Next == null)
        {
            bezierEnd = currWaypointPosition;
            bezierSpeed = movementSpeed / current.NearDistance;
        }
        else
        {
            var nextWaypointPosition = current.Next.transform.position;
            bezierEnd = currWaypointPosition + (nextWaypointPosition - currWaypointPosition).normalized * current.NearDistance;
            bezierSpeed = movementSpeed / current.NearDistance * 0.5f;
        }
    }
}
