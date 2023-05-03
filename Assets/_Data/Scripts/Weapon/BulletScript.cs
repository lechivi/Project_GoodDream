using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer hitSprite;
    [SerializeField] private SpriteRenderer bulletSprite;

    public WeaponParent WeaponParent { get; set; }
    public int Damage { get; set; }
    public float LifeTime { get; set; }
    public bool IsCritical { get; set; }

    private void Start()
    {
        Physics2D.IgnoreLayerCollision(gameObject.layer, gameObject.layer);
    }

    private void OnEnable()
    {
        this.bulletSprite.enabled = true;
        this.trailRenderer.emitting = true;
        this.animator.Rebind();
        Invoke("HitSomething", this.LifeTime);
    }

    private void OnDisable()
    {
        CancelInvoke("HitSomething");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("EnemyWeapon") && !collision.gameObject.CompareTag("PlayerWeapon") &&!collision.gameObject.CompareTag("ColliderWithWall") && !collision.gameObject.CompareTag("Detector") && !collision.gameObject.CompareTag("PlayerCollector"))
        {
            this.HitSomething();
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && collision.gameObject.CompareTag("PlayerBattle") && gameObject.CompareTag("EnemyWeapon"))
        {
            if (collision.gameObject.GetComponent<PlayerLife>().Health <= 0) return;
            collision.gameObject.GetComponent<PlayerLife>().TakeDamage(Damage);
            this.HitSomething();
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
            this.HitSomething();
        }
    }

    private void HitSomething()
    {
        
        this.bulletSprite.enabled = false;
        this.rb.bodyType = RigidbodyType2D.Static;
        this.animator.SetTrigger("Hit");
    }

    public void Finish()
    {
        this.trailRenderer.emitting = false;
        this.hitSprite.sprite = null;
        gameObject.SetActive(false);
    }
}
