using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class AreaWeaponBase : WeaponBase
{
    bool isAlive = true;
    float duration = 5.0f;
    float attackCycle = 2.0f;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        Init();
    }

    protected override void Init()
    {
        base.Init();
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        GetTargetInCircleArea();
        StartCoroutine(ContainAttack());
    }

    protected virtual void FixedUpdate()
    {
        if (isAlive)
        {
            GetTargetInCircleArea();
        }
    }

    IEnumerator AddDamage()
    {
        while (isAlive)
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
            yield return new WaitForSeconds(0.1f);
        }
        yield break;
    }

    IEnumerator ContainAttack()
    {
        while (true)
        {
            spriteRenderer.enabled = true;
            isAlive = true;
            StartCoroutine(AddDamage());
            yield return new WaitForSeconds(duration);

            isAlive = false;
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(attackCycle);
        }
    }

}
