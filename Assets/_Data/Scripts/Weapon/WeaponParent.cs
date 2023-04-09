using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponParent : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public Animator animator;
    Vector3 direction;

    private void Update()
    {
        FaceGun();

        if (Input.GetMouseButtonDown(0))
        {
            this.animator.SetTrigger("Attack");
        }
    }

    private void FaceGun()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        this.direction = mousePos - (Vector2) transform.position;

        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, this.playerMovement.IsFacingRight ? rotZ : rotZ - 180);
    }
}
