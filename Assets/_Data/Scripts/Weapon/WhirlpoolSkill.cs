using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhirlpoolSkill : MonoBehaviour
{
    [SerializeField] private float lifeTime = 3f;
    [SerializeField] private int damage = 3;
    [SerializeField] private float waveTime = 0.5f;
    [SerializeField] private GameObject damageTextPrefab;

    private List<Collider2D> colliders = new List<Collider2D>();

    private void Start()
    {
        StartCoroutine(this.ApplyDamageOverTime());
        Destroy(gameObject, lifeTime);
    }

    private void OnDestroy()
    {
        foreach(Collider2D collider in colliders)
        {
            if (collider.GetComponent<EnemyAI>() != null)
                collider.GetComponent<EnemyAI>().IsStopMove = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy") && collision.gameObject.CompareTag("ColliderWithWall"))
        {
            if (collision.GetComponentInChildren<EnemyAI>() != null)
            {
                collision.GetComponentInChildren<EnemyAI>().IsStopMove = true;
            }

            if (!this.colliders.Contains(collision))
            {
                this.colliders.Add(collision);
            }
        }
    }

    private IEnumerator ApplyDamageOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(this.waveTime);

            foreach (Collider2D collider in colliders)
            {
                if (collider.GetComponent<EnemyAI>() == null)
                {
                    this.SpawnDamageText(this.damage, collider, collider.transform);
                }
                else
                {
                    EnemyCtrl enemyCtrl = collider.GetComponent<EnemyAI>().EnemyCtrl;
                    if (enemyCtrl.EnemyLife.Health > 0)
                    {
                        enemyCtrl.EnemyLife.TakeDamage(this.damage);
                        this.SpawnDamageText(this.damage, collider, enemyCtrl.NeverFlip.transform);
                    }
                }
            }
        }
    }

    public void SpawnDamageText(int damage, Collider2D collision, Transform parent)
    {
        Vector3 position = new Vector3(collision.transform.position.x + Random.Range(-0.5f, 0.5f), collision.transform.position.y + 1.4f, collision.transform.position.z);
        GameObject damageTextObject = Instantiate(this.damageTextPrefab, position, Quaternion.identity, parent);

        damageTextObject.GetComponentInChildren<TextMesh>().text = "-" + damage.ToString();
        damageTextObject.GetComponentInChildren<TextMesh>().color = Color.white;
        damageTextObject.GetComponentInChildren<TextMesh>().fontSize = 50;
    }
}
