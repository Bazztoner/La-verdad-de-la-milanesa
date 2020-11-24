using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager: MonoBehaviour
{
	static GameManager instance;
	public static GameManager Instance
	{
		get
		{
			if(instance == null)
			{
				instance = FindObjectOfType<GameManager>();
				if(instance == null)
				{
					instance = new GameObject("new GameManager Object").AddComponent<GameManager>().GetComponent<GameManager>();
				}
			}
			return instance;
		}
	}
}
