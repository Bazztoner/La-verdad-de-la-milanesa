using System.Collections;
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
            foodToDeliver.SendOrderDeliveryInfo(this);
            _player.ForceDepositObject(foodPos);
        }
        else if (!_isPickup && _player.itemPickup == null)
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
}
