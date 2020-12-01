using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    void Start()
    {
		CurrentMoney = startingMoney;
		_currentTime = levelTimer;
		StartCoroutine(LevelTimer());
    }

	IEnumerator LevelTimer()
    {
		//Wait for mise on place time to activate customers

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

}
