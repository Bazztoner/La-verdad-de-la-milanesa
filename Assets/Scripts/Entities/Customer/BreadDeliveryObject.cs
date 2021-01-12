using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BreadDeliveryObject : OrderDelivery
{
    Animator _an;
    Renderer[] _rends;

    protected override void Awake()
    {
        base.Awake();
        _an = GetComponent<Animator>();
        _rends = GetComponentsInChildren<Renderer>();
    }

    public override void Interact()
    {
        if (_player.itemPickup is FoodBase)
        {
            foodToDeliver = _player.itemPickup as FoodBase;
            if (!foodToDeliver.IsCooked() || foodToDeliver.IsOvercooked()) return;

            foodToDeliver.SendOrderDeliveryInfo(this);
            _player.ForceDepositObject(foodPos);
            foodToDeliver.transform.localRotation = Quaternion.Euler(Vector3.zero);

            _an.SetBool("hasFood", true);
        }
        else if (!_isPickup && _player.itemPickup is OrderDelivery) //this fix is so fucking stupid I can't believe my ass/eyes
        {
            _isPickup = true;
            ChangePhysicsState(false);
            transform.parent = playerHand;
            gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }

    public override void ActivateHighlight(bool state)
    {
        foreach (var renderer in _rends)
        {
            renderer.material.SetFloat("_Highlighted", state ? 1f : 0f);
        }
    }

    public override void FoodGotPulled(PickupBase food)
    {
        if (food as FoodBase != null)
        {
            foodToDeliver = null;
            _an.SetBool("hasFood", false);
        }
    }
}
