using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLevel : MonoBehaviour
{
    private bool levelComplete;

    private void Update()
    {
        if (this.levelComplete)
        {
            this.CompleteLevel();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ColliderWithWall") && collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //if (AudioManager.HasInstance)
            //{
            //    AudioManager.Instance.PlaySE(AUDIO.SE_FINISH);
            //}
            this.levelComplete = true;
            collision.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        }
    }

    public void CompleteLevel()
    {
        if (SceneManager.GetActiveScene().name.Equals("SceneLv3"))
        {
            //if (UIManager.HasInstance && AudioManager.HasInstance)
            //{
            //    Time.timeScale = 0f;
            //    UIManager.Instance.ActiveVictoryPanel(true);
            //    AudioManager.Instance.PlaySE(AUDIO.SE_VICTORY);

            //    return;
            //}
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        if (GameManager.HasInstance && UIManager.HasInstance && AudioManager.HasInstance)
        {
            //AudioManager.Instance.PlayBGM(AUDIO.BGM_BGM_04);
        }
    }
}
