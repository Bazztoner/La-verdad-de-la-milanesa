using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelCustomerList : MonoBehaviour
{
    [SerializeField]
    public CustomerSpawnInfo[] customerData;

    public CustomerBase customerPrefab;

    public WaypointChain[] spawnPoints;
    public Waypoint patrolOutStart;

    byte _customerIndex;

    public byte CustomerIndex { get => _customerIndex; }

    /// <summary>
    /// Returns a tuple with the Customer to spawn and a delay for WaitForSeconds
    /// </summary>
    /// <returns></returns>
    public Tuple<CustomerBase, int> GetCustomer()
    {
        if (CustomerIndex >= customerData.Length - 1)
        {
            Debug.Log("NO HAY MAS PEDIDOS");
            return null;
        }

        var customer = GameObject.Instantiate(customerPrefab, GameManager.Instance.customerSpawnPoint.position, Quaternion.identity);
        customer.Initialize(customerData[_customerIndex].orderType);
        customer.gameObject.SetActive(false);

        var spawnDelay = customerData[_customerIndex + 1].spawnDelay - customerData[_customerIndex].spawnDelay;

        _customerIndex++;

        return Tuple.Create(customer, spawnDelay);
    }
}

[System.Serializable]
public struct CustomerSpawnInfo
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
