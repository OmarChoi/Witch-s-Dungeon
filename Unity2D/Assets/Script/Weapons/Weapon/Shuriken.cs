using UnityEngine;

public class Shuriken : RicochetWeapon
{
    public override void Init()
    {
        base.Init();
    }

    protected override void SetDirection()
    {
        GetTargetInCircleArea();
        Transform target = GetNearestTarget();
        CalculateDirection(target);
    }

    protected override void CalculateDirection(Transform target)
    {
        if (target == null)
        {
            Clear();
            return;
        }
        Vector2 targetPos = target.transform.position;
        direction = (targetPos - weaponPos).normalized;
    }
}
