using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AjoPerejil : PickupBase
{
    Renderer[] _rends;
    FoodStationBase _stationParent;

    public Transform normalMeshes, choppedMeshes;

    public bool isChopped;

    protected override void Awake()
    {
        base.Awake();
        _rends = normalMeshes.GetComponentsInChildren<Renderer>();

        normalMeshes.gameObject.SetActive(true);
        choppedMeshes.gameObject.SetActive(false);
    }

    public override void ActivateHighlight(bool state)
    {
        foreach (var renderer in _rends)
        {
            renderer.material.SetFloat("_Highlighted", state ? 1f : 0f);
        }
    }

    public override void SendStateToParent()
    {
        if (_stationParent != null) _stationParent.FoodGotPulled(this);
    }

    public override void SendFoodStationInfo(FoodStationBase station)
    {
        _stationParent = station;
    }

    public void OnChoppedVeggies()
    {
        normalMeshes.gameObject.SetActive(false);
        ActivateHighlight(false);

        choppedMeshes.gameObject.SetActive(true);
        _rends = choppedMeshes.GetComponentsInChildren<Renderer>();
        isChopped = true;
    }
}
