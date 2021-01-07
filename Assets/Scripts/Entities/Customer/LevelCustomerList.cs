using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelCustomerList : MonoBehaviour
{
    [SerializeField]
    public SO_CustomerData customers;
    public string levelPrefix;

    public CustomerBase customerPrefab;

    public WaypointChain[] spawnPoints;
    public Waypoint patrolOutStart;

    byte _customerIndex;

    void Awake()
    {
        if (customers == null) customers = Resources.Load<SO_CustomerData>("ScriptableObjects/LevelData/" + levelPrefix + "_CustomerData");
    }

    public byte CustomerIndex { get => _customerIndex; }

    /// <summary>
    /// Returns a tuple with the Customer to spawn and a delay for WaitForSeconds
    /// </summary>
    /// <returns></returns>
    public Tuple<CustomerBase, int> GetCustomer()
    {
        if (CustomerIndex >= customers.customerData.Length - 1)
        {
            Debug.Log("NO HAY MAS PEDIDOS");
            return null;
        }

        var customer = GameObject.Instantiate(customerPrefab, GameManager.Instance.customerSpawnPoint.position, Quaternion.identity);
        customer.Initialize(customers.customerData[_customerIndex].orderType);
        customer.gameObject.SetActive(false);

        var spawnDelay = customers.customerData[_customerIndex + 1].spawnDelay - customers.customerData[_customerIndex].spawnDelay;

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
