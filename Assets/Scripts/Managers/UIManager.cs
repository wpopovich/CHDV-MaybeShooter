using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image foregroundStaminaBar;
    public Image backgroundDetectedClock;
    public Image itemImage;
    public GameObject detectedClock;

    public Text objectiveText;
    public GameObject gameOverMenu;
    public GameObject pauseMenu;

    public Inventory playerInventory;
    public InventoryItem item;
    public static UIManager instance;

    private void Start()
    {
        LevelManager.GetInstance().onGameOver += ShowGameOverMenu;
    }
    public static UIManager GetInstance()
    {
        return instance;
    }

    private void Update()
    {
        foregroundStaminaBar.fillAmount = Player.currentStamina / 100;

        if (LevelManager.GetInstance().activeAlarm)
        {
            ShowDetectedClock();
        }
        else
        {
            HideDetectedClock();
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            TogglePauseMenu();
        }
    }

    public void TogglePauseMenu()
    {
        bool currentToggle = pauseMenu.activeSelf;
        Debug.Log(currentToggle);
        pauseMenu.SetActive(!currentToggle);
        if (!currentToggle) {
            Time.timeScale = 0;
        } else {
            Time.timeScale = 1;
        }
    }

    public void ShowGameOverMenu() {
        Debug.Log("ShowGameOverMenu");
        gameOverMenu.SetActive(true);
        Time.timeScale = 0;
    }

    private void OnDestroy()
    {
        LevelManager.GetInstance().onGameOver -= ShowGameOverMenu;
    }

    void ShowDetectedClock()
    {
        detectedClock.SetActive(true);
        backgroundDetectedClock.fillAmount -= Time.deltaTime / LevelManager.GetInstance().timeBeforeEndGame;
    }

    void HideDetectedClock()
    {
        backgroundDetectedClock.fillAmount = 1;
        detectedClock.SetActive(false);
    }

    //public void ShowItemIcon(Sprite icon)
    //{
    //    itemImage.sprite = icon;
    //}
}
