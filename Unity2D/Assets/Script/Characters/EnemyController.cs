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
        enemyType = transform.name;
        Managers.Data.enemyData.TryGetValue(enemyType, out status);
    }

    protected override void UpdateTransform()
    {
        UpdatePosition();
        UpdateAnimation();
    }

    public void UpdateTargetDirection()
    {
        Vector2 targetPos = targetObject.position;
        Vector2 position = rigidBody.position;
        moveDirection = (targetPos - position).normalized;
    }

    public void SpawnMonster(GameObject target, Vector2 pos, int id)
    {
        rigidBody.position = new Vector3(pos.x, pos.y, 0);
        targetObject = target.GetComponent<Rigidbody2D>();
    }

    private void UpdatePosition()
    {
        UpdateTargetDirection();
        base.UpdateTransform();
    }

    private void UpdateAnimation()
    {
        switch (state)
        {
            case Define.State.Move:
                break;
        }
    }
}
