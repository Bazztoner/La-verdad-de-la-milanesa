using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Variables
    public float movementSpeed;
    public float runMultiplier;

    public float cameraRotationClamp = 30f;
    public float headSensitivity;
    public float headRotationSpeed;

    bool _lockedByGame;
    bool _hasItem;

    PickupBase _itemPickup;
    public PlayerHand hand;

    Keyboard _keyboard;
    Gamepad _gamepad;
    Mouse _mouse;
    public Camera cam;

    Tuple<float, float> _directionVectors = new Tuple<float, float>(0, 0);

    Rigidbody _rb;
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
    }

    void Update()
    {
        if (_lockedByGame) return;

        MovementInput();
        MouseLook();
        CheckInteract();
        CheckMouseInput();
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
                _itemPickup.Throw(8f);
                _hasItem = false;
                _itemPickup = null;
            }
            else if(_mouse.rightButton.wasPressedThisFrame)
            {
                _itemPickup.Drop();
                _hasItem = false;
                _itemPickup = null;
            }
        }
    }

    void CheckInteract()
    {
        if (_keyboard.fKey.wasPressedThisFrame)
        {
            if (_hasItem)
            {
                _itemPickup.Drop();
                _hasItem = false;
                _itemPickup = null;
            }
            else
            {
                RaycastHit rch;
                var mask = LayerMask.GetMask("Interactuable");
                var hits = Physics.Raycast(cam.transform.position, cam.transform.forward, out rch, 4, mask);
                if (hits)
                {
                    print(rch.transform.name);

                    if (rch.collider.GetComponent(typeof(IInteractuable)) is IInteractuable interact) interact.Interact();

                    var pickup = rch.collider.GetComponent<PickupBase>();
                    if (pickup)
                    {
                        _hasItem = true;
                        _itemPickup = pickup;
                    }
                }
            }
        }
    }
}
