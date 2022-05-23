using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime;

    public void LoadNextScene()
    {
        DontDestroyOnLoad(gameObject);
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadScene(int levelIndex)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }

    public void ExitGame()
    {
        Debug.Log("Quitting!");
        Application.Quit();
    }
}
