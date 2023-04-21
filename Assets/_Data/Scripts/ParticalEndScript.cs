using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticalEndScript : MonoBehaviour
{
    [SerializeField] private float timeLife = 2.5f;

    private void Start()
    {
        Destroy(gameObject, this.timeLife);
    }
}
