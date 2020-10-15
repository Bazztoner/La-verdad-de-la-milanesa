using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MilanesaStation : FoodStationBase, IInteractuable
{
    public int maxBreadCharges = 3;
    int _currentCharges;

    public Transform milanesaPosition;
    public Milanesa currentMilanga;
    public MilanesaMinigame minigame;

    public bool inMinigame;

    public int CurrentCharges
    {
        get => _currentCharges;
        private set => _currentCharges = Mathf.Clamp(value, 0, maxBreadCharges);
    }

    protected override void Start()
    {
        milanesaPosition = transform.Find("MilanesaPosition");
        minigame = FindObjectOfType<Canvas>().GetComponentInChildren<MilanesaMinigame>(true);
        CurrentCharges = maxBreadCharges;
        base.Start();
    }

    public override void Interact()
    {
        if (currentMilanga == null)
        {
            if (_player.itemPickup is Milanesa)
            {
                currentMilanga = _player.itemPickup as Milanesa;
                _player.ForceDepositObject(milanesaPosition);
            }
        }
        else
        {
            //start minigame if pan rallado > 0
            if (CurrentCharges > 0)
            {
                StartMinigame();
            }

        }
    }

    public void OnClickMilanesa()
    {
        currentMilanga.OnClickMilanesa();
        if (currentMilanga.IsCooked()) EndMinigame();
    }

    public void OnClickTurnOver()
    {
        currentMilanga.TurnMilanesa();
    }

    void StartMinigame()
    {
        inMinigame = true;
        _player.SetOnMinigame(true);
        minigame.gameObject.SetActive(true);
        minigame.Init(this);
    }

    public void EndMinigame()
    {
        if (currentMilanga.IsCooked())
        {
            /*give milanga to chaboncito*/
            CurrentCharges--;
            currentMilanga = null;
        }

        inMinigame = false;
        _player.SetOnMinigame(false);
        minigame.gameObject.SetActive(false);
    }
}
