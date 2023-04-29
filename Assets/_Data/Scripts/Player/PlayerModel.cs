using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerModel : PlayerAbstract
{
    [SerializeField] private List<GameObject> characterObj = new List<GameObject>();

    private void OnEnable()
    {
        if (PlayerManager.HasInstance)
        {
            this.SetPlayerModel(PlayerManager.Instance.CharacterSO);
        }

    }

    public void SetPlayerModel(CharacterSO characterSO)
    {
        foreach(GameObject character in characterObj)
        {
            if (character.name == characterSO.characterName)
            {
                character.SetActive(true);
                this.playerCtrl.PlayerAnimator = character.GetComponent<Animator>();
            }
            else
            {
                character.SetActive(false);
            }
        }

    }
}
