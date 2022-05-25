using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image foregroundStaminaBar;
    public Text objectiveText;
    public Animator animator;
    public GameObject pauseMenu;
    public GameObject failureCanvas;

    private void Update()
    {
        foregroundStaminaBar.fillAmount = Player.currentStamina / 100;

        if (Input.GetKeyDown(KeyCode.Escape)) {
            TogglePauseMenu();
        }
    }

    private void TogglePauseMenu()
    {
        bool currentToggle = pauseMenu.activeSelf;
        pauseMenu.SetActive(!currentToggle);
        if (!currentToggle) {
            Time.timeScale = 0;
        } else {
            Time.timeScale = 1;
        }
    }
}
