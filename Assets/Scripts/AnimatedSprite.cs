using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AnimatedSprite : MonoBehaviour
{
    public SpriteRenderer spriteRenderer{ get; private set; }
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private float animationTime= 0.25f;
    [SerializeField] private bool loop = true;
    public int animationFrame{ get; private set; }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        InvokeRepeating(nameof(Repeat), animationTime, animationTime);
    }

    private void Repeat()
    {
        if(!spriteRenderer.enabled)
        {
            return;
        }
        
        animationFrame++;

        if(animationFrame >= sprites.Length && loop)
        {
            animationFrame = 0;
        }

        if(animationFrame >= 0 && animationFrame < sprites.Length)
        {
            spriteRenderer.sprite = sprites[animationFrame];
        }
    }

    private void Restart()
    {
        animationFrame = -1;

        Repeat();
    }

}
