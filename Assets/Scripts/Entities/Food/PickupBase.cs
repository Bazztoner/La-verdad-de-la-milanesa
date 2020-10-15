using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PickupBase : MonoBehaviour, IInteractuable
{
    public Transform playerHand;
    PlayerController _player;
    bool _isPickup;

    Rigidbody _rb;
    Collider _coll;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _coll = GetComponent<Collider>();

    }

    void Start()
    {
        _player = FindObjectOfType<PlayerController>();
        playerHand = _player.hand.transform;

        Physics.IgnoreCollision(_player.GetComponent<Collider>(), _coll);
    }

    void Update()
    {
        if (_isPickup)
        {
            transform.position = playerHand.position;
        }
    }

    public void Interact()
    {
        if (!_isPickup)
        {
            _isPickup = true;
            ChangePhysicsState(false);
            transform.parent = playerHand;
            gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }

    public void Throw(float force)
    {
        transform.parent = null;
        _isPickup = false;
        ChangePhysicsState(true);
        _rb.AddForce(_player.cam.transform.forward * force, ForceMode.VelocityChange);
    }

    public void Drop()
    {
        transform.parent = null;
        _isPickup = false;
        ChangePhysicsState(true);
        gameObject.layer = LayerMask.NameToLayer("Interactuable");
    }

    public void Deposit(Transform depositPlace)
    {
        transform.parent = depositPlace;
        transform.localPosition = Vector3.zero;
        _isPickup = false;
        gameObject.layer = LayerMask.NameToLayer("Interactuable");
    }

    public void GiveToPlayer()
    {
        _isPickup = true;
        ChangePhysicsState(false);
        transform.parent = playerHand;
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    void ChangePhysicsState(bool physicsOn)
    {
        if (physicsOn)
        {
            _rb.useGravity = true;
            _rb.isKinematic = false;
            _coll.isTrigger = false;
        }
        else
        {
            _rb.useGravity = false;
            _rb.isKinematic = true;
            _coll.isTrigger = true;
        }
    }

}
