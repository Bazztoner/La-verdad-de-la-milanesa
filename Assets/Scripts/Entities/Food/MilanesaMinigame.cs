using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class MilanesaMinigame : MonoBehaviour
{
    MilanesaStation _station;
    public TextMeshProUGUI sideText, progressText;

    public bool endingSequence;

    void Awake()
    {
        sideText = transform.Find("SideText").GetComponent<TextMeshProUGUI>();
        progressText = transform.Find("ProgressText").GetComponent<TextMeshProUGUI>();
    }

    public void Init(MilanesaStation station)
    {
        endingSequence = false;
        _station = station;
        sideText.text = "Lado " + (_station.currentMilanga.currentSide ? "A" : "B");
        progressText.text = "Progreso " + _station.currentMilanga.GetCurrentSideClicks() + "/" + _station.currentMilanga.clicksNeededBySide;
    }

    public void OnClickMilanesa()
    {
        if (endingSequence) return;

        _station.OnClickMilanesa();
        if (_station.inMinigame)
        {
            sideText.text = "Lado " + (_station.currentMilanga.currentSide ? "A" : "B");
            progressText.text = "Progreso " + _station.currentMilanga.GetCurrentSideClicks() + "/" + _station.currentMilanga.clicksNeededBySide;
        }
    }

    public void OnClickTurnOver()
    {
        if (endingSequence) return;

        _station.OnClickTurnOver();
        if (_station.inMinigame)
        {
            sideText.text = "Lado " + (_station.currentMilanga.currentSide ? "A" : "B");
            progressText.text = "Progreso " + _station.currentMilanga.GetCurrentSideClicks() + "/" + _station.currentMilanga.clicksNeededBySide;
        }
    }

    public void EndMinigame()
    {
        StartCoroutine(EndMinigameDelay(2f));
    }

    IEnumerator EndMinigameDelay(float t)
    {
        endingSequence = true;

        yield return new WaitForEndOfFrame();

        sideText.text = " ";
        progressText.text = "Complete!";

        yield return new WaitForSeconds(t);

        _station.EndMinigame();
        endingSequence = false;
    }
}
