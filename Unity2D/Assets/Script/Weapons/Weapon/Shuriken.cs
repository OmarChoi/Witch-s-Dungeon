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
        // �� �ȿ� �ִ� ���� �߿� �������� Setting �ǰ� ����
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
