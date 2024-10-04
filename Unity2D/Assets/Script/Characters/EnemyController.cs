using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class EnemyController : ControllerBase
{
    int mid = -1;
    string enemyType = string.Empty;
    Rigidbody2D targetObject = null;

    public override void Init()
    {
        base.Init();
        enemyType = transform.name.Replace("(Clone)", "").Trim();
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
        Vector2 targetPos = targetObject.position;
        Vector2 position = rigidBody.position;
        moveDirection = (targetPos - position).normalized;
    }

    public void SpawnMonster(GameObject target, Vector2 pos, int id)
    {
        mid = id;
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
        this.gameObject.SetActive(false);
        Managers.Pool.ReleaseObject(enemyType, this.gameObject);
    }
}
