using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    public static LevelManager instance;

    public float timeBeforeEndGame;
    public event Action onGameOver;

    [SerializeField]
    private Player player;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip alarmSound;

    [SerializeField]
    private GameObject pauseMenu;

    private float alarmCounter;
    private bool activeAlarm;
    public bool gameOver = false;
    public static LevelManager GetInstance()
    {
        return instance;
    }
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null) {
            instance = this;
        } else
            Destroy(this);

        if (player == null)
            Debug.LogError("GameManager player reference is not present!");

        audioSource.clip = alarmSound;
    }

    private void Update()
    {
        if (activeAlarm)
            alarmCounter += Time.deltaTime;

        if (activeAlarm && alarmCounter >= timeBeforeEndGame) {
            GameOver();
        }
    }

    public Player Player()
    {
        return player;
    }

    public void PlayAlarm()
    {
        Debug.Log("Alarm!");
        audioSource.mute = false;
    }

    public void StopAlarm()
    {
        Debug.Log("Stop Alarm");
        audioSource.mute = true;
        activeAlarm = false;
        alarmCounter = 0;
    }

    public void Detected()
    {
        activeAlarm = true;
        PlayAlarm();
    }

    void GameOver()
    {
        if (!gameOver) {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            gameOver = true;
            onGameOver?.Invoke();
            Debug.Log("GameOver");
        }
    }

}
