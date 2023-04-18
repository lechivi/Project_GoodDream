using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDamage : MonoBehaviour
{
    [SerializeField] private int damage = 5;
    [SerializeField] private float delay = 0.25f;

    private Transform target;
    private float timer = 0f;
    private bool isStartCooldowm;
    private bool isReady = true;

    public bool IsShowTrap { get; set; }

    private void Update()
    {
        if (this.isStartCooldowm)
        {
            this.TakeDamageTarget();
        }
    }

    private void TakeDamageTarget()
    {
        if (this.isReady)
        {
            this.isReady = false;
            this.target.gameObject.GetComponent<PlayerLife>().TakeDamage(this.damage);
        }
        this.timer += Time.deltaTime;
        if (this.timer < this.delay) return;

        this.timer = 0f;
        this.isReady= true;
    }

    public void ResetCooldown()
    {
        this.timer = 0;
        this.isStartCooldowm = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!this.IsShowTrap) return;
        if (collision.gameObject.CompareTag("PlayerBattle"))
        {
            this.isStartCooldowm = true;
            this.target = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!this.IsShowTrap) return;
        if (collision.gameObject.CompareTag("PlayerBattle"))
        {
            this.ResetCooldown();
        }
    }
}
