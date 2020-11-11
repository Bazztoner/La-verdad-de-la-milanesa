using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Milanesa : FoodBase
{
    public int clicksNeededBySide = 3;

    public float cookingTime;

    public bool sideAEnhuevated = false, sideBEnhuevated = false;

    /// <summary>
    /// No seas tan obvio conque le estás robando al overcooked boludo
    /// </summary>
    public float overcookingTime;

    float _currentCookingTime;

    public SpriteRenderer[] stateIcons;

    enum StateSprites
    {
        CookWarning,
        Overcooked
    }

    /// <summary>
    /// true A, false B
    /// </summary>
    public bool currentEmpanatingSide = true;

    /// <summary>
    /// true A, false B
    /// </summary>
    public bool currentEnhuevatingSide = true;

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
            item.transform.localRotation = _player.cam.transform.rotation;
        }*/
    }

    public void OnTurnMilanesaForEnhuevating()
    {
        currentEnhuevatingSide = !currentEnhuevatingSide;
    }

    /// <summary>
    /// TO DO change function name
    /// </summary>
    public void OnClickMilanesaForEmpanating()
    {
        if (currentEmpanatingSide) SideAClicks++;
        else SideBClicks++;
    }

    public bool IsEnhuevated()
    {
        return sideAEnhuevated && sideBEnhuevated;
    }

    public bool IsEmpanated()
    {
        return SideAClicks >= clicksNeededBySide && SideBClicks >= clicksNeededBySide;
    }

    public override bool IsCooked()
    {
        return _currentCookingTime > cookingTime && !IsOvercooked();
    }

    public override bool IsOvercooked()
    {
        return _currentCookingTime > overcookingTime;
    }

    public int GetCurrentSideEmpanation()
    {
        return currentEmpanatingSide ? SideAClicks : SideBClicks;
    }

    public void TurnMilanesaForEmpanating()
    {
        currentEmpanatingSide = !currentEmpanatingSide;
    }

    public override void PulledFromCooking()
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
