using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WipeCtrl : MonoBehaviour
{
    public float CircleSize;

    private Animator animtor;
    private Image image;
    private readonly int circleSizeId = Shader.PropertyToID(name: "_Circle_Size");

    private void Awake()
    {
        this.animtor = GetComponent<Animator>();
        this.image = GetComponent<Image>();
    }

    private void Update()
    {
        this.image.materialForRendering.SetFloat(this.circleSizeId, this.CircleSize);
    }
}
