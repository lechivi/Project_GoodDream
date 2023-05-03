using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaCollectible : Collectible
{
    protected override void Collect()
    {
        base.Collect();
        target.gameObject.GetComponent<PlayerCollector>().CollectMana(this.value);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerCollector"))
        {
            if (collision.gameObject.GetComponent<PlayerCollector>().CanCollectMana())
            {
                this.target = collision.gameObject.transform;
                this.isFly = true;
            }
        }
    }
}
