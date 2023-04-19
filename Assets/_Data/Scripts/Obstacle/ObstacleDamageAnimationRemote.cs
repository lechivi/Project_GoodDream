using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDamageAnimationRemote : MonoBehaviour
{
    [SerializeField] private ObstacleDamage obstacle;
    private Animator animator;

    private void Awake()
    {
        this.animator = GetComponent<Animator>();

        this.animator.SetTrigger("Remote");
    }
    public void TurnOnTarget() //Call in animation frame
    {
        this.obstacle.IsShowTrap = true;
    }

    public void TurnOffTarget() //Call in animation frame
    {
        this.obstacle.IsShowTrap = false;
    }
}
