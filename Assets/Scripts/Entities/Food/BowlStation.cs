using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BowlStation : FoodStationBase
{
    public bool hasEggs;
    public bool hasVeggies;
    public Milanesa currentMilanga;

    public GameObject normalEggMesh, scrambledEggMesh, veggiesMesh;

    public Transform milangaPos;

    Animator _an;

    public bool scrambledEggs;

    public WhiskEggsMinigame whiskEggsMinigame;
    public EnhuevateMilanesaMinigame enhuevateMinigame;

    public bool inMinigame;



    protected override void Start()
    {
        _an = GetComponentInChildren<Animator>();
        base.Start();
    }

    public override void Interact()
    {
        if (_player.itemPickup is Milanesa)
        {
            if (currentMilanga != null || !scrambledEggs || !hasVeggies) return;

            var milanga = _player.itemPickup as Milanesa;
            if (milanga.IsEnhuevated() || milanga.IsEmpanated() || milanga.IsCooked() || milanga.IsOvercooked()) return;

            currentMilanga = milanga;
            milanga.SendFoodStationInfo(this);
            _player.ForceDepositObject(milangaPos);
        }
        else if (_player.itemPickup is EggFood)
        {
            if (hasEggs) return;
            var buebos = _player.itemPickup as EggFood;

            hasEggs = true;

            _player.ForceDepositObject(milangaPos);

            buebos.gameObject.SetActive(false);

            normalEggMesh.SetActive(true);
        }
        else if (_player.itemPickup is AjoPerejil)
        {
            if (hasVeggies) return;

            var ajoPerejil = _player.itemPickup as AjoPerejil;
            if (!ajoPerejil.isChopped) return;

            hasVeggies = true;

            _player.ForceDepositObject(milangaPos);

            ajoPerejil.gameObject.SetActive(false);

            veggiesMesh.SetActive(true);

        }
        else if(_player.itemPickup == null)
        {
            if (!scrambledEggs && hasEggs)
            {
                StartEggsMinigame();
            }
            else if(currentMilanga != null && hasVeggies)
            {
                StartMilanesaMinigame();
            }

        }
    }

    public void OnScrambleEggs()
    {
        normalEggMesh.SetActive(false);
        scrambledEggMesh.SetActive(true);
        scrambledEggs = true;
    }

    void StartMilanesaMinigame()
    {
        inMinigame = true;
        _player.SetOnMinigame(true);
        enhuevateMinigame.gameObject.SetActive(true);
        enhuevateMinigame.Init(this);
    }

    public void EndMilanesaMinigame()
    {
        if (currentMilanga.IsEnhuevated())
        {
            _player.ForceTakeObject(currentMilanga);

            currentMilanga = null;
        }

        enhuevateMinigame.gameObject.SetActive(false);
        inMinigame = false;
        _player.SetOnMinigame(false);
    }

    void StartEggsMinigame()
    {
        inMinigame = true;
        _player.SetOnMinigame(true);
        whiskEggsMinigame.gameObject.SetActive(true);
        whiskEggsMinigame.Init(this);
    }

    public void EndEggsMinigame()
    {
        if (whiskEggsMinigame.MinigameComplete) OnScrambleEggs();

        whiskEggsMinigame.gameObject.SetActive(false);
        inMinigame = false;
        _player.SetOnMinigame(false);
    }

    public override void FoodGotPulled(PickupBase food)
    {
        var mila = food as Milanesa;
        if (mila == null) return;

        currentMilanga = null;
    }

}
