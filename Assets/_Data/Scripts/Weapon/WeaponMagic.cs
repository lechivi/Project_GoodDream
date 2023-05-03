using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponMagic : Weapon
{
    [Header("MAGIC SETTING")]
    [SerializeField] protected List<Transform> shootPoints = new List<Transform>();
    [SerializeField] protected List<GameObject> bulletClone = new List<GameObject>();
    [SerializeField] protected GameObject bulletSpellPrefab;
    [SerializeField] protected int manaCostAttack;
    [SerializeField] protected int manaCostSpell;
    [SerializeField] protected float spellSpeed;
    [SerializeField] private float spellLifeTime = 2f;
    [SerializeField] protected float cooldownTimeAttackMove;
    [SerializeField] protected float cooldownTimeSpellMove;

    protected int direction;
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
        foreach(Transform child in transform)
        {
            if (child.gameObject.name == "ShootPoint")
                this.shootPoints.Add(child);
        }

        this.isReadyAttackMove = true;
        this.isReadySpellMove = true;
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
                if (UIManager.HasInstance)
                {
                    if (EventSystem.current.IsPointerOverGameObject()) return;
                }

                if (Input.GetMouseButtonDown(0) && this.isReadyAttackMove)
                {
                    this.AttackMove();
                }
                if (Input.GetMouseButtonDown(1) && this.isReadySpellMove)
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

    protected void CooldownAttackMove()
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

    protected void CooldownSpellMove()
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
        //For overrite
    }

    protected virtual void ShootBulletSpell(bool isUp)
    {
        foreach (Transform child in this.shootPoints)
        {
            this.GetBulletSpell(child, true, isUp);
        }
        this.weaponParent.PlayerCtrl.PlayerMagic.UseMana(this.manaCostAttack);

        if ( UIManager.HasInstance)
        {
            PlayerMagic.playerManaDelegate(this.weaponParent.PlayerCtrl.PlayerMagic.Mana, this.weaponParent.PlayerCtrl.PlayerMagic.MaxMana);
        }
    }

    protected GameObject GetBulletSpell(Transform shootPoint, bool isPlayer, bool isUp)
    {
        int damage = this.GetRandomDamage();
        this.direction = 1 * (this.weaponParent.PlayerCtrl.PlayerMovement.IsFacingRight ? 1 : -1);
        
        for (int i = 0; i < this.bulletClone.Count; i++)
        {
            if (!this.bulletClone[i].activeInHierarchy)
            {
                //this.bulletClone[i].name = "BulletClone_" + i;
                this.bulletClone[i].transform.position = shootPoint.position;
                this.bulletClone[i].transform.rotation = shootPoint.rotation;
                this.bulletClone[i].transform.parent = this.weaponParent.SpawnPool;

                BulletScript bulletScriptClone = this.bulletClone[i].GetComponent<BulletScript>();
                bulletScriptClone.LifeTime = this.spellLifeTime;
                bulletScriptClone.WeaponParent = this.weaponParent;
                bulletScriptClone.Damage = damage;
                bulletScriptClone.IsCritical = damage == this.maxDamage;

                Vector2 scale = new Vector2(Mathf.Abs(bulletScriptClone.transform.localScale.x), bulletScriptClone.transform.localScale.y);
                bulletScriptClone.transform.localScale = this.weaponParent.PlayerCtrl.PlayerMovement.IsFacingRight ? new Vector2(-scale.x, scale.y) : scale;

                this.bulletClone[i].SetActive(true);

                Rigidbody2D rbClone = bulletClone[i].GetComponent<Rigidbody2D>();
                rbClone.bodyType = RigidbodyType2D.Dynamic;

                Vector3 vectorClone = isUp ? this.bulletClone[i].transform.up : this.bulletClone[i].transform.right;
                rbClone.AddForce(this.direction * vectorClone * this.spellSpeed, ForceMode2D.Impulse);

                return this.bulletClone[i];
            }
        }

        GameObject obj = Instantiate(this.bulletSpellPrefab, shootPoint.position, shootPoint.transform.rotation, this.weaponParent.SpawnPool);
        obj.tag = isPlayer ? "PlayerWeapon" : "EnemyWeapon";
        obj.layer = LayerMask.NameToLayer(isPlayer ? "Player" : "Enemy");

        BulletScript bullet = obj.GetComponent<BulletScript>();
        bullet.LifeTime = this.spellLifeTime;
        bullet.WeaponParent = this.weaponParent;
        bullet.Damage = damage;
        bullet.IsCritical = damage == this.maxDamage;
        bullet.transform.localScale = this.weaponParent.PlayerCtrl.PlayerMovement.IsFacingRight ? bullet.transform.localScale : new Vector2(-bullet.transform.localScale.x, bullet.transform.localScale.y);

        obj.SetActive(true);

        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;

        Vector3 vector = isUp ? obj.transform.up : obj.transform.right;
        rb.AddForce(this.direction * vector * this.spellSpeed, ForceMode2D.Impulse);
        
        this.bulletClone.Add(obj);

        return obj;
    }

    protected virtual void SpellMove()
    {
        //For overrite
    }

    public void EnemyShootFrameAnimation() //Call at a shoot Frame in animation (enemy)
    {
        foreach (Transform child in this.shootPoints)
        {
            this.GetBulletSpell(child, false, true);
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
