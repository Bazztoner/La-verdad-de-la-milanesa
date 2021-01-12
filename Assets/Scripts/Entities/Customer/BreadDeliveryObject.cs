using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BreadDeliveryObject : OrderDelivery
{
    Animator _an;
    Renderer[] _rends;

    protected override void Awake()
    {
        base.Awake();
        _an = GetComponent<Animator>();
        _rends = GetComponentsInChildren<Renderer>();
    }

    public override void Interact()
    {
        base.Interact();
        _an.SetBool("hasFood", foodToDeliver != null);
    }

    public override void ActivateHighlight(bool state)
    {
        foreach (var renderer in _rends)
        {
            renderer.material.SetFloat("_Highlighted", state ? 1f : 0f);
        }
    }
}
