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
        Transform target = null;
        if (monstersInRange.Length > 0 )
        {
            int randomIndex = Random.Range(0, monstersInRange.Length - 1);
            target = monstersInRange[randomIndex].transform;
        }
        // 원 안에 있는 몬스터 중에 랜덤으로 Setting 되게 설정
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
