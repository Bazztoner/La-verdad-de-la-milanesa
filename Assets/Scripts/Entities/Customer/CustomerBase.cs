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

    public DeliverableFood wantedFood;


    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _coll = GetComponent<Collider>();
        _rend = GetComponentInChildren<Renderer>();
        _player = FindObjectOfType<PlayerController>();
    }

    void Update()
    {

    }

    public void GetDelivery(OrderDelivery delivery)
    {
        var correctFood = FoodIWant(delivery.foodToDeliver);

        _player.ForceDepositObject(transform);

        delivery.gameObject.SetActive(false);

        var elreturn = correctFood ? "GRACIAS LOCO" : "SOS PELOTUDO TARADO DE MIERDA?";

        print(elreturn);
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