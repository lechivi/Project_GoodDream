using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : PlayerAbstract
{
    public delegate void PlayerHealth(int health, int maxHealth);
    public static PlayerHealth playerHealthDelegate;

    [SerializeField] private int health;
    [SerializeField] private int maxHealth;

    public int Health { get => this.health; set => this.health = value; }
    public int MaxHealth { get => this.maxHealth; set => this.health = value; }

    public bool Test;

    private void Start()
    {
        //this.health = PlayerPrefs.GetInt("PLayerHealth", this.MaxHealth);
        if (PlayerManager.HasInstance)
        {
            this.health = PlayerManager.Instance.CurrentHealth;
            this.maxHealth = PlayerManager.Instance.MaxHealth;
        }
    }

    public void TakeDamage(int damage)
    {
        if (this.health < 0) return;

        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySFX(AUDIO.SFX_PLAYERHIT);
        }

        //Debug.Log($"HP -{damage} ({this.health}/{this.maxHealth})");
        this.health -= damage;
        if (this.health <= 0)
        {
            if (this.Test)
            {
                this.health = this.maxHealth;
            }
            else
            {
                this.health = 0;
                this.Die();
            }
        }
        if (PlayerManager.HasInstance)
        {
            PlayerManager.Instance.UpdateHealth(this.health);
            playerHealthDelegate(this.health, this.maxHealth);
        }
    }

    public void Heal(int amount)
    {
        if (this.health >= this.maxHealth) return;

        this.health += amount;
        if (this.health > this.maxHealth)
            this.health = this.maxHealth;

        if (PlayerManager.HasInstance)
        {
            PlayerManager.Instance.UpdateHealth(health);
            playerHealthDelegate(this.health, this.maxHealth);
        }
    }

    public void UpgradeMaxHealth(int amout)
    {
        this.maxHealth += amout;
        if (PlayerManager.HasInstance)
        {
            PlayerManager.Instance.UpdateMaxHealth(maxHealth);
            playerHealthDelegate(this.health, this.maxHealth);
        }
    }

    private void Die()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySFX(AUDIO.SFX_PLAYERDEATH);
            AudioManager.Instance.PlayBGM(AUDIO.BGM_6_GAMEOVER);
        }

        this.playerCtrl.PlayerMovement.MovementState = MovementState.Death;
        this.gameObject.layer = LayerMask.NameToLayer("Death");
        this.playerCtrl.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

        if (UIManager.HasInstance)
        {
            UIManager.Instance.ActiveLosePanel(true);
        }
    }
}
