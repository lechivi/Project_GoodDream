using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponMagic : Weapon
{
    [Header("MAGIC SETTING")]
    [SerializeField] protected List<Transform> shootPoints = new List<Transform>();
    [SerializeField] protected GameObject bulletSpellPrefab;
    [SerializeField] protected int manaCostAttack;
    [SerializeField] protected int manaCostSpell;
    [SerializeField] protected float spellSpeed;
    [SerializeField] protected float cooldownTimeAttackMove;
    [SerializeField] protected float cooldownTimeSpellMove;

    protected float timerSpellMove;
    protected float timerAttackMove;
    protected bool isReadyAttackMove;
    protected bool isReadySpellMove;
    protected bool isStartCooldownAttackMove;
    protected bool isStartCooldownSpellMove;

    public float CooldownTimeAttackMove => this.cooldownTimeAttackMove;
    public float CooldownTimeSpellMove => this.cooldownTimeSpellMove;

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

        if (this.isStartCooldownAttackMove)
        {
            this.CooldownAttackMove();
        }
        if (this.isStartCooldownSpellMove)
        {
            this.CooldownSpellMove();
        }

        if (this.IsUsing && gameObject.CompareTag("PlayerWeapon") && this.weaponParent.PlayerCtrl.PlayerLife.Health > 0)
        {

            if (this.weaponParent.PlayerCtrl.PlayerMagic.Mana > 0)
            {
                if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
                {
                    this.AttackMove();
                }
                if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
                {
                    this.SpellMove();
                }
            }

            if (UIManager.HasInstance)
            {
                GamePanel gamePanel = UIManager.Instance.GamePanel;
                gamePanel.SecondMove.gameObject.SetActive(true);
                gamePanel.FirstMove.Icon.sprite = gamePanel.Magic1;
                gamePanel.SecondMove.Icon.sprite = gamePanel.Magic2;
            }
        }

    }

    private void CooldownAttackMove()
    {
        this.timerAttackMove += Time.deltaTime;
        if (UIManager.HasInstance && this.IsUsing && gameObject.CompareTag("PlayerWeapon"))
        {
            UIManager.Instance.GamePanel.FirstMove.StartFillMove(this.timerAttackMove, this.cooldownTimeAttackMove);
        }
        if (this.timerAttackMove < this.cooldownTimeAttackMove) return;

        this.timerAttackMove = 0;
        this.isReadyAttackMove = true;
        this.isStartCooldownAttackMove = false;
    }

    private void CooldownSpellMove()
    {
        this.timerSpellMove += Time.deltaTime;
        if (UIManager.HasInstance && this.IsUsing && gameObject.CompareTag("PlayerWeapon"))
        {
            UIManager.Instance.GamePanel.SecondMove.StartFillMove(this.timerSpellMove, this.cooldownTimeSpellMove);
        }
        if (this.timerSpellMove < this.cooldownTimeSpellMove) return;

        this.timerSpellMove = 0;
        this.isReadySpellMove = true;
        this.isStartCooldownSpellMove = false;
    }

    protected virtual void AttackMove()
    {
        this.isReadyAttackMove = false;
        this.isStartCooldownAttackMove = true;

        this.animator.SetTrigger("Attack");
    }

    public void ShootFrameAnimation() //Call at a shoot frame in animation
    {
        this.ShootBulletSpell();
    }

    protected virtual void ShootBulletSpell()
    {
        foreach (Transform child in this.shootPoints)
        {
            this.GetBulletSpell(child, true);
        }
        this.weaponParent.PlayerCtrl.PlayerMagic.UseMana(this.manaCostAttack);

        if ( UIManager.HasInstance)
        {
            PlayerMagic.playerManaDelegate(this.weaponParent.PlayerCtrl.PlayerMagic.Mana, true);
        }
    }

    private GameObject GetBulletSpell(Transform shootPoint, bool isPlayer)
    {
        int damage = this.GetRandomDamage();
        GameObject bulletSpellObj = Instantiate(this.bulletSpellPrefab, shootPoint.position, shootPoint.transform.rotation/*, this.weaponParent.SpawnPool*/);
        bulletSpellObj.tag = isPlayer ? "PlayerWeapon" : "EnemyWeapon";
        bulletSpellObj.layer = LayerMask.NameToLayer(isPlayer ? "Player" : "Enemy");

        BulletScript bulletScript = bulletSpellObj.GetComponent<BulletScript>();
        bulletScript.WeaponParent = this.weaponParent;
        bulletScript.Damage = damage;
        bulletScript.IsCritical = damage == this.maxDamage;

        Rigidbody2D rb = bulletSpellObj.GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.AddForce(bulletSpellObj.transform.up * this.spellSpeed, ForceMode2D.Impulse);

        return bulletSpellObj;
    }

    protected virtual void SpellMove()
    {
        //For overrite
    }

    public void EnemyShootFrameAnimation() //Call at a shoot frame in animation (enemy)
    {
        foreach (Transform child in this.shootPoints)
        {
            this.GetBulletSpell(child, false);
        }
    }

    public override void EnemyUseWeapon()
    {
        this.animator.SetTrigger("EnemyAttack");
    }

    public virtual void EnemyUseSpellMove()
    {
        //For overrite
    }
}
