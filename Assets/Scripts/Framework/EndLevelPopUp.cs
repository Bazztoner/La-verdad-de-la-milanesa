using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;

public class EndLevelPopUp : MonoBehaviour
{
    public bool changingScene = false;
    public GameObject loadingContainer;

    public void ResetGame()
    {
        if (!changingScene)
        {
            changingScene = true;

            loadingContainer.SetActive(true);
            Invoke("StartLoading", 1);
        }
    }

    void StartLoading()
    {
        SceneManager.LoadScene("Main");
    }
}
