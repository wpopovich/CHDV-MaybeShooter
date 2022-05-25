using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DetectedMenu : MonoBehaviour
{
    public GameObject failedText;
    public void Retry()
    {
        Time.timeScale = 1;
        SceneLoader.GetInstance().RestartScene();
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneLoader.GetInstance().LoadMainMenu();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ShowFailedText()
    {
        failedText.SetActive(true);
    }
}
