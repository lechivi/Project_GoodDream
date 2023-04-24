using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLife : PlayerAbstract
{
    public delegate void PlayerHealth(int health, int maxHealth);
    public static PlayerHealth playerHealthDelegate;

    [SerializeField] private int health;
    [SerializeField] private int maxHealth;

    public int Health { get => this.health; set => this.health = value; }
    public int MaxHealth { get => this.maxHealth; set => this.health = value; }

    protected override void Awake()
    {
        base.Awake();

    }

    private void Start()
    {
        //this.health = PlayerPrefs.GetInt("PLayerHealth", this.maxHealth);
        if (GameManager.HasInstance)
        {
            this.health = GameManager.Instance.CurrentHealth;
            this.maxHealth = GameManager.Instance.MaxHealth;
        }
    }

    public void TakeDamage(int damage)
    {
        if (this.health < 0) return;

        Debug.Log($"HP -{damage} ({this.health}/{this.maxHealth})");
        this.health -= damage;
        if (this.health <= 0)
        {
            this.health = 0;
            this.Die();
        }
        //PlayerPrefs.SetInt("PLayerHealth", this.health);
        if (GameManager.HasInstance)
        {
            GameManager.Instance.UpdateHealth(this.health);
            playerHealthDelegate(this.health, this.maxHealth);
        }
    }

    public void Heal(int amount)
    {
        if (this.health >= this.maxHealth) return;

        //Debug.Log("Player +" + amount);
        this.health += amount;
        if (this.health > this.maxHealth)
            this.health = this.maxHealth;
        //PlayerPrefs.SetInt("PLayerHealth", this.health);
        if (GameManager.HasInstance)
        {
            GameManager.Instance.UpdateHealth(health);
            playerHealthDelegate(this.health, this.maxHealth);
        }
    }

    public void UpgradeMaxHealth(int amout)
    {
        this.maxHealth += amout;
        if (GameManager.HasInstance)
        {
            GameManager.Instance.UpdateMaxHealth(maxHealth);
            playerHealthDelegate(this.health, this.maxHealth);
        }
    }

    private void Die()
    {
        this.playerCtrl.PlayerMovement.MovementState = MovementState.Death;
        this.gameObject.layer = LayerMask.NameToLayer("Death");
        this.playerCtrl.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
    }
}
