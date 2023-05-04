using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPanel : MonoBehaviour
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
}
