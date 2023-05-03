using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class TestScript : MonoBehaviour
{
    [SerializeField] private SaveDataSO saveDataSO;

    //public void OnClickedSaveButton()
    //{
    //    SaveLoadManager.Instance.SaveGameData(saveDataSO, "savegame.json");

    //} 
    
    //public void OnClickedLoadButton()
    //{
    //    SaveDataSO dataLoad = SaveLoadManager.Instance.LoadGameData("savegame.json");
    //    Debug.Log(dataLoad.MaxHealth);
    //}
}
