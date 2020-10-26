using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EggFood : PickupBase
{
    Renderer[] _rends;
    FoodStationBase _stationParent;

    protected override void Awake()
    {
        base.Awake();
        _rends = GetComponentsInChildren<Renderer>();
    }

    public override void ActivateHighlight(bool state)
    {
        foreach (var renderer in _rends)
        {
            renderer.material.SetFloat("_Highlighted", state ? 1f : 0f);
        }
    }
}
