using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Diagnostics;

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

    public override void FoodGotPulled()
    {
        //do we consume a charge of pan rallado when pulling the milanga, even if it's not completed?
        CurrentCharges--;
        currentMilanga = null;
    }

    public void FillTrayWithPanRallado()
    {
        CurrentCharges = maxBreadCharges;
    }

    /// <summary>
    /// DO NOT USE YET!!
    /// </summary>
    /// <param name="chargesToAdd"></param>
    public void FillTrayWithPanRallado(int chargesToAdd)
    {
        //TODO make pan rallado give X charges and that be needed to interact like them ilanesa
    }

    public void OnClickMilanesa()
    {
        currentMilanga.OnClickMilanesa();
        if (currentMilanga.IsEmpanated()) minigame.CompleteMinigame();
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
        if (currentMilanga.IsEmpanated())
        {
            _player.ForceTakeObject(currentMilanga);

            CurrentCharges--;
            currentMilanga = null;
        }

        minigame.gameObject.SetActive(false);
        inMinigame = false;
        _player.SetOnMinigame(false);
    }
}
