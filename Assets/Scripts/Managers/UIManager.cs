using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image foregroundStaminaBar;
    public Text objectiveText;
    public Animator animator;

    private void Update()
    {
        foregroundStaminaBar.fillAmount = Player.currentStamina / 100;
    }
}
