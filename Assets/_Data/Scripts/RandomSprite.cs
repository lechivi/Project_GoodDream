using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSprite : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private List<Sprite> sprites = new List<Sprite>();

    private void Awake()
    {
        if (this.sprites.Count == 0 || this.spriteRenderer == null) return;
        this.spriteRenderer.sprite = this.sprites[Random.Range(0, this.sprites.Count)];
    }

}
