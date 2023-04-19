using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDamageRemote : MonoBehaviour
{
    [SerializeField] private ObstacleDamage target;
    private Animator animator;

    private void Awake()
    {
        this.animator = GetComponent<Animator>();

        this.animator.SetTrigger("Remote");
    }
    public void TurnOnTarget() //Call in animation frame
    {
        this.target.IsShowTrap = true;
    }

    public void TurnOffTarget() //Call in animation frame
    {
        this.target.IsShowTrap = false;
        this.target.ResetCooldown();
    }
}
