using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class AreaWeaponBase : WeaponBase
{
    public override void Init()
    {
        GetTargetInCircleArea();
        StartCoroutine(AttackEnemy());
    }

    public override void UpdateTransform() { }

    protected void ApplyDamage()
    {
        GetTargetInCircleArea();
        foreach (Collider2D monster in monstersInRange)
        {
            EnemyController controller = monster.transform.GetComponent<EnemyController>();
            if (!monster.transform.CompareTag("Monster")) continue;
            controller.GetDamage(Damage);
        }
    }

    protected IEnumerator AttackEnemy()
    {
        float timeLeft = Duration;
        while (timeLeft >= 0.0f)
        {
            PlayAudio();
            ApplyDamage();
            yield return new WaitForSeconds(1 / AttackCycle);
            timeLeft -= (1 / AttackCycle);
        }
        Clear();
        yield break;
    }

    private void Clear()
    {
        Managers.Pool.ReleaseObject(weaponName, this.gameObject);
    }
}
