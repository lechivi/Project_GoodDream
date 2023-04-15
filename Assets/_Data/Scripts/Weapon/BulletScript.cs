using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class BulletScript : MonoBehaviour
{
    public WeaponParent WeaponParent { get; set; }
    public int Damage { get; set; }
    public bool IsCritical { get; set; }

    private Collider2D[] collidersWithTag;

    private void Start()
    {
        //this.collidersWithTag = GameObject.FindGameObjectsWithTag("ColliderWithWall").Select(go => go.GetComponent<Collider2D>()).ToArray();
        //foreach (Collider2D collider in collidersWithTag)
        //{
        //    Physics2D.IgnoreCollision(collider, gameObject.GetComponent<Collider2D>());
        //}
        Physics2D.IgnoreLayerCollision(gameObject.layer, this.WeaponParent.gameObject.layer);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag(this.tag) && !collision.gameObject.CompareTag("ColliderWithWall"))
        {
            this.HitSomething();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && collision.gameObject.CompareTag("PlayerBattle") && gameObject.CompareTag("EnemyWeapon"))
        {
            if (collision.gameObject.GetComponent<PlayerLife>().Health <= 0) return;
            collision.gameObject.GetComponent<PlayerLife>().TakeDamage(Damage);

            this.HitSomething();
        }

        else if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy") && collision.gameObject.CompareTag("EnemyBattle") && gameObject.CompareTag("PlayerWeapon"))
        {
            EnemyLife enemyLife = collision.gameObject.GetComponent<EnemyLife>();
            if (enemyLife.Health <= 0) return;
            enemyLife.TakeDamage(Damage);

            this.WeaponParent.SpawnDamageText(Damage, collision, enemyLife.EnemyCtrl.NeverFlip.transform, IsCritical);
            this.HitSomething();
        }
    }

    private void HitSomething()
    {
        gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        gameObject.GetComponent<Animator>().SetTrigger("Hit");
    }

    public void Finish()
    {
        gameObject.SetActive(false);
    }
}
