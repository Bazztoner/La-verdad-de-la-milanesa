using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEditor;

public class PlayerController : MonoBehaviour
{
    #region Variables
    public float movementSpeed;
    public float runMultiplier;

    public float cameraRotationClamp = 30f;
    public float headSensitivity;
    public float headRotationSpeed;

    [Header("Raycast lenght")]
    public float interactuableRange = 6f;

    bool _lockedByGame;
    bool _hasItem;

    public PickupBase itemPickup;
    public PlayerHand hand;

    IInteractuable _pointedInteractuable;

    Keyboard _keyboard;
    Gamepad _gamepad;
    Mouse _mouse;
    public Camera cam;

    Tuple<float, float> _directionVectors = new Tuple<float, float>(0, 0);

    Rigidbody _rb;
    RaycastHit _rch;
    #endregion

    void Awake()
    {
        _keyboard = Keyboard.current;
        _gamepad = Gamepad.current;
        _mouse = Mouse.current;
        cam = GetComponentInChildren<Camera>();
        _rb = GetComponent<Rigidbody>();
        hand = GetComponentInChildren<PlayerHand>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (EditorApplication.isPlaying) headSensitivity *= 3;
    }

    void Update()
    {
        if (_lockedByGame) return;

        MovementInput();
        MouseLook();
        ScanForInteractuables();
        CheckInteract();
        CheckMouseInput();

        if (_keyboard.qKey.wasPressedThisFrame) print(_pointedInteractuable);
    }

    void FixedUpdate()
    {
        if (_lockedByGame) return;

        CheckMovement();
    }


    void MovementInput()
    {
        var xVkt = _keyboard.dKey.isPressed ? 1 : _keyboard.aKey.isPressed ? -1 : 0;
        var zVkt = _keyboard.wKey.isPressed ? 1 : _keyboard.sKey.isPressed ? -1 : 0;

        _directionVectors = new Tuple<float, float>(xVkt, zVkt);
    }

    void CheckMovement()
    {
        var dir = transform.forward * _directionVectors.Item2 + transform.right * _directionVectors.Item1;
        var multiplier = _keyboard.leftAltKey.isPressed || _keyboard.leftShiftKey.isPressed ? runMultiplier : 1;
        var movVector = _rb.position + dir.normalized * Time.fixedDeltaTime * (movementSpeed * multiplier);
        _rb.MovePosition(movVector);
    }

    void MouseLook()
    {
        var mouseDelta = _mouse.delta.ReadValue() * Time.deltaTime * headSensitivity;

        Vector3 headRot = cam.transform.localEulerAngles + new Vector3(-mouseDelta.y, 0, 0f);
        headRot.x = ClampAngle(headRot.x, -cameraRotationClamp, cameraRotationClamp);
        cam.transform.localRotation = Quaternion.Euler(headRot);

        Vector3 playerRot = transform.eulerAngles + new Vector3(0, mouseDelta.x, 0f);
        transform.rotation = Quaternion.Euler(playerRot);
    }

    float ClampAngle(float angle, float from, float to)
    {
        // accepts e.g. -80, 80
        if (angle < 0f) angle = 360 + angle;
        if (angle > 180f) return Mathf.Max(angle, 360 + from);
        return Mathf.Min(angle, to);
    }

    void CheckMouseInput()
    {
        if (_hasItem)
        {
            if (_mouse.leftButton.wasPressedThisFrame)
            {
                itemPickup.Throw(8f);
                _hasItem = false;
                itemPickup = null;
            }
            else if (_mouse.rightButton.wasPressedThisFrame)
            {
                itemPickup.Drop();
                _hasItem = false;
                itemPickup = null;
            }
        }
    }

    void ScanForInteractuables()
    {
        var mask = LayerMask.GetMask("Interactuable", "Customer");
        var hits = Physics.Raycast(cam.transform.position, cam.transform.forward, out _rch, interactuableRange, mask);
        if (hits)
        {
            if (_rch.collider.GetComponent(typeof(IInteractuable)) is IInteractuable interact)
            {
                if (_pointedInteractuable != interact)
                {
                    if (_pointedInteractuable != null) _pointedInteractuable.ActivateHighlight(false);

                    if (!_hasItem)
                    {
                        _pointedInteractuable = interact;
                        _pointedInteractuable.ActivateHighlight(true);
                    }
                    else if (interact is FoodStationBase || interact is OrderDelivery || interact is CustomerBase)
                    {
                        var customer = interact as CustomerBase;
                        if (customer != null && customer.orderRecieved) return;
                        _pointedInteractuable = interact;
                        _pointedInteractuable.ActivateHighlight(true);
                    }
                    else
                    {
                        _pointedInteractuable = null;
                    }
                }
            }
            else
            {
                if (_pointedInteractuable != null) _pointedInteractuable.ActivateHighlight(false);
                _pointedInteractuable = null;
            }
        }
        else
        {
            if (_pointedInteractuable != null) _pointedInteractuable.ActivateHighlight(false);
            _pointedInteractuable = null;
        }
    }

    void CheckInteract()
    {
        if (_keyboard.fKey.wasPressedThisFrame)
        {
            if (_hasItem)
            {
                if (_pointedInteractuable != null)
                {
                    _pointedInteractuable.Interact();
                }
                else
                {
                    itemPickup.Drop();
                    _hasItem = false;
                    itemPickup = null;
                }
            }
            else if (_pointedInteractuable != null)
            {
                var pickup = _pointedInteractuable as PickupBase;
                if (pickup)
                {
                    _hasItem = true;
                    itemPickup = pickup;
                    pickup.SendStateToParent();
                }
                _pointedInteractuable.Interact();
            }
        }
    }

    public void SetOnMinigame(bool minigame)
    {
        Cursor.lockState = minigame ? CursorLockMode.Confined : CursorLockMode.Locked;
        Cursor.visible = minigame;
        _lockedByGame = minigame;
    }

    public void ForceTakeObject(PickupBase pickup)
    {
        _hasItem = true;
        itemPickup = pickup;
        itemPickup.Interact();
    }

    public void ForceDepositObject(Transform giveTo)
    {
        itemPickup.Deposit(giveTo);
        _hasItem = false;
        itemPickup = null;
    }

}
