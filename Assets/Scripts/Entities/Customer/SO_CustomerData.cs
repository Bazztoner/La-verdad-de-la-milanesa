using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "CustomerData", menuName = "Scriptable Objects/LevelData/CustomerData")]
public class SO_CustomerData : ScriptableObject
{
    [SerializeField]
    public CustomerSpawnInfo[] customerData;
}
