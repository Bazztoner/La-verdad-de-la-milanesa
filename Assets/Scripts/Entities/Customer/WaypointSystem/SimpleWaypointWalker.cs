using UnityEngine;
using System.Collections;

public class SimpleWaypointWalker : MonoBehaviour
{
    public Waypoint current;
    public float movementSpeed;

    Rigidbody _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (current == null)
        {
            _rb.velocity = Vector3.zero;
            return;
        }

        var toWaypoint = current.transform.position - _rb.position;
        var direction = toWaypoint.normalized;
        var movementDelta = direction * movementSpeed * Time.fixedDeltaTime;
        var adjustedMovementDelta = movementDelta.sqrMagnitude > toWaypoint.sqrMagnitude ? toWaypoint : movementDelta;

        transform.forward = direction;
        _rb.MovePosition(_rb.position + adjustedMovementDelta);

        if ((current.transform.position - transform.position).sqrMagnitude < .1f)
            current = current.Next;
    }
}
