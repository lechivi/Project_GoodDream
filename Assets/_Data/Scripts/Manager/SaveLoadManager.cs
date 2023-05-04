using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class SaveLoadManager : BaseManager<SaveLoadManager>
{
    public SaveDataSO saveData;

    public void Save()
    {
        if (SceneManager.GetActiveScene().buildIndex < 3 || SceneManager.GetActiveScene().buildIndex > 5) return;
        if (PlayerManager.HasInstance)
        {
            this.saveData.MaxHealth = PlayerManager.Instance.MaxHealth;
            this.saveData.MaxMana = PlayerManager.Instance.MaxMana;
            this.saveData.CurrentHealth = PlayerManager.Instance.CurrentHealthSave;
            this.saveData.CurrentMana = PlayerManager.Instance.CurrentManaSave;
            this.saveData.ReloadSpeed = PlayerManager.Instance.MulReloadSpeed;
            this.saveData.characterSO = PlayerManager.Instance.CharacterSO;

            this.saveData.listWeapon = PlayerManager.Instance.ListWeaponObj;
            this.saveData.listHotkey = PlayerManager.Instance.Hotkeys;
            this.saveData.EquipWeapon = PlayerManager.Instance.CurrentWeapon;

            this.saveData.CurrentScene = SceneManager.GetActiveScene().buildIndex;

            this.saveData.Saved = true;
            
            Debug.Log("SaveGame");
        }
    }

    public void Load()
    {
        if (this.saveData.CurrentScene < 3 || this.saveData.CurrentScene > 5) return;

        if (PlayerManager.HasInstance)
        {
            PlayerManager.Instance.MaxHealth = this.saveData.MaxHealth;
            PlayerManager.Instance.MaxMana = this.saveData.MaxMana;
            PlayerManager.Instance.CurrentHealth = this.saveData.CurrentHealth;
            PlayerManager.Instance.CurrentMana = this.saveData.CurrentMana;
            PlayerManager.Instance.MulReloadSpeed = this.saveData.ReloadSpeed;
            PlayerManager.Instance.CharacterSO = this.saveData.characterSO;

            PlayerManager.Instance.ListWeaponObj = this.saveData.listWeapon;
            PlayerManager.Instance.Hotkeys = this.saveData.listHotkey;
            PlayerManager.Instance.CurrentWeapon = this.saveData.EquipWeapon;

            //SceneManager.LoadScene(this.saveData.CurrentScene);
            if (GameManager.HasInstance)
            {
            }
            GameManager.Instance.LoadChangeScene(this.saveData.CurrentScene);
            
            Debug.Log("LoadGame");
        }
    }
}
