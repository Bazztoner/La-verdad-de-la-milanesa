﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Milanesa : PickupBase
{
    public int clicksNeededBySide = 3;

    ///REMEMBER TO FUCKING MAKE A FOODBASE ITEM TO ADD THE COSO OF COCINADO
    public float cookingTime;

    /// <summary>
    /// No seas tan obvio conque le estás robando al overcooked boludo
    /// </summary>
    public float overcookingTime;

    float _currentCookingTime;

    public SpriteRenderer[] stateIcons;

    FoodStationBase _stationParent;

    enum StateSprites
    {
        CookWarning,
        Overcooked
    }

    /// <summary>
    /// true A, false B
    /// </summary>
    public bool currentSide = true;

    int _sideAClicks, _sideBClicks;

    public int SideAClicks
    {
        get => _sideAClicks;
        private set => _sideAClicks = Mathf.Clamp(value, 0, clicksNeededBySide);
    }

    public int SideBClicks
    {
        get => _sideBClicks;
        private set
        {
            _sideBClicks = Mathf.Clamp(value, 0, clicksNeededBySide);
        }
    }

    protected override void Start()
    {
        base.Start();

        stateIcons = new SpriteRenderer[2];

        stateIcons[(int)StateSprites.CookWarning] = transform.Find("ExclamationMark").GetComponent<SpriteRenderer>();
        stateIcons[(int)StateSprites.Overcooked] = transform.Find("FireIcon").GetComponent<SpriteRenderer>();

    }

    protected override void Update()
    {
        base.Update();

        //I CAN'T MAKE a fucking billboard for fuck's sake
        /*foreach (var item in stateIcons)
        {
            item.transform.rotation = _player.cam.transform.rotation;
        }*/
    }

    public void OnClickMilanesa()
    {
        if (currentSide) SideAClicks++;
        else SideBClicks++;
    }

    public bool IsEmpanated()
    {
        return SideAClicks >= clicksNeededBySide && SideBClicks >= clicksNeededBySide;
    }

    public bool IsCooked()
    {
        return _currentCookingTime > cookingTime && !IsOvercooked();
    }

    public bool IsOvercooked()
    {
        return _currentCookingTime >= overcookingTime;
    }

    public int GetCurrentSideClicks()
    {
        return currentSide ? SideAClicks : SideBClicks;
    }

    public void TurnMilanesa()
    {
        currentSide = !currentSide;
    }

    public override void SendStateToParent()
    {
        if (_stationParent != null) _stationParent.FoodGotPulled(this);
    }

    public override void SendFoodStationInfo(FoodStationBase station)
    {
        _stationParent = station;
    }

    public void PulledFromCooking()
    {
        stateIcons[(int)StateSprites.CookWarning].gameObject.SetActive(false);
    }

    /// <summary>
    /// Add a Time.deltaTime or something like that please
    /// </summary>
    /// <param name="t"></param>
    public void AddCookingTime(float t)
    {
        _currentCookingTime += t;
        ManageCookingTime();
    }

    void ManageCookingTime()
    {
        if (IsCooked())
        {
            stateIcons[(int)StateSprites.CookWarning].gameObject.SetActive(true);
            stateIcons[(int)StateSprites.Overcooked].gameObject.SetActive(false);
        }
        else if (IsOvercooked())
        {
            stateIcons[(int)StateSprites.CookWarning].gameObject.SetActive(false);
            stateIcons[(int)StateSprites.Overcooked].gameObject.SetActive(true);
        }
    }
}
