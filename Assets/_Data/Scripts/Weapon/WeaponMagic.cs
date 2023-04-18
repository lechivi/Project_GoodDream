using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMagic : Weapon
{
    [Header("MAGIC SETTING")]
    [SerializeField] protected List<Transform> shootPoints = new List<Transform>();
    [SerializeField] protected GameObject spellPrefab;
    [SerializeField] protected int manaCost;
    [SerializeField] protected float cooldownTime;
    [SerializeField] protected float spellSpeed;

    protected float timerCooldown;
    protected bool isReadyMove;
    protected bool isStartCooldown;

    protected override void Awake()
    {
        base.Awake();
        this.WeaponType = WeaponType.Magic;
        foreach(Transform child in transform)
        {
            if (child.gameObject.name == "ShootPoint")
                this.shootPoints.Add(child);
        }
    }

    protected override void Update()
    {
        if (GameManager.HasInstance)
        {
            if (!GameManager.Instance.IsPlaying) return;
        }

        if (this.isStartCooldown)
        {
            this.CooldownMove();
        }

        if (this.IsUsing && gameObject.CompareTag("PlayerWeapon"))
        {
            if (Input.GetMouseButtonDown(0) && this.weaponParent.PlayerCtrl.PlayerMagic.Mana > 0)
            {
                this.AttackMove();
            }

            if (UIManager.HasInstance)
            {
                GamePanel gamePanel = UIManager.Instance.GamePanel;
                gamePanel.SecondMove.gameObject.SetActive(true);
                gamePanel.FirstMove.Icon.sprite = gamePanel.Magic1;
                gamePanel.FirstMove.Icon.sprite = gamePanel.Magic2;
            }
        }

    }

    private void CooldownMove()
    {
        this.timerCooldown += Time.deltaTime;
        if (this.timerCooldown < this.cooldownTime) return;

        this.timerCooldown = 0;
        this.isReadyMove = true;
        this.isStartCooldown = false;
    }

    protected virtual void AttackMove()
    {
        this.isReadyMove = false;
        this.isStartCooldown = true;

        this.animator.SetTrigger("Attack");
    }

    public void ShootFrameAnimation() //Call at a shoot frame in animation
    {
        this.SpellMove();
    }

    protected virtual void SpellMove()
    {
        foreach (Transform child in this.shootPoints)
        {
            this.GetSpell(child);
        }
        this.weaponParent.PlayerCtrl.PlayerMagic.UseMana(this.manaCost);

        if ( UIManager.HasInstance)
        {
            PlayerMagic.playerManaDelegate(this.weaponParent.PlayerCtrl.PlayerMagic.Mana, true);
        }
    }

    private GameObject GetSpell(Transform shootPoint)
    {
        int damage = this.GetRandomDamage();
        GameObject spellObj = Instantiate(this.spellPrefab, shootPoint.position, shootPoint.transform.rotation, this.weaponParent.SpawnPool);
        spellObj.tag = "PlayerWeapon";
        spellObj.layer = LayerMask.NameToLayer("Player");

        //TODO: change BulletScript to fitter name
        BulletScript bulletScript = spellObj.GetComponent<BulletScript>();
        bulletScript.WeaponParent = this.weaponParent;
        bulletScript.Damage = damage;
        bulletScript.IsCritical = damage == this.maxDamage;

        Rigidbody2D rb = spellObj.GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.AddForce(spellObj.transform.up * this.spellSpeed, ForceMode2D.Impulse);

        return spellObj;
    }
    public override void EnemyUseWeapon()
    {
        base.EnemyUseWeapon();
    }
}
