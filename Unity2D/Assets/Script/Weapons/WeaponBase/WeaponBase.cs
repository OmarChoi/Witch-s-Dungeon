using NUnit.Framework;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    protected float damage = 15.0f;
    protected float attackRange = 2.0f;
    protected Vector2 detectCenter = Vector2.zero;

    protected LayerMask targetLayer;
    protected Transform nearestTarget;
    protected Collider2D[] monstersInRange;

    public float Damage { get { return damage; } protected set { damage = value; } }

    private void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        targetLayer = (1 << (int)Define.Layer.Attackable);
    }

    protected void GetTargetInCircleArea()
    {
        monstersInRange = Physics2D.OverlapCircleAll(detectCenter, attackRange, targetLayer, 0.0f, 0.0f);
    }

    Transform GetNearestTarget()
    {
        Transform targetObject = null;
        float minDist = attackRange;

        foreach (Collider2D monster in monstersInRange)
        {
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
