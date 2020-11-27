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

    public float maxWaitTime = 37f, emotionIconTime = 4f;
    float _currentWaitTime = 0;

    public bool orderRecieved;

    public DeliverableFood wantedFood;
    public SpriteRenderer speechGlobe, foodIcon, happyIcon, angryIcon;


    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _coll = GetComponent<Collider>();
        _rend = GetComponentInChildren<Renderer>();
        _player = FindObjectOfType<PlayerController>();
        speechGlobe = transform.Find("SpeechGlobe").GetComponent<SpriteRenderer>();

        switch (wantedFood)
        {
            case DeliverableFood.MilanesaDeCarne:
                foodIcon = speechGlobe.transform.Find("Carne").GetComponent<SpriteRenderer>();
                break;
            case DeliverableFood.MilanesaDePollo:
                foodIcon = speechGlobe.transform.Find("Pollo").GetComponent<SpriteRenderer>();
                break;
            case DeliverableFood.MilanesaDeBerenjena:
                foodIcon = speechGlobe.transform.Find("Berenjena").GetComponent<SpriteRenderer>();
                break;
            case DeliverableFood.MilanesaDePescado:
                foodIcon = speechGlobe.transform.Find("Pescado").GetComponent<SpriteRenderer>();
                break;
            default:
                print("Noncontré nada xd");
                break;
        }

        angryIcon = speechGlobe.transform.Find("AngryIcon").GetComponent<SpriteRenderer>();
        happyIcon = speechGlobe.transform.Find("HappyIcon").GetComponent<SpriteRenderer>();
    }

    public void StartOrder()
    {
        StartCoroutine(OrderTimeHandler());
    }

    IEnumerator OrderTimeHandler()
    {
        print("PADRE, QUIERO " + wantedFood);

        speechGlobe.gameObject.SetActive(true);
        foodIcon.gameObject.SetActive(true);

        while (_currentWaitTime <= maxWaitTime)
        {
            if (orderRecieved) yield break;

            yield return new WaitForEndOfFrame();

            _currentWaitTime += Time.deltaTime;
        }

        orderRecieved = true;

        print("BUENO, ESPERÉ " + maxWaitTime + " Y NO ME DIERON LA ORDEN, ME TOMO EL PALO");

        StartCoroutine(EmotionIconTimer(false));
    }

    IEnumerator EmotionIconTimer(bool happy)
    {
        foodIcon.gameObject.SetActive(false);

        if (happy) happyIcon.gameObject.SetActive(true);
        else angryIcon.gameObject.SetActive(true);

        yield return new WaitForSeconds(emotionIconTime);

        speechGlobe.gameObject.SetActive(false);
        happyIcon.gameObject.SetActive(false);
        angryIcon.gameObject.SetActive(false);
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

        StartCoroutine(EmotionIconTimer(correctFood));
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