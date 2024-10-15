using UnityEngine;

public class Shuriken : RicochetWeapon
{
    public override void Init()
    {
        base.Init();
    }

    protected override void SetDirection()
    {
        base.SetDirection();
        GetTargetInCircleArea();
        Transform target = null;
        if (monstersInRange.Length > 0 )
        {
            int randomIndex = Random.Range(0, monstersInRange.Length - 1);
            target = monstersInRange[randomIndex].transform;
        }
        else
        {
            direction = UnityEngine.Random.insideUnitCircle.normalized;
        }
        CalculateDirection(target);
    }

    protected override void CalculateDirection(Transform target)
    {
        if (target != null)
        { 
            Vector2 targetPos = target.transform.position;
            direction = (targetPos - weaponPos).normalized;
        }
    }
}
