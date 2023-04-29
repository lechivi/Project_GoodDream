using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public WeaponParent WeaponParent { get; set; }
    public int Damage { get; set; }
    public bool IsCritical { get; set; }


    private void Start()
    {
        Physics2D.IgnoreLayerCollision(gameObject.layer, gameObject.layer);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("EnemyWeapon") && !collision.gameObject.CompareTag("PlayerWeapon") &&!collision.gameObject.CompareTag("ColliderWithWall") && !collision.gameObject.CompareTag("Detector") && !collision.gameObject.CompareTag("PlayerCollector"))
        {
            this.HitSomething(collision);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && collision.gameObject.CompareTag("PlayerBattle") && gameObject.CompareTag("EnemyWeapon"))
        {
            if (collision.gameObject.GetComponent<PlayerLife>().Health <= 0) return;
            collision.gameObject.GetComponent<PlayerLife>().TakeDamage(Damage);

            this.HitSomething(collision);
        }

        else if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy") && collision.gameObject.CompareTag("EnemyBattle") && gameObject.CompareTag("PlayerWeapon"))
        {
            EnemyLife enemyLife = collision.gameObject.GetComponent<EnemyLife>();
            if (enemyLife != null)
            {
                if (enemyLife.Health <= 0) return;
                enemyLife.TakeDamage(Damage);

                this.WeaponParent.SpawnDamageText(Damage, collision, enemyLife.EnemyCtrl.NeverFlip.transform, IsCritical);
            }
            else
            {
                this.WeaponParent.SpawnDamageText(Damage, collision, collision.transform, IsCritical);
            }

            this.HitSomething(collision);
        }
    }

    private void HitSomething(Collider2D collision)
    {
        //Debug.Log(collision.gameObject);
        gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        gameObject.GetComponent<Animator>().SetTrigger("Hit");
    }

    public void Finish()
    {
        gameObject.SetActive(false);
    }
}
