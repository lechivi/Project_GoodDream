using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterInformation : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject parentModel;
    [SerializeField] private TMP_Text characterName;
    [SerializeField] private TMP_Text description;

    public Image Frame;
    public PanelCharacter PanelCharacter;
    public CharacterSO CharacterSO;

    //private bool clickedOnce = false;

    public void TransferModel()
    {
        this.characterName.SetText(this.CharacterSO.characterName);
        this.description.SetText(this.CharacterSO.description);
        Instantiate(this.CharacterSO.model.transform.Find("Root"), this.parentModel.transform);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        this.PanelCharacter.ChooseCharacter(this);
        //if (!this.clickedOnce)
        //{
        //    this.clickedOnce = true;
        //    StartCoroutine(this.ResetClick());
        //}
        //else
        //{
        //    //Debug.Log("Double-clicked!");
        //    this.clickedOnce = false;
        //    StopAllCoroutines();

        //    //Unchoose
        //    if (PlayerManager.HasInstance)
        //    {
        //        PlayerManager.Instance.CharacterSO = Resources.Load<CharacterSO>("SO/Character/Asaka");
        //    }
        //}
    }

    //private IEnumerator ResetClick()
    //{
    //    yield return new WaitForSeconds(0.3f);
    //    this.clickedOnce = false;
    //}
}
