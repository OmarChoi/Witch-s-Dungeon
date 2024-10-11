using UnityEngine;

public class Bird : ProjectileBase
{
    SpriteRenderer spriteRenderer;
    public override void Init()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        base.Init();
    }

    protected override void SetDirection()
    {
        direction = UnityEngine.Random.insideUnitCircle.normalized;
        if(direction.x < 0)
            spriteRenderer.flipX = false;
        else
            spriteRenderer.flipX = true;

    }
}
