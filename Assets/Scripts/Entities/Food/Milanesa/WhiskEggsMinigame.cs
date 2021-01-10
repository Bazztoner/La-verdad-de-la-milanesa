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

    AudioSource _audioSource;
    public AudioClip minigameSuccessSound;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = 0;
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
            _audioSource.volume = 1;

            if (MinigameComplete)
            {
                whiskerAn.enabled = false;
                _audioSource.Stop();
                CompleteMinigame();
            }
        }
        else
        {
            whiskerAn.enabled = false;
            _audioSource.volume = 0;
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

        _audioSource.PlayOneShot(minigameSuccessSound);
        progressText.text = "Completo!";

        yield return new WaitForSeconds(t);

        _station.EndEggsMinigame();
        endingSequence = false;
    }
}
