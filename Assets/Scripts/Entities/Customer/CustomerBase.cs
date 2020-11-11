using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class CustomerBase : MonoBehaviour
{
	Camera _cam;
    float _camAngle;


    void Awake()
    {
        _cam = Camera.main;
    }

    void Update()
    {
        
    }
}