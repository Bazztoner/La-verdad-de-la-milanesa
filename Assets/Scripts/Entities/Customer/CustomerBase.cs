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
    public Image speechGlobe, foodIcon, happyIcon, angryIcon;
    public Image orderTimeFeedback;

    Canvas _cnv;
    SimpleWaypointWalker _walker;
    public WaypointChain currentChain;

    AudioSource _audioSource;

    public AudioClip orderSound, happySound, angrySound;

    public int moneyWhenHappy = 100, moneyWhenAngry = -50;

    bool _ordering = false;

    void Awake()
    {
        _ordering = false;
        _rb = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
        _coll = GetComponent<Collider>();
        _rend = GetComponentInChildren<Renderer>();
        _player = FindObjectOfType<PlayerController>();
        _cnv = GetComponentInChildren<Canvas>(true);
        _cnv.gameObject.SetActive(false);

        var allImg = _cnv.GetComponentsInChildren<Image>(true);

        speechGlobe = allImg.First(x => x.name == "SpeechGlobe").GetComponent<Image>();
        _walker = GetComponent<SimpleWaypointWalker>();
        _walker.current = null;

        angryIcon = allImg.First(x => x.name == "AngryIcon").GetComponent<Image>();
        happyIcon = allImg.First(x => x.name == "HappyIcon").GetComponent<Image>();

        orderTimeFeedback = GetComponentsInChildren<Image>(true).Where(x => x.name == "RedFill").First();
    }

    public void Initialize(DeliverableFood orderType)
    {
        _ordering = false;
        wantedFood = orderType;

        var allImg = speechGlobe.GetComponentsInChildren<Image>(true);

        switch (wantedFood)
        {
            case DeliverableFood.MilanesaDeCarne:
                foodIcon = allImg.First(x => x.name == "Carne").GetComponent<Image>();
                break;
            case DeliverableFood.MilanesaDePollo:
                foodIcon = allImg.First(x => x.name == "Pollo").GetComponent<Image>();
                break;
            case DeliverableFood.MilanesaDeBerenjena:
                foodIcon = allImg.First(x => x.name == "Berenjena").GetComponent<Image>();
                break;
            case DeliverableFood.MilanesaDePescado:
                foodIcon = allImg.First(x => x.name == "Pescado").GetComponent<Image>();
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

    void Update()
    {
        _cnv.transform.LookAt(_player.cam.transform);
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
        _ordering = true;

        print("PADRE, QUIERO " + wantedFood);

        _cnv.gameObject.SetActive(true);
        speechGlobe.gameObject.SetActive(true);
        foodIcon.gameObject.SetActive(true);
        _audioSource.PlayOneShot(orderSound);

        while (_currentWaitTime <= maxWaitTime)
        {
            if (orderRecieved) yield break;

            yield return new WaitForEndOfFrame();

            _currentWaitTime += Time.deltaTime;
            orderTimeFeedback.fillAmount = _currentWaitTime / maxWaitTime;
        }

        orderRecieved = true;

        print("BUENO, ESPERÉ " + maxWaitTime + " Y NO ME DIERON LA ORDEN, ME TOMO EL PALO");

        GameManager.Instance.AddMoneyValue(moneyWhenAngry);
        GameManager.Instance.SpawnMoneyPrompt(this.transform.position, moneyWhenAngry);
        GameManager.Instance.OnCustomerTimeOut();

        currentChain.OnFinishedCustomer();
        currentChain = null;

        StartCoroutine(EmotionIconTimer(false));
    }

    IEnumerator EmotionIconTimer(bool happy)
    {
        _ordering = false;

        foodIcon.gameObject.SetActive(false);
        _cnv.transform.Find("ProgressBar").gameObject.SetActive(false);

        if (happy)
        {
            happyIcon.gameObject.SetActive(true);
            _audioSource.PlayOneShot(happySound);
        }
        else
        {
            angryIcon.gameObject.SetActive(true);
            _audioSource.PlayOneShot(angrySound);
        }

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

        GameManager.Instance.AddMoneyValue(correctFood ? moneyWhenHappy : moneyWhenAngry);
        GameManager.Instance.SpawnMoneyPrompt(this.transform.position, correctFood ? moneyWhenHappy : moneyWhenAngry);
        GameManager.Instance.OnCustomerOrderFulfilled(correctFood);

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

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent(typeof(IInteractuable)) is IInteractuable pickup)
        {
            if (pickup != null)
            {
                CheckHitByPickup(other.gameObject.GetComponentInChildren<Rigidbody>());
            }
        }
    }

    void CheckHitByPickup(Rigidbody obj)
    {
        if (!_ordering) return;

        var forceVkt = Mathf.Abs(obj.velocity.x + obj.velocity.y + obj.velocity.z);

        print(obj.name + " " + forceVkt);
        if (forceVkt >= 1f)
        {
            orderRecieved = true;

            print("QUE ME TIRÁS CUBILLA");

            GameManager.Instance.AddMoneyValue(moneyWhenAngry);
            GameManager.Instance.SpawnMoneyPrompt(this.transform.position, moneyWhenAngry);
            GameManager.Instance.OnCustomerTimeOut();

            currentChain.OnFinishedCustomer();
            currentChain = null;

            StartCoroutine(EmotionIconTimer(false));
        }
    }
}

public enum DeliverableFood
{
    MilanesaDeCarne,
    MilanesaDePollo,
    MilanesaDeBerenjena,
    MilanesaDePescado
}