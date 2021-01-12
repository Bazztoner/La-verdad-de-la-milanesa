using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PickupButtonSpawner : MonoBehaviour, IInteractuable
{
    protected Renderer _rend;
    public Collider spawningBox;
    public PickupBase pickupPrefab;
    public int moneyCost;


    void Awake()
    {
        _rend = GetComponentInChildren<Renderer>();
    }

    public void ActivateHighlight(bool state)
    {
        _rend.material.SetFloat("_Highlighted", state ? 1f : 0f);
    }

    public void Interact()
    {
        if (GameManager.Instance.HasEnoughMoney(moneyCost))
        {
            var randompoint = GetRandomPointOnBoundingBox();

            var instancedPickup = Instantiate(pickupPrefab, randompoint, Quaternion.identity);
            instancedPickup.transform.SetParent(null);
            instancedPickup.transform.eulerAngles = new Vector3(0, 0, 0);

            GameManager.Instance.AddMoneyValue(-moneyCost);
            GameManager.Instance.SpawnMoneyPrompt(transform.position, -moneyCost);
        }
    }

    Vector3 GetRandomPointOnBoundingBox()
    {
        var bounds = spawningBox.bounds;
        float offsetX = Random.Range(-bounds.extents.x, bounds.extents.x);
        float offsetY = Random.Range(-bounds.extents.y, bounds.extents.y);
        float offsetZ = Random.Range(-bounds.extents.z, bounds.extents.z);

        return bounds.center + new Vector3(offsetX, offsetY, offsetZ);
    }
}
