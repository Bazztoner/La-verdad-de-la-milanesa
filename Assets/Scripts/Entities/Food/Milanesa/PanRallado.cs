using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PanRallado : MonoBehaviour, IInteractuable
{
    protected PlayerController _player;
    protected Renderer _rend;
    public MilanesaStation station;

    protected virtual void Start()
    {
        _player = FindObjectOfType<PlayerController>();
        _rend = GetComponentInChildren<Renderer>();
        station = GetComponentInParent<MilanesaStation>();
    }

    public void Interact()
    {
        station.FillTrayWithPanRallado();
    }

    public virtual void ActivateHighlight(bool state)
    {
        if (_rend != null) _rend.material.SetFloat("_Highlighted", state ? 1f : 0f);
    }

}
