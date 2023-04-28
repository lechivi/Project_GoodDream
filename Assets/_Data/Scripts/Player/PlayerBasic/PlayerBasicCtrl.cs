using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasicCtrl : MonoBehaviour
{
    public static PlayerBasicCtrl instance;
    [SerializeField] private PlayerBasicMovement playerMovement;
    [SerializeField] private PlayerBasicHolder playerHolder;

    public PlayerBasicMovement PlayerMovement => this.playerMovement;
    public PlayerBasicHolder PlayerHolder => this.playerHolder;

    private void Awake()
    {
        PlayerBasicCtrl.instance = this;
    }
}
