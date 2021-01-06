using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class CustomerBase : MonoBehaviour, IInteractuable
{
    protected PlayerController _player;
    protected Renderer _rend;
    protected Rigidbody _rb;
    protected Collider _coll;

    public float maxWaitTime = 37f, emotionIconTime = 1.5f;
    float _currentWaitTime = 0;

    public bool orderRecieved;

    public DeliverableFood wantedFood;
    public SpriteRenderer speechGlobe, foodIcon, happyIcon, angryIcon;
    public Image orderTimeFeedback;

    SimpleWaypointWalker _walker;
    public WaypointChain currentChain;


    void Awake()
    {
        /*var allFoods = new DeliverableFood[4] { DeliverableFood.MilanesaDeBerenjena, DeliverableFood.MilanesaDeCarne, DeliverableFood.MilanesaDePescado, DeliverableFood.MilanesaDePollo };

        wantedFood = allFoods[Random.Range(0, allFoods.Length)];*/

        _rb = GetComponent<Rigidbody>();
        _coll = GetComponent<Collider>();
        _rend = GetComponentInChildren<Renderer>();
        _player = FindObjectOfType<PlayerController>();
        speechGlobe = transform.Find("SpeechGlobe").GetComponent<SpriteRenderer>();
        _walker = GetComponent<SimpleWaypointWalker>();
        _walker.current = null;

        /*switch (wantedFood)
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
        }*/

        angryIcon = speechGlobe.transform.Find("AngryIcon").GetComponent<SpriteRenderer>();
        happyIcon = speechGlobe.transform.Find("HappyIcon").GetComponent<SpriteRenderer>();

        orderTimeFeedback = GetComponentsInChildren<Image>(true).Where(x => x.name == "RedFill").First();
    }

    public void Initialize(DeliverableFood orderType)
    {
        wantedFood = orderType;

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
    }

    public void Initialize(DeliverableFood orderType, float orderWaitTime)
    {
        Initialize(orderType);
        maxWaitTime = orderWaitTime;
    }

    public void SetWaypoint(WaypointChain chain)
    {
        currentChain = chain;
        _walker.current = currentChain.startWaypoint;
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
            orderTimeFeedback.fillAmount = _currentWaitTime / maxWaitTime;
        }

        orderRecieved = true;

        print("BUENO, ESPERÉ " + maxWaitTime + " Y NO ME DIERON LA ORDEN, ME TOMO EL PALO");

        currentChain.OnFinishedCustomer();
        currentChain = null;

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

        _walker.current = GameManager.Instance.customerSpawner.patrolOutStart;
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

        currentChain.OnFinishedCustomer();
        currentChain = null;

        //HARDCODEA3
        if (correctFood)
        {
            GameManager.Instance.AddMoneyValue(100);
            GameManager.Instance.SpawnMoneyPrompt(this.transform.position, 100);
        }

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
    MilanesaDePescado
}