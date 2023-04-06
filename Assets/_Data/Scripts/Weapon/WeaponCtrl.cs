using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponCtrl : MonoBehaviour
{
    [SerializeField] private AudioClip swapWeaponAudio;
    [SerializeField] private List<GameObject> listWeapon = new List<GameObject>();
    [SerializeField] private Slider sliderAmmo;
    [SerializeField] private TextMeshProUGUI textAmmo;


    //private PlayerLife playerLife;
    private int currentWeapon;

    private void Awake()
    {
        //this.playerLife = GameObject.Find("Player").GetComponent<PlayerLife>();

        foreach (Transform child in transform) 
        {
            child.gameObject.SetActive(false);
            this.listWeapon.Add(child.gameObject);
        }

        this.currentWeapon = 0;
        this.listWeapon[currentWeapon].SetActive(true);
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
        

        if (this.listWeapon[currentWeapon].GetComponent<WeaponShooting>().IsReloading()) return;

        this.SwapWeapon();
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
        //    AudioManager.Instance.PlaySE(this.swapWeaponAudio);
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
