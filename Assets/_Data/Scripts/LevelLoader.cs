using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            this.LoadNextLevel();
        }
    }

    private void LoadNextLevel()
    {
        StartCoroutine(this.LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    private IEnumerator LoadLevel(int levelIndex)
    {
        this.transition.SetTrigger("Start");
        Debug.Log("Trigger");
        yield return new WaitForSeconds(this.transitionTime);

        SceneManager.LoadScene(levelIndex);
    }
}
