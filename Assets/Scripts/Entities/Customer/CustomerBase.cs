using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class CustomerBase : MonoBehaviour
{
	Camera _cam;
    float _camAngle;
    SpriteRenderer[] _spriteDirs;

    enum SpriteDirections { Front, Back, Left, Right };


    void Awake()
    {
        _cam = Camera.main;
        _spriteDirs = new SpriteRenderer[4];
        _spriteDirs[(int)SpriteDirections.Front] = transform.Find("Front").GetComponent<SpriteRenderer>();
        _spriteDirs[(int)SpriteDirections.Back] = transform.Find("Back").GetComponent<SpriteRenderer>();
        _spriteDirs[(int)SpriteDirections.Left] = transform.Find("Left").GetComponent<SpriteRenderer>();
        _spriteDirs[(int)SpriteDirections.Right] = transform.Find("Right").GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        //SpriteHandler();
    }

    void SpriteHandler()
    {
        var camPos = new Vector2(_cam.transform.forward.x, _cam.transform.forward.z);
        var myPos = new Vector2(transform.forward.x, transform.forward.z);
        _camAngle = Vector2.Angle(camPos, myPos);
        Vector3 cross = Vector3.Cross(camPos, myPos);

        if (cross.z > 0) _camAngle = (360 - _camAngle);

        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            print(_camAngle);
        }

        if (_camAngle <= 45 && _camAngle >= 315)
        {
            HandleSpriteDirs((int)SpriteDirections.Back);
        }
        else if(_camAngle >= 45 && _camAngle <= 135)
        {
            HandleSpriteDirs((int)SpriteDirections.Left);
        }
        else if (_camAngle >= 135 && _camAngle <= 225)
        {
            HandleSpriteDirs((int)SpriteDirections.Right);
        }
        else
        {
            HandleSpriteDirs((int)SpriteDirections.Front);
        }

    }

    void HandleSpriteDirs(int dir)
    {
        for (int i = 0; i < _spriteDirs.Length; i++)
        {
            _spriteDirs[i].enabled = i == dir;
        }
    }

}
