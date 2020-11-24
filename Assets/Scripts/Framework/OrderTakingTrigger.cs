using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class OrderTakingTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Customer"))
        {
            var customer = other.GetComponent<CustomerBase>();
            customer.StartOrder();
        }   
    }
}
