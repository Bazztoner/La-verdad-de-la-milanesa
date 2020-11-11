using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class FoodBase : PickupBase
{
	protected FoodStationBase _stationParent;
    protected OrderDelivery _deliveryParent;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public abstract bool IsCooked();

    public abstract bool IsOvercooked();

    public override void SendStateToParent()
    {
        if (_stationParent != null) _stationParent.FoodGotPulled(this);
        if (_deliveryParent != null) _deliveryParent.FoodGotPulled(this);
    }

    public override void SendFoodStationInfo(FoodStationBase station)
    {
        _stationParent = station;
    }

    public virtual void SendOrderDeliveryInfo(OrderDelivery deliverer)
    {
        _deliveryParent = deliverer;
    }


    public abstract void PulledFromCooking();
}
