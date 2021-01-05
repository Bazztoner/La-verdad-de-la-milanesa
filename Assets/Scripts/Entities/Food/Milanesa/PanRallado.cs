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
        if (!station.HasFullTray())
        {
            if (GameManager.Instance.HasEnoughMoney(moneyCost))
            {
                station.FillTrayWithPanRallado();
                GameManager.Instance.AddMoneyValue(-moneyCost);
                GameManager.Instance.SpawnMoneyPrompt(this.transform.position, -moneyCost);
            }
            else print("NO HAY PLATA!");
        }
        else print("BANDEJA LLENA!");
    }

    public virtual void ActivateHighlight(bool state)
    {
        if (_rend != null) _rend.material.SetFloat("_Highlighted", state ? 1f : 0f);
    }

}
