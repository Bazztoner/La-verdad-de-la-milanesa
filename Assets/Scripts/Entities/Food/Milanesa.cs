using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Milanesa : PickupBase
{
    public int clicksNeededBySide = 3;

    /// <summary>
    /// true A, false B
    /// </summary>
    public bool currentSide = true;

    int _sideAClicks, _sideBClicks;

    public int SideAClicks
    {
        get => _sideAClicks;
        private set =>  _sideAClicks = Mathf.Clamp(value, 0, clicksNeededBySide);
    }

    public int SideBClicks
    {
        get => _sideBClicks;
        private set
        {
            _sideBClicks = Mathf.Clamp(value, 0, clicksNeededBySide);
        }
    }

    public void OnClickMilanesa()
    {
        if (currentSide) SideAClicks++;
        else SideBClicks++;
    }

    public bool IsCooked()
    {
        return SideAClicks >= clicksNeededBySide && SideBClicks >= clicksNeededBySide;
    }

    public int GetCurrentSideClicks()
    {
        return currentSide ? SideAClicks : SideBClicks;
    }

    public void TurnMilanesa()
    {
        currentSide = !currentSide;
    }


}
