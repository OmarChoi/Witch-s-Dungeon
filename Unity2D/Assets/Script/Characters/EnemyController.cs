using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class EnemyController : ControllerBase
{
    string enemyType = string.Empty;
    Rigidbody2D targetObject = null;

    public override void Init()
    {
        base.Init();
        enemyType = Utils.GetNameExceptClone(transform.name);
        Managers.Data.GetMonsterStatusByName(enemyType, out status);
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
    }

    protected override void Dead()
    {
        Managers.Player.GetComponent<PlayerController>().AddExp(currentExp);
        Managers.Pool.ReleaseObject(enemyType, this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Projectile") != true) return;
        ProjectileBase weapon = collision.GetComponent<ProjectileBase>();
        float damage = weapon.Damage;
        GetDamage(damage);
        Collider2D collider = this.GetComponent<Collider2D>();
        weapon.Ricocheted(collider);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") != true) return;
        // 공격 속도에 따라 공격
        // LastAttackTime에 따른 공격 설정
        collision.gameObject.GetComponent<ControllerBase>().GetDamage(Damage);
    }
}
