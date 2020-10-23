using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;

public class FreidoraStation : FoodStationBase
{
    public int maxMilangasCooking = 3;

    /// <summary>
    /// Item 1: Transform
    /// Item 2: Milanesa
    /// </summary>
    public Tuple<Transform, Milanesa>[] milanesas;

    Animator _an;

    public int CurrentMilangas()
    {
        return milanesas.Where(x => x.Item2 != null).Count();
    }

    protected override void Start()
    {
        milanesas = new Tuple<Transform, Milanesa>[maxMilangasCooking];
        
        var milanesaPositions = GetComponentsInChildren<Transform>().Where(x => x.name == "MilanesaPosition").ToArray();
        for (int i = 0; i < milanesas.Length; i++)
        {
            milanesas[i] = new Tuple<Transform, Milanesa>(milanesaPositions[i], null);
        }

        _an = GetComponentInChildren<Animator>();
        base.Start();
    }

    void Update()
    {
        foreach (var tuple in milanesas)
        {
            if (tuple.Item2 != null) tuple.Item2.AddCookingTime(Time.deltaTime);
        }

        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            foreach (var item in milanesas)
            {
                if (item.Item2 != null) Debug.Log("TENGO " + item.Item2, item.Item2.gameObject);
            }
        }
    }

    Transform AddMilanga(Milanesa milangaToAdd)
    {
        for (int i = 0; i < milanesas.Length; i++)
        {
            if (milanesas[i].Item2 == null)
            {
                var pos = milanesas[i].Item1;
                milanesas[i] = new Tuple<Transform, Milanesa>(pos, milangaToAdd);
                return pos;
            }
        }
        return null;
    }

    public override void Interact()
    {
        if (CurrentMilangas() < maxMilangasCooking)
        {
            if (_player.itemPickup is Milanesa)
            {
                var milanga = _player.itemPickup as Milanesa;
                if (!milanga.IsEmpanated() || milanga.IsCooked() || milanga.IsOvercooked()) return;

                milanga.SendFoodStationInfo(this);
                var milangaPos = AddMilanga(milanga);

                _player.ForceDepositObject(milangaPos);
            }
        }
    }

    public override void FoodGotPulled(PickupBase food)
    {
        var mila = food as Milanesa;
        if (mila == null) return;

        mila.PulledFromCooking();

        for (int i = 0; i < milanesas.Length; i++)
        {
            if (milanesas[i].Item2 == mila)
            {
                var pos = milanesas[i].Item1;
                milanesas[i] = new Tuple<Transform, Milanesa>(pos, null);
            }
        }
    }


}
