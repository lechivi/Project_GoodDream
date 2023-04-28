using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideScript : MonoBehaviour
{
    private void OnEnable()
    {
        //GameObject.Find("Player").GetComponent<PlayerMovement>()
        Time.timeScale = 0;
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) 
        {
            gameObject.SetActive(false);
        }
    }
}
