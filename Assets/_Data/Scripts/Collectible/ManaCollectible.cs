using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaCollectible : MonoBehaviour
{
    [SerializeField] private int manaVakue = 1;
    [SerializeField] private float launchForce = 8f;
    [SerializeField] private float launchAngle = 60f;
    [SerializeField] private float flyTime = 0.25f;

    private Rigidbody2D rb;

    private void Awake()
    {
        this.rb = GetComponent<Rigidbody2D>();
        this.rb.gravityScale = 3;
        this.rb.bodyType = RigidbodyType2D.Static;
    }

    private void Start()
    {
        this.Bound();
    }

    private void Bound()
    {
        this.rb.bodyType = RigidbodyType2D.Dynamic;
        float randomAngle = Random.Range(180 - this.launchAngle, this.launchAngle);
        float randomTime = Random.Range(this.flyTime - 0.5f, this.flyTime + 0.5f);
        Vector2 launchDirection = new Vector2(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad));
        this.rb.AddForce(launchDirection * Random.Range(this.launchForce - 2f, this.launchForce + 2f), ForceMode2D.Impulse);

        Invoke("SetRigidStatic", randomTime);
    }
    private void SetRigidStatic()
    {
        this.rb.bodyType = RigidbodyType2D.Static;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}
