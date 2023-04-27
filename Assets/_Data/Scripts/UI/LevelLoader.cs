using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float TransitionTime;

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.O))
        //{
        //    this.LoadNextLevel();
        //}
    }

    public IEnumerator SetTriggerFadeOut()
    {
        this.transition.SetTrigger("Start");
        //Debug.Log("Trigger");
        yield return new WaitForSeconds(this.TransitionTime);
    }

    public void LoadNextLevel()
    {
        StartCoroutine(this.LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    private IEnumerator LoadLevel(int levelIndex)
    {
        this.transition.SetTrigger("Start");
        //Debug.Log("Trigger");
        yield return new WaitForSeconds(this.TransitionTime);

        SceneManager.LoadScene(levelIndex);
    }
}
