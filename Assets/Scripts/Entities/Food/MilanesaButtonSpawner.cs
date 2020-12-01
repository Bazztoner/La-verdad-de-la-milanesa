using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MilanesaButtonSpawner : MonoBehaviour, IInteractuable
{
    protected Renderer _rend;
    public Collider spawningBox;
    public Milanesa milangaPrefab;
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

            var instancedMilanga = Instantiate(milangaPrefab, randompoint, Quaternion.identity);
            instancedMilanga.transform.SetParent(null);
            instancedMilanga.transform.eulerAngles = new Vector3(0, 0, 90);

            GameManager.Instance.AddMoneyValue(-moneyCost);
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
