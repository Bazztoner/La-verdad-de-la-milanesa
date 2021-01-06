using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelCostumerList : MonoBehaviour
{
    [SerializeField]
    public CostumerSpawnInfo[] CostumerData;

    public CostumerBase CostumerPrefab;

    public WaypointChain[] spawnPoints;
    public Waypoint patrolOutStart;

    byte _costumerIndex;

    public byte CostumerIndex { get => _costumerIndex; }

    /// <summary>
    /// Returns a tuple with the Costumer to spawn and a delay for WaitForSeconds
    /// </summary>
    /// <returns></returns>
    public Tuple<CostumerBase, int> GetCostumer()
    {
        if (CostumerIndex >= CostumerData.Length - 1)
        {
            Debug.Log("NO HAY MAS CostumerS");
            return null;
        }

        var costumer = GameObject.Instantiate(CostumerPrefab, GameManager.Instance.CostumerSpawnPoint.position, Quaternion.identity);
        costumer.Initialize(CostumerData[_costumerIndex].orderType);
        costumer.gameObject.SetActive(false);

        var spawnDelay = CostumerData[_costumerIndex + 1].spawnDelay - CostumerData[_costumerIndex].spawnDelay;

        _costumerIndex++;

        return Tuple.Create(costumer, spawnDelay);
    }
}

[System.Serializable]
public struct CostumerSpawnInfo
{
    /// <summary>
    /// In seconds
    /// </summary>
    public int spawnDelay;

    public DeliverableFood orderType;

    /// <summary>
    /// In seconds
    /// </summary>
    //public int orderTime;


}
