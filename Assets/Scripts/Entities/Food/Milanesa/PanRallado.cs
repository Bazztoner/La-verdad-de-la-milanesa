using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PanRallado : MonoBehaviour, IInteractuable
{
    protected PlayerController _player;
    protected Renderer _rend;
    public MilanesaStation station;
    public int moneyCost;
    protected virtual void Start()
    {
        _player = FindObjectOfType<PlayerController>();
        _rend = GetComponentInChildren<Renderer>();
        station = GetComponentInParent<MilanesaStation>();
    }

    public void Interact()
    {
        if (GameManager.Instance.HasEnoughMoney(moneyCost) && !station.HasFullTray())
        {
            station.FillTrayWithPanRallado();
            GameManager.Instance.AddMoneyValue(-moneyCost);
            GameManager.Instance.SpawnMoneyPrompt(this.transform.position, -moneyCost);
        }
        else print("NO HAY PLATA!");
    }

    public virtual void ActivateHighlight(bool state)
    {
        if (_rend != null) _rend.material.SetFloat("_Highlighted", state ? 1f : 0f);
    }

}
