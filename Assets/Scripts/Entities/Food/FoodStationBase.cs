using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class FoodStationBase : MonoBehaviour, IInteractuable
{
	protected PlayerController _player;
	protected Renderer _rend;

    protected virtual void Start()
    {
        _player = FindObjectOfType<PlayerController>();
        _rend = GetComponentInChildren<Renderer>();
    }

    public abstract void Interact();
    public abstract void FoodGotPulled(PickupBase food);

    public virtual void ActivateHighlight(bool state)
    {
        if(_rend != null) _rend.material.SetFloat("_Highlighted", state ? 1f : 0f);
    }
}
