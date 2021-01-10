using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

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

    public Image[] stateIcons;
    public Animator progressBar;
    Image _progressBar;

    [Header("Get from resources")]
    public Sprite meatImage, enhuevatedImage;

    public enum MilanesaType { Carne, Pollo, Berenjena, Pescado }

    public MilanesaType typeOfMilanga;

    public AudioClip tickingSound, burntSound;


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

        if (_progressBar == null) progressBar = _cnv.GetComponentsInChildren<Animator>(true).First(x => x.name == "ProgressBar");

        _audioSource.Stop();

        _progressBar = progressBar.GetComponentsInChildren<Image>(true).First(x => x.name == "Filler");
        _progressBar.fillAmount = 0f;
        progressBar.gameObject.SetActive(false);

        stateIcons = new Image[2];

        var icons = GetComponentsInChildren<Image>(true);

        stateIcons[(int)StateSprites.CookWarning] = icons.First(x => x.name == "ExclamationMark");
        stateIcons[(int)StateSprites.Overcooked] = icons.First(x => x.name == "FireIcon");

    }

    protected override void Update()
    {
        base.Update();
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
        progressBar.SetBool("cooking", false);
        _audioSource.Stop();
    }

    /// <summary>
    /// Add a Time.deltaTime or something like that please
    /// </summary>
    /// <param name="t"></param>
    public void AddCookingTime(float t)
    {
        _currentCookingTime += t;
        _progressBar.fillAmount = _currentCookingTime / cookingTime;
        ManageCookingTime();
    }

    public void StartCooking()
    {
        progressBar.gameObject.SetActive(true);
        progressBar.SetBool("cooking", true);
    }

    void ManageCookingTime()
    {
        if (IsCooked())
        {
            stateIcons[(int)StateSprites.CookWarning].gameObject.SetActive(true);
            stateIcons[(int)StateSprites.Overcooked].gameObject.SetActive(false);
            progressBar.SetBool("cooking", false);

            _audioSource.loop = true;
            _audioSource.clip = tickingSound;
            _audioSource.Play();
        }
        else if (IsOvercooked())
        {
            stateIcons[(int)StateSprites.CookWarning].gameObject.SetActive(false);
            stateIcons[(int)StateSprites.Overcooked].gameObject.SetActive(true);
            progressBar.gameObject.SetActive(false);

            _audioSource.Stop();
            _audioSource.PlayOneShot(burntSound);
        }
    }
}
