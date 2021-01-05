using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WaypointChain : MonoBehaviour
{
    public Waypoint startWaypoint;
    public CostumerBase currentCostumer;

    void Awake()
    {
        startWaypoint = GetComponentsInChildren<Waypoint>().First(Matrix4x4 => Matrix4x4.gameObject.name == "WP_PatrolInStart");
    }

    public void SetCostumer(CostumerBase costumer)
    {
        costumer.SetWaypoint(this);
        currentCostumer = costumer;
    }
    
    public void OnFinishedCostumer()
    {
        print("NO SE POR QUE NO ANDAAAAAAAAAAAAAAAA");
        currentCostumer = null;

        GameManager.Instance.OnFinishedCostumer(this);
    }
}
