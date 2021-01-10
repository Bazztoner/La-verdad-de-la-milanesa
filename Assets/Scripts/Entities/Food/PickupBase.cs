using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PickupBase : MonoBehaviour, IInteractuable
{
    public Transform playerHand;
    protected PlayerController _player;
    protected bool _isPickup;

    protected Renderer _rend;
    protected Rigidbody _rb;
    protected Collider _coll;
    protected Canvas _cnv;
    protected AudioSource _audioSource;

    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _coll = GetComponent<Collider>();
        _rend = GetComponentInChildren<Renderer>();
        _audioSource = GetComponentInChildren<AudioSource>();
        _cnv = GetComponentInChildren<Canvas>(true);
    }

    protected virtual void Start()
    {
        _player = FindObjectOfType<PlayerController>();
        playerHand = _player.hand.transform;

        Physics.IgnoreCollision(_player.GetComponent<Collider>(), _coll);
    }

    protected virtual void Update()
    {
        if (_isPickup)
        {
            transform.position = playerHand.position;
        }

        if (_cnv != null) _cnv.transform.LookAt(_player.cam.transform);
    }

    public virtual void Interact()
    {
        if (!_isPickup)
        {
            _isPickup = true;
            ChangePhysicsState(false);
            transform.parent = playerHand;
            gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }

    public virtual void SendStateToParent()
    {

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

    public virtual void SendFoodStationInfo(FoodStationBase station)
    {

    }

    public void GiveToPlayer()
    {
        _isPickup = true;
        ChangePhysicsState(false);
        transform.parent = playerHand;
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    protected virtual void ChangePhysicsState(bool physicsOn)
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

    public virtual void ActivateHighlight(bool state)
    {
        _rend.material.SetFloat("_Highlighted", state ? 1f : 0f);
    }

}
