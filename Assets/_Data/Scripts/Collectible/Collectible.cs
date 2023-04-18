using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] protected int value = 1;
    [SerializeField] protected float launchForce = 8f;
    [SerializeField] protected float launchAngle = 60f;
    [SerializeField] protected float flyTime = 0.25f;

    protected Transform target;
    protected Rigidbody2D rb;
    protected TrailRenderer trailRenderer;
    protected bool isFly;

    protected virtual void Awake()
    {
        this.rb = GetComponent<Rigidbody2D>();
        this.trailRenderer = GetComponentInChildren<TrailRenderer>();

        this.rb.gravityScale = 3;
        this.rb.bodyType = RigidbodyType2D.Static;
        this.trailRenderer.enabled = false;
    }

    protected virtual void Start()
    {
        this.Bound();
    }

    protected virtual void Update()
    {
        if (this.isFly)
        {
            this.FlyToTarget();
        }
    }

    protected virtual void Bound()
    {
        this.rb.gravityScale = 3;
        this.rb.bodyType = RigidbodyType2D.Dynamic;
        this.trailRenderer.enabled = true;

        float randomAngle = Random.Range(180 - this.launchAngle, this.launchAngle);
        float randomTime = Random.Range(this.flyTime - 0.5f, this.flyTime + 0.5f);
        Vector2 launchDirection = new Vector2(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad));
        this.rb.AddForce(launchDirection * Random.Range(this.launchForce - 2f, this.launchForce + 2f), ForceMode2D.Impulse);

        Invoke("SetRigidStatic", randomTime);
    }

    protected virtual void SetRigidStatic()
    {
        this.rb.bodyType = RigidbodyType2D.Static;
        this.trailRenderer.enabled = false;
    }

    protected virtual void FlyToTarget()
    {
        this.rb.bodyType = RigidbodyType2D.Dynamic;
        this.rb.gravityScale = 0;
        this.trailRenderer.enabled = true;

        if (Vector2.Distance(transform.position, this.target.position) <= 0.4f)
        {
            this.Collect();
            this.isFly = false;
            this.SetRigidStatic();
            gameObject.SetActive(false); //TODO: Pooling object
        }

        transform.position = Vector2.MoveTowards(transform.position, this.target.position, 10f * Time.deltaTime);
    }

    protected virtual void Collect()
    {
        //For overrite
    }
}
