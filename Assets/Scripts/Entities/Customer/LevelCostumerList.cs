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

    byte _CostumerIndex;

    public byte CostumerIndex { get => _CostumerIndex; }

    /// <summary>
    /// Returns a tuple with the Costumer to spawn and a delay for WaitForSeconds
    /// </summary>
    /// <returns></returns>
    public Tuple<CostumerBase, int> GetCostumer()
    {
        if (CostumerIndex >= CostumerData.Length)
        {
            Debug.Log("NO HAY MAS CostumerS");
            return null;
        }

        var costumer = GameObject.Instantiate(CostumerPrefab, GameManager.Instance.CostumerSpawnPoint.position, Quaternion.identity);
        costumer.Initialize(CostumerData[_CostumerIndex].orderType);
        costumer.gameObject.SetActive(false);

        var spawnDelay = CostumerData[_CostumerIndex + 1].spawnDelay - CostumerData[_CostumerIndex].spawnDelay;

        _CostumerIndex++;

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
