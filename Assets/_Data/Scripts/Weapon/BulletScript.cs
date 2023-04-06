using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    //[SerializeField] private LayerMask hitable;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            gameObject.GetComponent<Animator>().SetTrigger("Hit");
        }
        //else
        //{
        //    gameObject.GetComponent<CircleCollider2D>().isTrigger = true;
        //}
    }

    public void Finish()
    {
        gameObject.SetActive(false);
    }
}
