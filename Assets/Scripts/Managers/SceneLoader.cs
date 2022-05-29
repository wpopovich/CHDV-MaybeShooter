using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime;
    public static SceneLoader instance;
    Button playButton;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        SceneManager.sceneLoaded += onSceneLoad;
        
        SetupCrossFade();
    }

    private void onSceneLoad(Scene scene, LoadSceneMode loadMode)
    {
        SetupCrossFade();
    }

    public void SetupCrossFade()
    {
        GameObject crossfade = GameObject.FindWithTag("Crossfade");

        if (crossfade != null)
        {
            transition = crossfade.GetComponent<Animator>();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LoadNextScene();
        }

        if (CameraManager.cinematicHasFinished && activeSceneIsMainMenu())
        {
            SetupPlayButton();
        }
    }

    public void LoadNextScene()
    {
        TriggerSceneLoad(SceneManager.GetActiveScene().buildIndex + 1);
    }

    IEnumerator LoadScene(int levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        if (levelIndex <= SceneManager.sceneCountInBuildSettings -1) {
            SceneManager.LoadScene(levelIndex);
        } else {
            SceneManager.LoadScene(0);
        }

        
    }

    private void TriggerSceneLoad(int levelIndex)
    {
        StartCoroutine(LoadScene(levelIndex));
    }

    public void LoadMainMenu()
    {
        TriggerSceneLoad(0);
    }

    public void RestartScene()
    {
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex));
    }

    public void ExitGame()
    {
        Debug.Log("Quitting!");
        Application.Quit();
    }

    public static SceneLoader GetInstance()
    {
        return instance;
    }

    public void DisableButtonInteraction()
    {
        Button button = playButton.GetComponent<Button>();

        button.interactable = false;
    }

    void SetupPlayButton()
    {
        
        playButton = GameObject.Find("PlayButton").GetComponent<Button>();
        playButton.onClick.AddListener(LoadNextScene);
    }

    bool activeSceneIsMainMenu()
    {
        return SceneManager.GetActiveScene().buildIndex == 0;
    }
}    
