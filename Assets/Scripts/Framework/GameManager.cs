﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class GameManager: MonoBehaviour
{
	static GameManager _instance;
	public static GameManager Instance
	{
		get
		{
			if(_instance == null)
			{
				_instance = FindObjectOfType<GameManager>();
				if(_instance == null)
				{
					_instance = new GameObject("new GameManager Object").AddComponent<GameManager>().GetComponent<GameManager>();
				}
			}
			return _instance;
		}
	}

    public int CurrentMoney { get => _currentMoney; private set => _currentMoney = Mathf.Max(value, 0); }

    public float levelTimer, miseEnPlaceTime;
	float _currentTime;

	public int startingMoney;
    int _currentMoney;

	public TextMeshProUGUI clockText;
	public TextMeshProUGUI hudTimeText;

    void Start()
    {
		CurrentMoney = startingMoney;
		_currentTime = levelTimer;
		StartCoroutine(LevelTimer());
    }

	IEnumerator LevelTimer()
    {
		//Wait for mise on place time to activate customers - we should account for customer travel time

		yield return new WaitForSeconds(miseEnPlaceTime);

		//activate customers
		//activate timer on canvas

        while (true)
        {
			yield return new WaitForEndOfFrame();
			_currentTime -= Time.deltaTime;

            if (_currentTime <= 0)
            {
				_currentTime = 0;

				EndLevel();
				yield break;
			}
		}
		
    }

    public void AddMoneyValue(int money)
    {
		CurrentMoney += money;
    }

    void Update()
    {
		clockText.text = SecondsToTimer(_currentTime);
    }

    public void EndLevel()
    {
		if (CurrentMoney > 0)
		{
			print("WIN");

		}
		else print("LOSE");
    }

	public bool HasEnoughMoney(int moneyAsked)
    {
		return moneyAsked <= CurrentMoney;
    }

	public string SecondsToTimer(float seconds)
    {
		//como soy un mogolico copio y pego:
		/*
		Divide the seconds by 60 to get the total minutes. Then, use the number to the left of the decimal point as the number of minutes.
		Find the remaining seconds by multiplying the even minutes found above by 60. Then, subtract that from the total seconds. This is the remaining seconds.
		Put the even number of minutes and seconds into the form MM:SS.
		*/

		var minutes = (int)Mathf.Floor(seconds / 60);
		var decimalSeconds = Mathf.Abs((minutes * 60) - seconds);
		if (Mathf.Approximately(decimalSeconds, 0)) decimalSeconds = 00;

		var minutesString = "0" + minutes;
		var secondsString = Mathf.Approximately(decimalSeconds, 0) ? "00" : Mathf.RoundToInt(decimalSeconds).ToString();

		return (minutesString + ":" + secondsString);
	}
}
