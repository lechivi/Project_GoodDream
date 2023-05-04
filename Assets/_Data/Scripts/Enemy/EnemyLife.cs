using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyLife : EnemyAbstract
{
    [SerializeField] private int health;
    [SerializeField] private int maxHealth = 100;

    public int Health { get => this.health; set => this.health = value; }
    public int MaxHealth { get => this.maxHealth; set => this.maxHealth = value; }

    private void Start()
    {
        this.health = this.maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (this.health < 0) return;

        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySFX(AUDIO.SFX_ENEMYHIT);
        }

        this.health -= damage;
        if (this.health <= 0)
        {
            this.health = 0;
            this.Die();
        }
    }

    public void Heal(int amount)
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySFX(AUDIO.SFX_SKILL_HEAL);
        }

        this.health += amount;
        if (this.health > maxHealth)
        {
            this.health = maxHealth;
        }
    }

    private void Die()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySFX(AUDIO.SFX_ENEMYDEATH);
        }

        this.enemyCtrl.EnemyAI.MovementState = MovementState.Death;
        this.enemyCtrl.EnemyAnimator.gameObject.GetComponent<SortingGroup>().sortingOrder = -10;
        this.gameObject.layer = LayerMask.NameToLayer("Death");
        this.enemyCtrl.Rb.bodyType = RigidbodyType2D.Static;

        this.enemyCtrl.SpawnerReward.Spawn();
    }
}
