using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
using UnityEngine;
using UnityEngine.EventSystems;

public class ProjectileBase : WeaponBase
{
    string projectileType;
    bool hasTarget = false;
    float speed = 1.0f;
    float arrange = 3.0f;
    Vector2 direction = Vector2.zero;
    Vector2 startPos = Vector2.zero;

    public override void Init()
    {
        base.Init();
        projectileType = transform.name.Replace("(Clone)", "").Trim();
    }

    protected override void UpdatePosition()
    {
        UpdateDirection();
        Vector2 currentPos = transform.position;
        float displacement = (currentPos - startPos).magnitude;
        if(displacement < arrange)
        {
            transform.Translate(direction * speed * Time.fixedDeltaTime, Space.World);
        }
        else
        {
            Managers.Pool.ReleaseObject(projectileType, this.gameObject);
        }
    }

    protected virtual void UpdateDirection() { }

    protected virtual void SetDirection() { } 

    private void Spawn(Vector2 startPosition)
    {
        SetDirection();
    }
}
