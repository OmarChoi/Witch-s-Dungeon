using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
using UnityEngine;
using UnityEngine.EventSystems;

public class ProjectileBase : WeaponBase
{
    float speed = 10.0f;
    protected int maxTarget = 5;
    protected int hitTarget = 0;
    protected Vector2 direction = Vector2.zero;

    public override void Init()
    {
        direction = Vector2.zero;
        hitTarget = 0;
        SetDirection();
    }

    public override void UpdateTransform()
    {
        Vector2 currentPos = transform.position;
        float displacement = (currentPos - weaponPos).magnitude;
        if(displacement > AttackRange || direction.magnitude < float.Epsilon || hitTarget > maxTarget)
        {
            Managers.Pool.ReleaseObject(weaponName, this.gameObject);
        }
        else
        {
            this.gameObject.transform.position = currentPos + direction * speed * Time.fixedDeltaTime;
        }
    }

    protected virtual void SetDirection() { }

    protected virtual void CalculateDirection(Transform target) { }

    public void Ricocheted(Collider2D hittedTarget)
    {
        hitTarget += 1;
        weaponPos = transform.position;
        GetTargetInCircleArea();
        Transform target = GetNearestTarget();
        CalculateDirection(target);
    }
}
