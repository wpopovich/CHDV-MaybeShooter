using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image foregroundStaminaBar;
    public Text objectiveText;
    public Animator animator;
    public GameObject gameOverText;
    public GameObject pauseMenu;

    private void Start()
    {
        LevelManager.GetInstance().onGameOver += ShowGameOverMenu;
    }

    private void Update()
    {
        foregroundStaminaBar.fillAmount = Player.currentStamina / 100;

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
        gameOverText.SetActive(true);
        Time.timeScale = 0;
    }

    private void OnDestroy()
    {
        LevelManager.GetInstance().onGameOver -= ShowGameOverMenu;
    }
}
