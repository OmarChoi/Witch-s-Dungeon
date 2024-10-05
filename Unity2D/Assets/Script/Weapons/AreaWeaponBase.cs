using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class AreaWeaponBase : WeaponBase
{
    protected bool isActive = true;
    protected float cooldown = 0.1f;
    protected float duration = 5.0f;
    protected float spawnCycle = 2.0f;
    protected float delay = 0.0f;
    protected SpriteRenderer spriteRenderer;

    public override void Init()
    {
        base.Init();
        spriteRenderer = GetComponent<SpriteRenderer>();
        GetTargetInCircleArea();
        StartCoroutine(SetAttackCycle());
    }

    protected IEnumerator AddDamage()
    {
        while (isActive)
        {
            GetTargetInCircleArea();
            ApplyDamage();
            yield return new WaitForSeconds(cooldown);
        }
        yield break;
    }

    protected void SetEnable(bool enable)
    {
        if (enable)
        {
            spriteRenderer.enabled = true;
            isActive = true;
        }
        else
        {
            isActive = false;
            spriteRenderer.enabled = false;
        }
    }

    protected void ApplyDamage()
    {
        foreach (Collider2D monster in monstersInRange)
        {
            EnemyController controller = monster.transform.GetComponent<EnemyController>();
            if (!monster.transform.CompareTag("Monster")) continue;
            if (controller == null)
            {
                Debug.LogError($"AreaWeaponBase : EnemyController Doesn't Exist");
            }
            controller.GetDamage(Damage);
        }
    }

    protected IEnumerator SetAttackCycle()
    {
        while (true)
        {
            UpdatePosition();
            SetEnable(true);
            yield return new WaitForSeconds(delay);
            StartCoroutine(AddDamage());
            yield return new WaitForSeconds(duration);
            SetEnable(false);
            yield return new WaitForSeconds(spawnCycle);
        }
    }
}
