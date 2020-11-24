using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PausedWaypointWalker : MonoBehaviour
{
    public Waypoint current;
    public float movementSpeed;
    public float pauseTime = 1f;
    public float rotationTime = 0.5f;

    Rigidbody _rb;

    enum State { Walking, Paused, Rotating }

    State state;
    float time;

    Quaternion from;
    Quaternion to;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        state = State.Walking;
    }

    void Update()
    {
        if (current == null)
            return;

        //Update
        switch (state)
        {
            case State.Walking:
                Walk();
                break;
            case State.Paused:
                Pause();
                break;
            case State.Rotating:
                Rotate();
                break;
        }

        //Next state
        switch (state)
        {
            case State.Walking:
                if (WalkFinished())
                    state = State.Paused;

                break;
            case State.Paused:
                if (PauseFinished())
                {
                    state = State.Rotating;
                    time = 0;
                    EnterRotation();
                }

                break;
            case State.Rotating:
                if (RotateFinished())
                {
                    state = State.Walking;
                    time = 0;
                    current = current.Next;
                }

                break;
        }
    }

    void Walk()
    {
        var toWaypoint = current.transform.position - transform.position;
        var direction = toWaypoint.normalized;
        var movementDelta = direction * movementSpeed * Time.deltaTime;
        var adjustedMovementDelta = movementDelta.sqrMagnitude > toWaypoint.sqrMagnitude ? toWaypoint : movementDelta;

        _rb.MovePosition(_rb.position + adjustedMovementDelta);
        transform.forward = direction;
    }

    void Pause()
    {
        time += Time.deltaTime;
    }

    void Rotate()
    {
        time = Mathf.Min(rotationTime, time + Time.deltaTime);

        transform.rotation = Quaternion.Lerp(from, to, time / rotationTime);
    }

    void EnterRotation()
    {
        from = Quaternion.LookRotation(transform.forward, Vector3.up);

        if (current.Next == null)
        {
            to = from;
            return;
        }

        var currPos = current.transform.position;
        var nextPos = current.Next != null ? current.Next.transform.position : currPos;

        to = Quaternion.LookRotation(nextPos - currPos, Vector3.up);
    }

    bool WalkFinished()
    {
        return (current.transform.position - transform.position).sqrMagnitude < 0.01f;
    }

    bool PauseFinished()
    {
        return time >= pauseTime;
    }

    bool RotateFinished()
    {
        return time >= rotationTime;
    }
}
