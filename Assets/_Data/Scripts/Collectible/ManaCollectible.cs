using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaCollectible : MonoBehaviour
{
    [SerializeField] private int manaValue = 1;
    [SerializeField] private float launchForce = 8f;
    [SerializeField] private float launchAngle = 60f;
    [SerializeField] private float flyTime = 0.25f;

    private Transform target;
    private Rigidbody2D rb;
    private TrailRenderer trailRenderer;
    private bool isFly;

    private void Awake()
    {
        this.rb = GetComponent<Rigidbody2D>();
        this.trailRenderer =GetComponentInChildren<TrailRenderer>();

        this.rb.gravityScale = 3;
        this.rb.bodyType = RigidbodyType2D.Static;
        this.trailRenderer.enabled = false;
    }

    private void Start()
    {
        this.Bound();
    }

    private void Update()
    {
        if (this.isFly)
        {
            this.FlyToTarget();
        }
    }

    private void Bound()
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

    private void SetRigidStatic()
    {
        this.rb.bodyType = RigidbodyType2D.Static;
        this.trailRenderer.enabled = false;
    }

    private void FlyToTarget()
    {
        this.rb.bodyType = RigidbodyType2D.Dynamic;
        this.rb.gravityScale = 0;
        this.trailRenderer.enabled = true;

        if (Vector2.Distance(transform.position, this.target.position) <= 0.4f)
        {
            target.gameObject.GetComponent<PlayerCollector>().CollectMana(this.manaValue);
            this.isFly = false;
            this.SetRigidStatic();
            gameObject.SetActive(false); //TODO: Pooling object
        }

        transform.position = Vector2.MoveTowards(transform.position, this.target.position, 10f * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerCollector"))
        {
                Debug.Log("CanFly");
            if (collision.gameObject.GetComponent<PlayerCollector>().CanCollectMana())
            {
                this.target = collision.gameObject.transform;
                this.isFly = true;
            }
        }
    }
}
