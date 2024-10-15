using System.Collections;
using UnityEngine;

public class AreaWeaponBase : WeaponBase
{
    AudioSource weaponAudio = null;

    protected override void Awake()
    {
        base.Awake();
        weaponAudio = GetComponent<AudioSource>();
    }

    private void PlayEffectSound()
    {
        weaponAudio.Play();
    }

    public override void Init()
    {
        base.Init();
        weaponAudio = GetComponent<AudioSource>();
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
            ApplyDamage();
            PlayEffectSound();
            yield return new WaitForSeconds(1 / AttackCycle);
            timeLeft -= (1 / AttackCycle);
        }
        Clear();
        yield break;
    }
}
