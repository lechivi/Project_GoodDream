using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WipeTest : MonoBehaviour
{
    public float CircleSize;

    private Animator animtor;
    private SpriteRenderer spriteRenderer;
    private readonly int circleSizeId = Shader.PropertyToID(name: "_Circle_Size");

    private void Awake()
    {
        this.animtor = GetComponent<Animator>();
        this.spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        this.spriteRenderer.material.SetFloat(this.circleSizeId, this.CircleSize);
    }
}
