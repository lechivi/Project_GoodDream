using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LosePanel : MonoBehaviour
{
    public void OnClickedRestarGame()
    {
        if (GameManager.HasInstance)
        {
            GameManager.Instance.RestarGame();
        }
    }

    public void OnClickedEndGame()
    {
        if (GameManager.HasInstance )
        {
            GameManager.Instance.EndGame();
        }
    }
}
