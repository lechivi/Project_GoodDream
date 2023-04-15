using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeverFlip : MonoBehaviour
{
    [SerializeField] private Transform target;
    public Transform Target { get => this.target; set => this.target = value; }

    private void FixedUpdate()
    {
        if (this.target != null)
        {
            transform.localScale = this.target.localScale == Vector3.one ? Vector3.one : new Vector3(-1, 1, 1);
        }
    }
}
