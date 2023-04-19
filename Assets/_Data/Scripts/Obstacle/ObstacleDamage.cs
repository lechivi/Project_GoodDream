using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrapType { NailTrap, Lava}
public class ObstacleDamage : MonoBehaviour
{
    [SerializeField] private TrapType trapType;
    [SerializeField] private int damage = 1;
    [SerializeField] private float delay = 1f;
    
    private float timer = 0f;
    private bool isEnter;
    private bool isReady = true;

    public bool IsShowTrap { get; set; }

    private void Update()
    {
        if (this.trapType == TrapType.NailTrap)
        {
            if (this.isEnter && this.IsShowTrap)
            {
                this.TakeDamageTarget();
            }
        }

        else if (this.trapType == TrapType.Lava)
        {
            this.TakeDamageTarget();
        }
    }

    private void TakeDamageTarget()
    {
        if (this.isReady)
        {
            this.isReady = false;
            PlayerLife.instance.TakeDamage(this.damage);
        }
        this.timer += Time.deltaTime;
        if (this.timer < this.delay) return;

        this.timer = 0f;
        this.isReady = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ColliderWithWall") && collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            this.isEnter = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ColliderWithWall") && collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            this.isEnter = false;
            this.isReady = true;
        }
    }
}
