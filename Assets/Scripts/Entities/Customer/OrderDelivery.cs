﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class OrderDelivery : PickupBase
{
    public FoodBase foodToDeliver;

    public Transform foodPos;

    public override void Interact()
    {
        if (_player.itemPickup is FoodBase)
        {
            foodToDeliver = _player.itemPickup as FoodBase;
            if (!foodToDeliver.IsCooked() || foodToDeliver.IsOvercooked()) return;

            foodToDeliver.SendOrderDeliveryInfo(this);
            _player.ForceDepositObject(foodPos);
        }
        else if (!_isPickup && _player.itemPickup is OrderDelivery) //this fix is so fucking stupid I can't believe my ass/eyes
        {
            _isPickup = true;
            ChangePhysicsState(false);
            transform.parent = playerHand;
            gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }

    public virtual void FoodGotPulled(PickupBase food)
    {
        if (food as FoodBase != null) foodToDeliver = null;
    }

    public void GetOrder(FoodBase food)
    {
        if (food.GetType() == foodToDeliver.GetType())
        {

        }
    }

    protected override void ChangePhysicsState(bool physicsOn)
    {
        base.ChangePhysicsState(physicsOn);

        //HARDCODEADO HORRIBLE JEJE
        _coll.enabled = physicsOn;
        foodToDeliver.GetComponent<Collider>().enabled = physicsOn;
    }
}
