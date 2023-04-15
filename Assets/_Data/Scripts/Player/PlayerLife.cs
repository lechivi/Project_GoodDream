using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private int health;
    [SerializeField] private int maxHealth = 100;

    public int Health { get => this.health; set => this.health = value; }
    public int MaxHealth { get => this.maxHealth; set => this.maxHealth = value; }

    private void Start()
    {
        this.health = PlayerPrefs.GetInt("PLayerHealth", this.maxHealth);
    }

    public void TakeDamage(int damage)
    {
        if (this.health < 0) return;

        this.health -= damage;
        if (this.health <= 0)
        {
            this.health = 0;
            this.Die();
        }
        PlayerPrefs.SetInt("PLayerHealth", this.health);
    }

    public void Heal(int amount)
    {
        this.health += amount;
        if (this.health > this.maxHealth)
            this.health = this.maxHealth;
        PlayerPrefs.SetInt("PLayerHealth", this.health);
    }

    public void UpgradeMaxHealth(int amout)
    {
        this.maxHealth += amout;
    }

    private void Die()
    {
        this.playerMovement.movementState = MovementState.Death;
        this.gameObject.layer = LayerMask.NameToLayer("Death");
    }
}
