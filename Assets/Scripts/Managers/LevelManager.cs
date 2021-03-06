using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    public static LevelManager instance;

    public float timeBeforeEndGame = 3f;

    public event Action<UIManager.GameOverReason> onGameOver;

    [SerializeField]
    private Player player;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip alarmSound;

    public float alarmCounter;
    public bool activeAlarm;
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
            Debug.LogError("LevelManager player reference is not present!");

        audioSource.clip = alarmSound;
    }

    private void Update()
    {
        if (activeAlarm)
            alarmCounter += Time.deltaTime;

        if (activeAlarm && alarmCounter >= timeBeforeEndGame) {
            GameOver(UIManager.GameOverReason.Alarm);
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

    public void GameOver(UIManager.GameOverReason reason)
    {
        if (!gameOver) {
            gameOver = true;
            onGameOver?.Invoke(reason);
            Debug.Log("GameOver");
        }
    }
}
