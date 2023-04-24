using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem instance;

    [Header("LEFT PAGE_Player Model")]
    [SerializeField] private SpriteRenderer weaponModel;

    [Header("LEFT PAGE_Hotkeys")]
    [SerializeField] private GameObject hotkey1;
    [SerializeField] private GameObject hotkey2;
    [SerializeField] private GameObject hotkey3;
    [SerializeField] private GameObject hotkey4;
    [SerializeField] private GameObject hotkey5;

    [Header("LEFT PAGE_Weapon Information")]
    [SerializeField] private Image imageSkill1;
    [SerializeField] private Image imageSkill2;
    [SerializeField] private TMP_Text textNameWeapon;
    [SerializeField] private TMP_Text textDamageWeapon;

    [Header("RIGHT PAGE_ List Weapon")]
    [SerializeField] private GameObject listWeaponPanel;

    [Header("RIGHT PAGE_ Evolution")]
    [SerializeField] private Image imageFalse;
    [SerializeField] private Image imageOrigin;
    [SerializeField] private Image imageEvolution;
    [SerializeField] private TMP_Text textNameOrigin;
    [SerializeField] private TMP_Text textNameEvolution;
    [SerializeField] private TMP_Text textPercent;


    [Header("REFERENCE")]
    [SerializeField] private GameObject slotInventoryPrefab;
    [SerializeField] private Sprite imageMelee1;
    [SerializeField] private Sprite imageMelee2;
    [SerializeField] private Sprite imageShooting;
    [SerializeField] private Sprite imageMagic1;
    [SerializeField] private Sprite imageMagic2;

    private List<GameObject> weaponsInventory = new List<GameObject>();

    private void Awake()
    {
        InventorySystem.instance = this;
        this.CreateListWeapon();
        this.EquipWeaponInventory(0);
    }

    private void Start()
    {

    }

    public void OnEnable()
    {
        this.EquipWeaponInventory(PlayerManager.Instance.CurrentWeapon);
    }

    public void CreateListWeapon()
    {
        int indexWeapon = 0;
        foreach (GameObject weaponObj in PlayerManager.Instance.ListWeaponObj)
        {
            Weapon weapon = weaponObj.GetComponent<Weapon>();
            if (weapon != null)
            {
                GameObject slot = Instantiate(this.slotInventoryPrefab, this.listWeaponPanel.transform);
                slot.GetComponentInChildren<DraggableInventory>().IndexWeapon = indexWeapon;
                slot.GetComponentInChildren<WeaponTransferInfor>().WeaponSO = weapon.WeaponSO;

                this.weaponsInventory.Add(slot);
                indexWeapon++;
            }

        }
    }

    public void TransferInfor(WeaponSO weaponSO)
    {
        string damage = weaponSO.minDamage == weaponSO.maxDamage ? weaponSO.maxDamage.ToString() : $"{weaponSO.minDamage}-{weaponSO.maxDamage}";
        this.textNameWeapon.SetText(weaponSO.weaponName);
        this.textDamageWeapon.SetText("DMG: " + damage);
 

        if (weaponSO.type == WeaponType.Melee)
        {
            this.imageSkill2.gameObject.SetActive(true);

            this.imageSkill1.sprite = this.imageMelee1;
            this.imageSkill2.sprite = this.imageMelee2;
        }
        else if (weaponSO.type == WeaponType.Shooting)
        {
            this.imageSkill2.gameObject.SetActive(false);

            this.imageSkill1.sprite = this.imageShooting;
        }
        else if (weaponSO.type == WeaponType.Magic)
        {
            this.imageSkill2.gameObject.SetActive(true);

            this.imageSkill1.sprite = this.imageMagic1;
            this.imageSkill2.sprite = this.imageMagic2;
        }

        if (weaponSO is WeaponNormalSO)
        {
            this.imageFalse.gameObject.SetActive(true);
            this.imageEvolution.gameObject.SetActive(false);

            WeaponNormalSO self = weaponSO as WeaponNormalSO;

            if (self.image != null)
            {
                this.imageOrigin.sprite = self.image;
                this.imageOrigin.SetNativeSize();
            }
            this.textNameOrigin.SetText(self.weaponName);
            this.textPercent.SetText("false");
            this.textNameEvolution.SetText(" ");

        }
        else if (weaponSO is WeaponPowerSO)
        {
            this.imageFalse.gameObject.SetActive(false);
            this.imageEvolution.gameObject.SetActive(true);

            WeaponPowerSO self = weaponSO as WeaponPowerSO;
            WeaponNormalSO origin = self.originWeapon;

            if (origin.image != null)
            {
                this.imageOrigin.sprite = origin.image;
                this.imageOrigin.SetNativeSize();
            }
            this.textNameOrigin.SetText(origin.weaponName);
            this.textPercent.SetText(origin.rateEvo + "%");

            if (self.image != null)
            {
                this.imageEvolution.sprite = weaponSO.image;
                this.imageEvolution.SetNativeSize();
            }
            this.textNameEvolution.SetText(self.weaponName);
        }
    }

    public void SelectWeaponInventory(int index)
    {
        for (int i = 0; i < this.weaponsInventory.Count; i++)
        {
            if (i == index)
            {
                WeaponTransferInfor weaponTransferInfor = this.weaponsInventory[i].GetComponentInChildren<WeaponTransferInfor>();

                weaponTransferInfor.SetSelectWeapon(true);
                this.TransferInfor(weaponTransferInfor.WeaponSO);
            }
            else
            {
                this.weaponsInventory[i].GetComponentInChildren<WeaponTransferInfor>().SetSelectWeapon(false);
            }
        }
    }
    public void EquipWeaponInventory(int index)
    {
        for (int i = 0; i < this.weaponsInventory.Count; i++)
        {
            if (i == index)
            {
                WeaponTransferInfor weaponTransferInfor = this.weaponsInventory[i].GetComponentInChildren<WeaponTransferInfor>();

                weaponTransferInfor.SetEquipWeapon(true);
                this.weaponModel.sprite = weaponTransferInfor.WeaponSO.image;
                this.TransferInfor(weaponTransferInfor.WeaponSO);

            }
            else
            {
                this.weaponsInventory[i].GetComponentInChildren<WeaponTransferInfor>().SetEquipWeapon(false);
            }
        }

        PlayerManager.Instance.EquipWeaponFromInventoryToPlayer(index);
    }

}
