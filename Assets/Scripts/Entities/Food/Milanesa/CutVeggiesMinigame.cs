using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class CutVeggiesMinigame : MonoBehaviour
{
    ChoppingTableStation _station;

    VeggieObject[] _veggies;

    public float velocityToSlash = 5f;

    public Image slasherPointer;
    public TextMeshProUGUI progressText;

    public bool endingSequence;

    #region fuckery for canvas
    GraphicRaycaster _raycaster;
    PointerEventData _pointerData;
    EventSystem _eventSystem;
    #endregion

    AudioSource _audioSource;
    public AudioClip minigameSuccessSound, slashSound;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _veggies = GetComponentsInChildren<VeggieObject>();

        //AK ROBANDO!!
        //Fetch the Raycaster from the GameObject (the Canvas)
        var canvas = FindObjectsOfType<Canvas>().Where(x => x.name == "MainCanvas").First();
        _raycaster = canvas.GetComponent<GraphicRaycaster>();
        //Fetch the Event System from the Scene
        _eventSystem = FindObjectOfType<EventSystem>();
    }

    public void Init(ChoppingTableStation station)
    {
        progressText.text = " ";
        endingSequence = false;
        _station = station;
        slasherPointer.enabled = false;
    }

    void Update()
    {
        slasherPointer.transform.position = Vector3.Lerp(slasherPointer.transform.position, Mouse.current.position.ReadValue(), Time.time);
        
        if (Mouse.current.leftButton.IsPressed() && Mathf.Abs(Mouse.current.delta.ReadValue().y) >= velocityToSlash)
        {
            slasherPointer.enabled = true;
            
            //Set up the new Pointer Event
            _pointerData = new PointerEventData(_eventSystem);
            //Set the Pointer Event Position to that of the mouse position
            _pointerData.position = Mouse.current.position.ReadValue();

            //Create a list of Raycast Results
            List<RaycastResult> results = new List<RaycastResult>();

            //Raycast using the Graphics Raycaster and mouse click position
            _raycaster.Raycast(_pointerData, results);

            //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
            foreach (RaycastResult result in results)
            {
                //Debug.Log(result.gameObject.name, result.gameObject);

                if (result.gameObject.name == "CutHitbox")
                {
                    result.gameObject.GetComponentInParent<VeggieObject>().OnChop();
                    return;
                }
            }
        }
        else
        {
            slasherPointer.enabled = false;
        }
    }

    public void OnChop(VeggieObject sender)
    {
        if (endingSequence) return;

        _audioSource.PlayOneShot(slashSound);
        _station.OnChop();
    }

    public void CancelMinigame()
    {
        if (!endingSequence) _station.EndMinigame();
    }
    public bool EverythingChopped()
    {
        return _veggies.All(x => x.chopped);
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

        _station.EndMinigame();
        endingSequence = false;
    }

}
