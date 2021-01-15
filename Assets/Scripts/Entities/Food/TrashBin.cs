using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TrashBin : MonoBehaviour, IInteractuable
{
    public Transform trashPos;
    protected PlayerController _player;
    protected Renderer _rend;

    protected virtual void Awake()
    {
        _rend = GetComponentInChildren<Renderer>();
    }

    protected virtual void Start()
    {
        _player = FindObjectOfType<PlayerController>();
    }

    public void Interact()
    {
        if (_player.itemPickup)
        {
            _player.ForceDepositObject(trashPos);
        }
    }

    public void ActivateHighlight(bool state)
    {
        _rend.material.SetFloat("_Highlighted", state ? 1f : 0f);
    }

}
