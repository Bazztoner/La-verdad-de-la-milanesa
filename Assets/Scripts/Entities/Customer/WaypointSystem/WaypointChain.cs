using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WaypointChain : MonoBehaviour
{
    public Waypoint startWaypoint;
    public CustomerBase currentCustimer;

    void Awake()
    {
        startWaypoint = GetComponentsInChildren<Waypoint>().First(Matrix4x4 => Matrix4x4.gameObject.name == "WP_PatrolInStart");
    }

    public void SetCustomer(CustomerBase customer)
    {
        customer.SetWaypoint(this);
        currentCustimer = customer;
    }
    
    public void OnFinishedCustomer()
    {
        currentCustimer = null;

        GameManager.Instance.OnFinishedCustomer(this);
    }
}
