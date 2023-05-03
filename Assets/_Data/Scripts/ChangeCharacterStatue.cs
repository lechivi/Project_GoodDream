using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCharacterStatue : MonoBehaviour
{
    private bool isEnter;

    private void Update()
    {
        //if (UIManager.HasInstance)
        //{
        //    if (!UIManager.Instance.GuideCtrl.GuideShow)
        //    {
               
        //    }
        //}
        
        if (GameManager.HasInstance && UIManager.HasInstance)
        {
            if (GameManager.Instance.IsPlaying && this.isEnter && Input.GetKeyDown(KeyCode.E))
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
