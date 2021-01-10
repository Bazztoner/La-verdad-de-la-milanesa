using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.UI;

public class EmpanateMilanesaMinigame : MonoBehaviour
{
    MilanesaStation _station;
    public TextMeshProUGUI sideText, progressText;

    public Image milangaImage;

    public bool endingSequence;

    AudioSource _audioSource;
    public AudioClip minigameSuccessSound, empanateSound, turnMilangaSound;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        sideText = transform.Find("SideText").GetComponent<TextMeshProUGUI>();
        progressText = transform.Find("ProgressText").GetComponent<TextMeshProUGUI>();
        milangaImage = transform.Find("Milanesa").GetComponent<Image>();
    }

    public void Init(MilanesaStation station)
    {
        endingSequence = false;
        _station = station;
        sideText.text = "Lado " + (_station.currentMilanga.currentEmpanatingSide ? "A" : "B");
        progressText.text = "Progreso " + _station.currentMilanga.GetCurrentSideEmpanation() + "/" + _station.currentMilanga.clicksNeededBySide;
        milangaImage.fillAmount = (float)_station.currentMilanga.GetCurrentSideEmpanation() / (float)_station.currentMilanga.clicksNeededBySide;
    }

    public void OnClickMilanesa()
    {
        if (endingSequence) return;

        _station.OnClickMilanesa();
        if (_station.inMinigame)
        {
            sideText.text = "Lado " + (_station.currentMilanga.currentEmpanatingSide ? "A" : "B");
            progressText.text = "Progreso " + _station.currentMilanga.GetCurrentSideEmpanation() + "/" + _station.currentMilanga.clicksNeededBySide;
            milangaImage.fillAmount = (float)_station.currentMilanga.GetCurrentSideEmpanation() / (float)_station.currentMilanga.clicksNeededBySide;
            _audioSource.PlayOneShot(empanateSound);
        }
    }

    public void OnClickTurnOver()
    {
        if (endingSequence) return;

        _station.OnClickTurnOver();
        if (_station.inMinigame)
        {
            sideText.text = "Lado " + (_station.currentMilanga.currentEmpanatingSide ? "A" : "B");
            progressText.text = "Progreso " + _station.currentMilanga.GetCurrentSideEmpanation() + "/" + _station.currentMilanga.clicksNeededBySide;
            milangaImage.fillAmount = (float)_station.currentMilanga.GetCurrentSideEmpanation() / (float)_station.currentMilanga.clicksNeededBySide;
            _audioSource.PlayOneShot(turnMilangaSound);
        }
    }

    public void CancelMinigame()
    {
       if(!endingSequence) _station.EndMinigame();
    }

    public void CompleteMinigame()
    {
        StartCoroutine(EndMinigameDelay(1f));
    }

    IEnumerator EndMinigameDelay(float t)
    {
        endingSequence = true;

        yield return new WaitForEndOfFrame();

        _audioSource.PlayOneShot(minigameSuccessSound);
        sideText.text = " ";
        progressText.text = "Completo!";

        yield return new WaitForSeconds(t);

        _station.EndMinigame();
        endingSequence = false;
    }
}
