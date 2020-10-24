using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ChoppingTableStation : FoodStationBase
{
    public Transform choppingPosition;
    public AjoPerejil currentVeggies;
    public CutVeggiesMinigame minigame;

    Animator _an;

    public bool inMinigame;

    protected override void Start()
    {
        choppingPosition = transform.Find("FoodPos");
        minigame = FindObjectOfType<Canvas>().GetComponentInChildren<CutVeggiesMinigame>(true);
        _an = GetComponentInChildren<Animator>();
        base.Start();
    }

    public override void Interact()
    {
        if (currentVeggies == null)
        {
            if (_player.itemPickup is AjoPerejil)
            {
                var ajoPerejil = _player.itemPickup as AjoPerejil;

                currentVeggies = ajoPerejil;
                ajoPerejil.SendFoodStationInfo(this);
                _player.ForceDepositObject(choppingPosition);
            }
        }
        else
        {
            StartMinigame();
        }
    }

    public override void FoodGotPulled(PickupBase food)
    {
        //var ajoPerejil = food as AjoPerejil;
        currentVeggies = null;
    }

    public void OnChop()
    {
        if (minigame.EverythingChopped())
        {
            minigame.CompleteMinigame();
        }
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
        if (minigame.EverythingChopped())
        {
            Destroy(currentVeggies);
        }
        else
        {
            _player.ForceTakeObject(currentVeggies);
        }

        currentVeggies = null;
        minigame.gameObject.SetActive(false);
        inMinigame = false;
        _player.SetOnMinigame(false);
    }
}
