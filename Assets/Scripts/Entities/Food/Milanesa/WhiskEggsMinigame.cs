using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using System.Runtime.InteropServices.WindowsRuntime;

public class WhiskEggsMinigame : MonoBehaviour
{
    public float whiskTime = 5f;
    float _currentWhiskingTime;

    public float mouseSpeedForWhisking = 5f;

    BowlStation _station;
    public TextMeshProUGUI progressText;

    public Animator bowlAn, whiskerAn;

    public bool endingSequence;

    public bool MinigameComplete{ get { return _currentWhiskingTime >= whiskTime; } }

    void Awake()
    {
        progressText = transform.Find("ProgressText").GetComponent<TextMeshProUGUI>();
        whiskerAn.enabled = false;
    }

    public void Init(BowlStation station)
    {
        endingSequence = false;
        _station = station;

        var intTime = Mathf.Lerp(3, 0, _currentWhiskingTime / whiskTime);
        bowlAn.Play(Mathf.RoundToInt(intTime).ToString());
    }


    void Update()
    {
        if (endingSequence) return;

        if (Mouse.current.leftButton.IsPressed() && Mathf.Abs(Mouse.current.delta.ReadValue().x) >= mouseSpeedForWhisking)
        {
            whiskerAn.enabled = true;

            _currentWhiskingTime += Time.deltaTime;

            var intTime = Mathf.Lerp(3, 0, _currentWhiskingTime / whiskTime);
            bowlAn.Play(Mathf.RoundToInt(intTime).ToString());

            if (MinigameComplete)
            {
                whiskerAn.enabled = false;
                CompleteMinigame();
            }
        }
        else
        {
            whiskerAn.enabled = false;
        }
    }

    public void CancelMinigame()
    {
        if (!endingSequence) _station.EndEggsMinigame();
    }

    public void CompleteMinigame()
    {
        StartCoroutine(EndMinigameDelay(1f));
    }

    IEnumerator EndMinigameDelay(float t)
    {
        endingSequence = true;

        yield return new WaitForEndOfFrame();

        progressText.text = "Completo!";

        yield return new WaitForSeconds(t);

        _station.EndEggsMinigame();
        endingSequence = false;
    }
}
