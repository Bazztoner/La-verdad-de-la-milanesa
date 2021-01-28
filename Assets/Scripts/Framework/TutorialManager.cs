using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TutorialManager : MonoBehaviour
{
    void Start()
    {
        EventManager.SubscribeToEvent(EventType.CUSTOMER_HAPPY, OnCustomerHappy);
        EventManager.SubscribeToEvent(EventType.CUSTOMER_ANGRY, OnCustomerAngry);
        EventManager.SubscribeToEvent(EventType.CUSTOMER_TIMEOUT, OnCustomerTimeout);
    }

    void OnCustomerHappy(params object[] parameters)
    {
        EventManager.UnsubscribeToEvent(EventType.CUSTOMER_HAPPY, OnCustomerHappy);
        EventManager.UnsubscribeToEvent(EventType.CUSTOMER_ANGRY, OnCustomerAngry);
        EventManager.UnsubscribeToEvent(EventType.CUSTOMER_TIMEOUT, OnCustomerTimeout);
        OnTutorialEnd(true);
    }
    void OnCustomerAngry(params object[] parameters)
    {
        EventManager.UnsubscribeToEvent(EventType.CUSTOMER_HAPPY, OnCustomerHappy);
        EventManager.UnsubscribeToEvent(EventType.CUSTOMER_ANGRY, OnCustomerAngry);
        EventManager.UnsubscribeToEvent(EventType.CUSTOMER_TIMEOUT, OnCustomerTimeout);
        OnTutorialEnd(false);
    }
    void OnCustomerTimeout(params object[] parameters)
    {
        EventManager.UnsubscribeToEvent(EventType.CUSTOMER_HAPPY, OnCustomerHappy);
        EventManager.UnsubscribeToEvent(EventType.CUSTOMER_ANGRY, OnCustomerAngry);
        EventManager.UnsubscribeToEvent(EventType.CUSTOMER_TIMEOUT, OnCustomerTimeout);
        OnTutorialEnd(false);
    }

    public void OnTutorialEnd(bool win)
    {
        GameManager.Instance.EndLevel(win);
    }
}
