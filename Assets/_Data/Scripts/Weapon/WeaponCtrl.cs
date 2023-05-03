using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponCtrl : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private AudioClip swapWeaponAudio;
    [SerializeField] private List<GameObject> listWeapon = new List<GameObject>();
    [SerializeField] private Slider sliderAmmo;
    [SerializeField] private TextMeshProUGUI textAmmo;


    //private PlayerLife playerLife;
    private int currentWeapon;
    private Vector2 direction;

    private void Awake()
    {
        //this.playerLife = GameObject.Find("Player").GetComponent<PlayerLife>();

        //foreach (Transform child in transform) 
        //{
        //    child.gameObject.SetActive(false);
        //    this.listWeapon.Add(child.gameObject);
        //}

        //this.currentWeapon = 0;
        //this.listWeapon[currentWeapon].SetActive(true);
    }

    private void Start()
    {
        //if (UIManager.HasInstance)
        //{
        //    this.sliderAmmo = GameObject.Find("Slider_Ammo").GetComponent<Slider>();
        //    this.textAmmo = this.sliderAmmo.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        //    this.SetSliderMaxValue();
        //    this.SetSliderValue();
        //}
    }

    private void Update()
    {
        //if (this.playerLife.IsDead())
        //{
        //    this.listWeapon[currentWeapon].SetActive(false);
        //    return;
        //}
        

        //if (this.listWeapon[currentWeapon].GetComponent<WeaponShooting>().IsReloading()) return;

        this.SwapWeapon();
    }

    private void FixedUpdate()
    {
        this.FaceGun();
    }

    private void FaceGun()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        this.direction = mousePos - (Vector2)transform.position;
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, this.playerMovement.IsFacingRight ? rotZ : rotZ - 180);
        //this.holderItems.transform.right = this.direction;
    }

    private void SwapWeapon()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            this.SetWeapon(this.currentWeapon + 1);
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            this.SetWeapon(this.currentWeapon - 1);
        }
    }

    private void SetWeapon(int index)
    {
        //if (AudioManager.HasInstance)
        //{
        //    AudioManager.Instance.PlaySFX(this.swapWeaponAudio);
        //}

        if (index >= this.listWeapon.Count)
            index = 0;
        else if (index < 0)
            index = this.listWeapon.Count - 1;

        this.listWeapon[currentWeapon].SetActive(false);
        this.currentWeapon = index;
        this.listWeapon[index].SetActive(true);

        //this.SetSliderMaxValue();
        //this.SetSliderValue();
    }

    //private void SetSliderMaxValue()
    //{
    //    if (!UIManager.HasInstance) return;
    //    this.sliderAmmo.maxValue = this.listWeapon[currentWeapon].GetComponent<WeaponShooting>().MaxAmmo();
    //}

    //public void SetSliderValue()
    //{
    //    if (!UIManager.HasInstance) return;
    //    this.sliderAmmo.value = this.listWeapon[currentWeapon].GetComponent<WeaponShooting>().CurrentAmmo();
    //    this.textAmmo.SetText($"{this.sliderAmmo.value}/{this.sliderAmmo.maxValue}");
    //}
}
