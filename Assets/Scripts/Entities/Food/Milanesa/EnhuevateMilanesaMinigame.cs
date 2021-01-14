using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.UI;

public class EnhuevateMilanesaMinigame : MonoBehaviour
{
	BowlStation _station;
	public TextMeshProUGUI progressText;

	public RectTransform buttonPositionsContainer;

	RectTransform[] _buttonPositions;
	public Button turnButton;

	public Image normalMeatImage;
	public Image enhuevatedImage;

	public bool endingSequence;

	public float buttonTeleportTime;
	float _currentTeleportTime;

	AudioSource _audioSource;
	public AudioClip minigameSuccessSound, enhuevateSound, turnMilangaSound;

	void Awake()
	{
		_audioSource = GetComponent<AudioSource>();

		progressText = transform.Find("ProgressText").GetComponent<TextMeshProUGUI>();
		_buttonPositions = buttonPositionsContainer.GetComponentsInChildren<RectTransform>().Where(x => x != buttonPositionsContainer).ToArray();
		turnButton.transform.position = _buttonPositions[Random.Range(0, _buttonPositions.Length)].position;
		_currentTeleportTime = 0f;

		var allImg = GetComponentsInChildren<Image>(true);
		normalMeatImage = allImg.First(x => x.name == "Meat");
		enhuevatedImage = allImg.First(x => x.name == "EnhuevatedMeat");
	}

	public void Init(BowlStation station)
	{
		endingSequence = false;
		_station = station;
		progressText.text = "";
		normalMeatImage.sprite = station.currentMilanga.meatImage;
		enhuevatedImage.sprite = station.currentMilanga.enhuevatedImage;

		if (_station.currentMilanga.currentEnhuevatingSide) enhuevatedImage.enabled = _station.currentMilanga.sideAEnhuevated;
		else enhuevatedImage.enabled = _station.currentMilanga.sideBEnhuevated;
	}

    void Update()
    {
		if (endingSequence) return;

		_currentTeleportTime += Time.deltaTime;

		if (_currentTeleportTime >= buttonTeleportTime)
        {
			turnButton.transform.position = _buttonPositions[Random.Range(0, _buttonPositions.Length)].position;
			_currentTeleportTime = 0f;
        }
    }

	public void OnClickMilanesa()
    {
		if (endingSequence) return;

		if (_station.currentMilanga.currentEnhuevatingSide) _station.currentMilanga.sideAEnhuevated = true;
		else _station.currentMilanga.sideBEnhuevated = true;

		enhuevatedImage.enabled = true;

		_station.currentMilanga.IsEnhuevated();

		_audioSource.PlayOneShot(enhuevateSound);

        if (_station.currentMilanga.IsEnhuevated())
        {
			CompleteMinigame();
        }
	}

	public void OnTurnMilanesa()
    {
		if (endingSequence) return;

		_station.currentMilanga.currentEnhuevatingSide = !_station.currentMilanga.currentEnhuevatingSide;

		_audioSource.PlayOneShot(turnMilangaSound);

		if (_station.currentMilanga.currentEnhuevatingSide) enhuevatedImage.enabled = _station.currentMilanga.sideAEnhuevated;
		else enhuevatedImage.enabled = _station.currentMilanga.sideBEnhuevated;
	}

    public void CancelMinigame()
	{
		if (!endingSequence) _station.EndMilanesaMinigame();
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

		_station.EndMilanesaMinigame();
		endingSequence = false;
	}
}
