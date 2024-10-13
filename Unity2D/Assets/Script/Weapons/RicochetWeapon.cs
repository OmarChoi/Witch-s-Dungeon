using System;
using UnityEngine;

public class RicochetWeapon : ProjectileBase
{
    const int maxTarget = 5;
    protected int hitTarget = 0;
    Transform[] alreadyHitTarget = new Transform[maxTarget];

    public override void Init()
    {
        base.Init();
        hitTarget = 0;
    }

    public void Ricocheted(Collider2D hittedTarget)
    {
        hitTarget += 1;
        if(hitTarget >= maxTarget)
        {
            Managers.Pool.ReleaseObject(weaponName, this.gameObject);
            return;
        }
        alreadyHitTarget[hitTarget] = hittedTarget.transform;
        weaponPos = transform.position;
        GetTargetInCircleArea();
        Transform target = GetNearestTarget();
        CalculateDirection(target);
    }

    protected override Transform GetNearestTarget()
    {
        Transform targetObject = null;
        float minDist = AttackRange;

        foreach (Collider2D monster in monstersInRange)
        {
            if (Array.Exists<Transform>(alreadyHitTarget, element => element == monster.transform)) continue;
            Vector3 playerPosition = transform.position;
            Vector3 targetPosition = monster.transform.position;
            float targetDist = Vector3.Distance(playerPosition, targetPosition);

            if (targetDist < minDist)
            {
                minDist = targetDist;
                targetObject = monster.transform;
            }
        }
        return targetObject;
    }
}
