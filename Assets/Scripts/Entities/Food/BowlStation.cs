using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BowlStation : FoodStationBase
{
    public bool hasEggs;
    public bool hasVeggies;
    public Milanesa currentMilanga;

    public GameObject eggMesh, veggiesMesh;

    public Transform milangaPos;

    Animator _an;

    public bool scrambledEggs;

    protected override void Start()
    {
        _an = GetComponentInChildren<Animator>();
        base.Start();
    }

    void Update()
    {

    }

    public override void Interact()
    {
        if (_player.itemPickup is Milanesa)
        {
            if (currentMilanga != null || !scrambledEggs) return;

            var milanga = _player.itemPickup as Milanesa;
            if (milanga.IsEnhuevated() || milanga.IsEmpanated() || milanga.IsCooked() || milanga.IsOvercooked()) return;

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
            if (!scrambledEggs)
            {
                //scramble eggs minigame
            }
            else if(currentMilanga != null)
            {
                //enhuevate milaness minigame
            }

        }
    }

    public override void FoodGotPulled(PickupBase food)
    {
        var mila = food as Milanesa;
        if (mila == null) return;

        currentMilanga = null;
    }

}
