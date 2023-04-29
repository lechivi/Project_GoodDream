using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCharacterStatue : MonoBehaviour
{
    private bool isEnter;

    private void Update()
    {
        if (this.isEnter && Input.GetKeyDown(KeyCode.E))
        {
            if (UIManager.HasInstance)
            {
                UIManager.Instance.ActiveTutorialScenePanel(true);
            }
        }
    }

    private void  OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && collision.CompareTag("ColliderWithWall"))
        {
            this.isEnter = true;
        }
    }    
    
    private void  OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && collision.CompareTag("ColliderWithWall"))
        {
            this.isEnter = false;
        }
    }
}
