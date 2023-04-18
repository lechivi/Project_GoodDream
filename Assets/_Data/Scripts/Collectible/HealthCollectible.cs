using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : Collectible
{
    protected override void Collect()
    {
        target.gameObject.GetComponent<PlayerCollector>().CollectHealth(this.value);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerCollector"))
        {
            if (collision.gameObject.GetComponent<PlayerCollector>().CanCollectHealth())
            {
                this.target = collision.gameObject.transform;
                this.isFly = true;
            }
        }
    }
}
