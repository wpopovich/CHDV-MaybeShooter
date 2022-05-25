using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DetectedMenu : MonoBehaviour
{
    public void Retry()
    {
        Scene scene = SceneManager.GetActiveScene();
        Time.timeScale = 1;
        SceneManager.LoadScene(scene.name);
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
