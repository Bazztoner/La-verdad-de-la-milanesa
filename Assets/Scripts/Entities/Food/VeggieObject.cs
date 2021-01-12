using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class VeggieObject : MonoBehaviour
{
	public Image baseSprite, choppedSprite, cutHitbox;

    public bool chopped;

	public CutVeggiesMinigame minigameParent;

    void Awake()
    {
        GetOwnData();
        minigameParent = GetComponentInParent<CutVeggiesMinigame>();
    }

    void GetOwnData()
    {
        var allImages = GetComponentsInChildren<Image>();
        foreach (var img in allImages)
        {
            if (img.name == "Picado") choppedSprite = img;
            else if (img.name == "CutHitbox") cutHitbox = img;
            else if (img.transform == this.transform) baseSprite = img;
        }
    }

    public void OnChop()
    {
        chopped = true;
        cutHitbox.gameObject.SetActive(false);

        baseSprite.enabled = false;
        choppedSprite.enabled = true;

        minigameParent.OnChop(this);
    }

    public void ResetVeggie()
    {
        chopped = false;
        cutHitbox.gameObject.SetActive(true);

        baseSprite.enabled = true;
        choppedSprite.enabled = false;
    }
}
