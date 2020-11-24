using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class CustomerBase : MonoBehaviour, IInteractuable
{
    protected PlayerController _player;
    protected Renderer _rend;
    protected Rigidbody _rb;
    protected Collider _coll;

    public float maxWaitTime = 37f;
    float _currentWaitTime = 0;

    public bool orderRecieved;

    public DeliverableFood wantedFood;


    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _coll = GetComponent<Collider>();
        _rend = GetComponentInChildren<Renderer>();
        _player = FindObjectOfType<PlayerController>();
    }

    public void StartOrder()
    {
        StartCoroutine(OrderTimeHandler());
    }

    IEnumerator OrderTimeHandler()
    {
        print("PADRE, QUIERO " + wantedFood);

        while (_currentWaitTime <= maxWaitTime)
        {
            if (orderRecieved) yield break;

            yield return new WaitForEndOfFrame();

            _currentWaitTime += Time.deltaTime;
        }

        orderRecieved = true;

        print("BUENO, ESPERÉ " + maxWaitTime + " Y NO ME DIERON LA ORDEN, ME TOMO EL PALO");
    }

    public void GetDelivery(OrderDelivery delivery)
    {
        if (delivery.foodToDeliver == null || orderRecieved) return;

        var correctFood = FoodIWant(delivery.foodToDeliver);

        _player.ForceDepositObject(transform);

        delivery.gameObject.SetActive(false);

        var elreturn = correctFood ? "GRACIAS LOCO!!!" : "NO, YO NO QUERIA ESTO!!";

        print(elreturn);
        orderRecieved = true;
    }

    bool FoodIWant(FoodBase food)
    {
        if (food == null) return false;

        if (wantedFood == DeliverableFood.MilanesaDeCarne)
        {
            var castedFood = food as Milanesa;
            return castedFood.typeOfMilanga == Milanesa.MilanesaType.Carne;
        }
        else if (wantedFood == DeliverableFood.MilanesaDePollo)
        {
            var castedFood = food as Milanesa;
            return castedFood.typeOfMilanga == Milanesa.MilanesaType.Pollo;
        }
        else if (wantedFood == DeliverableFood.MilanesaDePescado)
        {
            var castedFood = food as Milanesa;
            return castedFood.typeOfMilanga == Milanesa.MilanesaType.Pescado;
        }
        else if (wantedFood == DeliverableFood.MilanesaDeBerenjena)
        {
            var castedFood = food as Milanesa;
            return castedFood.typeOfMilanga == Milanesa.MilanesaType.Berenjena;
        }
        else if (wantedFood == DeliverableFood.PapasFritas)
        {
            return false;
        }
        else return false;
    }

    public void Interact()
    {
        if (_player.itemPickup is OrderDelivery)
        {
            GetDelivery(_player.itemPickup as OrderDelivery);
        }
    }

    public void ActivateHighlight(bool state)
    {
        _rend.material.SetFloat("_Highlighted", state ? 1f : 0f);
    }
}

public enum DeliverableFood
{
    MilanesaDeCarne,
    MilanesaDePollo,
    MilanesaDeBerenjena,
    MilanesaDePescado,
    PapasFritas
}