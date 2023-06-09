using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDamageRemote : MonoBehaviour
{
    [SerializeField] private ObstacleDamage obstacle;
    private Animator animator;

    private void Awake()
    {
        this.animator = GetComponent<Animator>();

        this.animator.SetTrigger("Remote");
    }
    public void TurnOnTarget() //Call in animation Frame
    {
        this.obstacle.IsShowTrap = true;
    }

    public void TurnOffTarget() //Call in animation Frame
    {
        this.obstacle.IsShowTrap = false;
    }
}
