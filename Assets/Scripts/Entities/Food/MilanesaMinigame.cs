using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class MilanesaMinigame : MonoBehaviour
{
    MilanesaStation _station;
    public TextMeshProUGUI sideText, progressText;

    void Awake()
    {
        sideText = transform.Find("SideText").GetComponent<TextMeshProUGUI>();
        progressText = transform.Find("ProgressText").GetComponent<TextMeshProUGUI>();
    }

    public void Init(MilanesaStation station)
    {
        _station = station;
        sideText.text = "Lado " + (_station.currentMilanga.currentSide ? "A" : "B");
        progressText.text = "Progreso " + _station.currentMilanga.GetCurrentSideClicks() + "/" + _station.currentMilanga.clicksNeededBySide;
    }

    public void OnClickMilanesa()
    {
        _station.OnClickMilanesa();
        if (_station.inMinigame)
        {
            sideText.text = "Lado " + (_station.currentMilanga.currentSide ? "A" : "B");
            progressText.text = "Progreso " + _station.currentMilanga.GetCurrentSideClicks() + "/" + _station.currentMilanga.clicksNeededBySide;
        }
    }

    public void OnClickTurnOver()
    {
        _station.OnClickTurnOver();
        if (_station.inMinigame)
        {
            sideText.text = "Lado " + (_station.currentMilanga.currentSide ? "A" : "B");
            progressText.text = "Progreso " + _station.currentMilanga.GetCurrentSideClicks() + "/" + _station.currentMilanga.clicksNeededBySide;
        }
    }

    public void EndMinigame()
    {
        _station.EndMinigame();
    }
}
