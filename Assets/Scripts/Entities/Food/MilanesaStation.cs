using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MilanesaStation : MonoBehaviour, IInteractuable
{
    public int maxBreadCharges = 3;
    int _currentCharges;

    public Transform milanesaPosition;
    public Milanesa currentMilanga;
    public MilanesaMinigame minigame;
    PlayerController _player;

    public int CurrentCharges
    {
        get => _currentCharges;
        private set => _currentCharges = Mathf.Clamp(value, 0, maxBreadCharges);
    }

    void Start()
    {
        milanesaPosition = transform.Find("MilanesaPosition");
        minigame = FindObjectOfType<MilanesaMinigame>();
        CurrentCharges = maxBreadCharges;
        _player = FindObjectOfType<PlayerController>();
    }

    public void Interact()
    {
        if (currentMilanga == null)
        {
            if (_player.itemPickup is Milanesa)
            {
                _player.ForceDepositObject(milanesaPosition);
            }
        }
        else
        {
            //start minigame if pan rallado > 0
            if (CurrentCharges > 0)
            {
                StartMinigame();
            }

        }
    }

    public void OnClickMilanesa()
    {
        currentMilanga.OnClickMilanesa();
        if (currentMilanga.IsCooked()) EndMinigame();
    }

    public void OnClickTurnOver()
    {
        currentMilanga.TurnMilanesa();
    }

    void StartMinigame()
    {
        _player.SetOnMinigame(true);
        minigame.gameObject.SetActive(true);
    }

    public void EndMinigame()
    {
        if (currentMilanga.IsCooked())
        {
            /*give milanga to chaboncito*/
            CurrentCharges--;
            currentMilanga = null;
        }

        _player.SetOnMinigame(false);
        minigame.gameObject.SetActive(false);
    }
}
