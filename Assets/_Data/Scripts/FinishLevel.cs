using Newtonsoft.Json.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLevel : MonoBehaviour
{
    [SerializeField] private Animator animatorTransition;
    [SerializeField] private float transitionTime = 1.5f;

    private bool levelComplete;

    private void Update()
    {      
        if (this.levelComplete)
        {
            this.levelComplete = false;
            if (SceneManager.GetActiveScene().buildIndex == 5)
            {
                if (UIManager.HasInstance && AudioManager.HasInstance)
                {
                    UIManager.Instance.ActiveVictoryPanel(true);
                    AudioManager.Instance.PlayBGM(AUDIO.BGM_7_VICTORY);
                }
            }
            else
            {
                if (GameManager.HasInstance)
                {
                    StartCoroutine(GameManager.Instance.LoadChangeScene(SceneManager.GetActiveScene().buildIndex + 1));
                }
            }
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ColliderWithWall") && collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (AudioManager.HasInstance)
            {
                AudioManager.Instance.PlaySFX(AUDIO.SFX_ENTERDOOR);
            }
            collision.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            this.levelComplete = true;
        }
    }

    private IEnumerator Endgame()
    {
        if (UIManager.HasInstance && AudioManager.HasInstance)
        {
            //Time.timeScale = 0f;
            UIManager.Instance.ActiveVictoryPanel(true);
            //AudioManager.Instance.PlaySFX(AUDIO.SE_VICTORY);

        }
        yield return new WaitForSeconds(this.transitionTime);
        Time.timeScale = 0f;
        Debug.Log("Endgame");
    }
}
