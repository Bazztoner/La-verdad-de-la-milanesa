using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;

public class WinLevelPopUp : EndLevelPopUp
{
    public TextMeshProUGUI customersDelivery, customersHappy, customersRemaining, moneyText;

    public void Init(int delivered, int happy, int remaining, int money)
    {
        customersDelivery.text += " " + delivered.ToString();
        customersHappy.text += " " + happy.ToString();
        customersRemaining.text += " " + remaining.ToString();
        moneyText.text += " " + money.ToString();
        loadingContainer.SetActive(false);
    }

}
