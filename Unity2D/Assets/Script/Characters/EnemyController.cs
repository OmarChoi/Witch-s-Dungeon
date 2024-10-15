using System.Collections;
using UnityEngine;

public class EnemyController : ControllerBase
{
    string enemyType = string.Empty;
    Rigidbody2D targetObject = null;
    private float lastAttackTime = 0;
    private float attackRange = 0.8f;

    public override void Init()
    {
        base.Init();
        enemyType = Utils.GetNameExceptClone(transform.name);
        Managers.Data.GetMonsterStatusByName(enemyType, out status);
        currentExp = status.Exp;
    }

    protected override void UpdateTransform()
    {
        UpdatePosition();
    }

    public void UpdateTargetDirection()
    {
        if (targetObject == null)
        {
            Debug.LogError($"{enemyType} doesn't have target");
        }
        Vector3 targetPos = targetObject.position;
        Vector3 position = rigidBody.position;
        moveDirection = (targetPos - position).normalized;
    }

    public void SpawnMonster(GameObject target, Vector2 pos)
    {
        targetObject = target.GetComponent<Rigidbody2D>();
        rigidBody.position = new Vector3(pos.x, pos.y, 0);
    }

    private void UpdatePosition()
    {
        UpdateTargetDirection();
        base.UpdateTransform();

        float execTime = ProgressTime - lastAttackTime;
        if (execTime > 1 / status.AttackSpeed)
        {
            Vector2 myPos = rigidBody.transform.position;
            Vector2 targetPos = Managers.Player.transform.position;
            float dist = (myPos - targetPos).magnitude;
            if (dist < attackRange)
            {
                Attack();
                lastAttackTime = ProgressTime;
            }
        }
    }

    protected override void Dead()
    {
        state = Define.State.Die;
        StopAllCoroutines();
        Managers.Player.GetComponent<PlayerController>().AddExp(currentExp);
        Managers.Pool.ReleaseObject(enemyType, this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Projectile") != true) return;
        ProjectileBase weapon = collision.GetComponent<ProjectileBase>();
        if (weapon == null) return;
        float damage = weapon.Damage;
        GetDamage(damage);

        RicochetWeapon ricochetFactor = collision.GetComponent<RicochetWeapon>();
        if (ricochetFactor ==  null) return;
        Collider2D collider = GetComponent<Collider2D>();
        ricochetFactor.Ricocheted(collider);
    }

    private void Attack()
    {
        Managers.Player.GetComponent<ControllerBase>().GetDamage(Damage);
    }
}
