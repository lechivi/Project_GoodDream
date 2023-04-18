using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbstract : MonoBehaviour
{
    protected PlayerCtrl playerCtrl;
    public PlayerCtrl PlayerCtrl => this.playerCtrl;

    protected virtual void Awake()
    {
        this.playerCtrl = transform.parent.GetComponent<PlayerCtrl>();
    }
}
