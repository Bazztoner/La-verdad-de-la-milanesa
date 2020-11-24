using UnityEngine;
using System.Collections;

public class SmoothedWaypointMovement : MonoBehaviour
{
    //Waypoint actual y velocidad de movimiento
    public Waypoint currentWaypoint;
    public float speed;

    //Autoexplicativo
    Rigidbody _rb;

    Vector3 _lastWaypointPos;
    Vector3 _bezierStart;
    Vector3 _bezierEnd;
    //Coeficiente de giro?
    float _bezierT;
    float _bezierSpeed;

    Vector3 _handle1;
    Vector3 _handle2;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _lastWaypointPos = transform.position;
        UpdateBezierParameters();
    }

    void FixedUpdate()
    {
        if (currentWaypoint == null) return;

        //Movimiento absoluto. Si el movimiento es mayor al waypoint, manda al waypoint
        var movement = currentWaypoint.IsNear(transform.position) ? BezierMovement() : RegularMovement();
        _rb.MovePosition(_rb.position + movement);

        //Agarrar el próximo waypoint
        if (_bezierT >= 1.0f && currentWaypoint.Next != null)
        {
            _lastWaypointPos = currentWaypoint.transform.position;
            currentWaypoint = currentWaypoint.Next;
            UpdateBezierParameters();
        }
        else if (_bezierT >= 1.0f && currentWaypoint.Next == null) currentWaypoint = null;
    }

    Vector3 BezierMovement()
    {
        _bezierT += Mathf.Min(1.0f, _bezierSpeed * Time.deltaTime);

        _handle1 = Vector3.Lerp(_bezierStart, currentWaypoint.transform.position, _bezierT);
        _handle2 = Vector3.Lerp(_bezierEnd, currentWaypoint.transform.position, _bezierT);

        var bezierPoint = Vector3.Lerp(_handle1, _handle2, _bezierT);

        return bezierPoint - transform.position;
    }

    Vector3 RegularMovement()
    {
        var toWaypoint = currentWaypoint.transform.position - transform.position;
        var direction = toWaypoint.normalized;
        var movementDelta = direction * speed * Time.deltaTime;

        return movementDelta.sqrMagnitude > toWaypoint.sqrMagnitude ? toWaypoint : movementDelta;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_lastWaypointPos, 0.5f);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_bezierStart, 0.5f);
        Gizmos.DrawWireSphere(_bezierEnd, 0.5f);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_handle1, 0.25f);
        Gizmos.DrawWireSphere(_handle2, 0.25f);
    }

    void UpdateBezierParameters()
    {
        var currentWaypointPos = currentWaypoint.transform.position;

        _bezierT = 0;
        _bezierStart = currentWaypointPos + (_lastWaypointPos - currentWaypointPos).normalized * currentWaypoint.NearDistance;

        if (currentWaypoint.Next == null)
        {
            _bezierEnd = currentWaypointPos;
            _bezierSpeed = speed / currentWaypoint.NearDistance;
        }
        else
        {
            var nextWaypointPosition = currentWaypoint.Next.transform.position;
            _bezierEnd = currentWaypointPos + (nextWaypointPosition - currentWaypointPos).normalized * currentWaypoint.NearDistance;
            _bezierSpeed = speed / currentWaypoint.NearDistance * 0.5f;
        }
    }
}