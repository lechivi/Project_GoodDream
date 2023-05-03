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
            //this.animatorTransition.SetTrigger("Start");
            //this.animatorTransition.Play("Wipe_CircleOut");

            if (SceneManager.GetActiveScene().name.Equals("Scene_Level3_Heaven"))
            {
                this.animatorTransition.Play("Crossfade_Start_Endgame");
                StartCoroutine(this.Endgame());
            }
            else
            {
                this.animatorTransition.Play("Crossfade_Start");
                StartCoroutine(this.CompleteLevel());
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
            this.levelComplete = true;
            collision.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        }
    }

    private IEnumerator CompleteLevel()
    {
        yield return new WaitForSeconds(this.transitionTime);

        if (Input.anyKeyDown)
        { 
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            if (GameManager.HasInstance && UIManager.HasInstance && AudioManager.HasInstance)
            {
                //AudioManager.Instance.PlayBGM(AUDIO.BGM_BGM_04);
            }
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
