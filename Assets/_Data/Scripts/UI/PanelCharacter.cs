using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PanelCharacter : MonoBehaviour
{
    [SerializeField] private List<CharacterSO> listCharacter = new List<CharacterSO>();
    [SerializeField] private List<CharacterInformation> listCharacterInfor = new List<CharacterInformation>();
    [SerializeField] private GameObject characterModelPrefab;
    [SerializeField] private Transform parentList;
    [SerializeField] private Sprite unchooseSprite;
    [SerializeField] private Sprite chooseSprite;

    private void Start()
    {
        foreach(CharacterSO characterSO in listCharacter)
        {
            GameObject model = Instantiate(this.characterModelPrefab, this.parentList);
            CharacterInformation characterInformation = model.GetComponent<CharacterInformation>();
            characterInformation.PanelCharacter = this;
            characterInformation.CharacterSO = characterSO;
            characterInformation.TransferModel();
            this.listCharacterInfor.Add(characterInformation);
        }    
    }

    public void ChooseCharacter(CharacterInformation characterInformation)
    {
        foreach(CharacterInformation character in listCharacterInfor)
        {
            character.Frame.sprite = character == characterInformation ? chooseSprite : unchooseSprite;
        }

        if (PlayerManager.HasInstance)
        {
            PlayerManager.Instance.SetPlayerModel(characterInformation.CharacterSO);
        }
    }
}
