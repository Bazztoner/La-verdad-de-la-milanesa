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
    public EmpanateMilanesaMinigame minigame;

    Animator _an;

    public bool inMinigame;

    public int CurrentCharges
    {
        get => _currentCharges;
        private set
        {
            _currentCharges = Mathf.Clamp(value, 0, maxBreadCharges);
            _an.SetInteger("charges", _currentCharges);
        }
    }

    protected override void Start()
    {
        milanesaPosition = transform.Find("MilanesaPosition");
        minigame = GameObject.Find("MainCanvas").GetComponent<Canvas>().GetComponentInChildren<EmpanateMilanesaMinigame>(true);
        _an = GetComponentInChildren<Animator>();
        CurrentCharges = maxBreadCharges;
        base.Start();
    }

    public override void Interact()
    {
        if (currentMilanga == null)
        {
            if (_player.itemPickup is Milanesa)
            {
                var milanga = _player.itemPickup as Milanesa;
                if (milanga.IsEmpanated() || !milanga.IsEnhuevated()) return;

                currentMilanga = milanga;
                milanga.SendFoodStationInfo(this);
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

    public override void FoodGotPulled(PickupBase food)
    {
        //do we consume a charge of pan rallado when pulling the milanga, even if it's not completed?
        //CurrentCharges--;
        var milanesa = food as Milanesa;
        if (milanesa != null) milanesa.PulledFromCooking();
        currentMilanga = null;
    }

    public void FillTrayWithPanRallado()
    {
        CurrentCharges = maxBreadCharges;
    }

    public bool HasFullTray()
    {
        return CurrentCharges >= maxBreadCharges;
    }

    /// <summary>
    /// DO NOT USE YET!!
    /// </summary>
    /// <param name="chargesToAdd"></param>
    public void FillTrayWithPanRallado(int chargesToAdd)
    {
        //TODO make pan rallado give X charges and that be needed to interact like them ilanesa
        Debug.LogError("I TOLD YOU NOT TO USE THIS FUCKING FUNCTION QUE CARAJO TE PASA MAN", this.gameObject);
    }

    public void OnClickMilanesa()
    {
        currentMilanga.OnClickMilanesaForEmpanating();
        if (currentMilanga.IsEmpanated()) minigame.CompleteMinigame();
    }

    public void OnClickTurnOver()
    {
        currentMilanga.TurnMilanesaForEmpanating();
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
