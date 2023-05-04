using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryPanel : MonoBehaviour
{
    public void OnClickedMainMenuButton()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySFX(AUDIO.SFX_BUTTON);
        }

        if (GameManager.HasInstance)
        {
            GameManager.Instance.BackToMainMenu();
        }
    }

    public void OnClickedQuitButton()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySFX(AUDIO.SFX_BUTTON);
        }

        if (GameManager.HasInstance)
        {
            GameManager.Instance.QuitGame();
        }
    }
}
