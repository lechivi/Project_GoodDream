using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScarecrowScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerWeapon"))
        {
            if (AudioManager.HasInstance)
            {
                AudioManager.Instance.PlaySFX(AUDIO.SFX_ENEMYHIT);
            }

            transform.GetComponent<Animator>().SetTrigger("Hit");
        }
    }
}
