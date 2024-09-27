using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class EnemyController : ControllerBase
{
    int monsterId = 0;
    string enemyType = string.Empty;
    Vector2 moveDirection = Vector2.zero;
    GameObject targetObject = null;

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
        Vector2 targetPos = targetObject.transform.position;
        moveDirection = (targetPos - position).normalized;
    }

    public void SpawnMonster(GameObject target, Vector2 pos, int id)
    {
        position = new Vector3(pos.x, pos.y, 0);
        transform.position = position;
        targetObject = target;
    }

    private void UpdatePosition()
    {
        UpdateTargetDirection();
        Vector2 nextPos = position + moveDirection * (Speed * 0.1f) * Time.deltaTime;
        Vector2 targetPos = targetObject.transform.position;

        float dist = Vector2.Distance(nextPos, targetPos);
        if (dist > 1.0f)
        {
            position = nextPos;
            transform.position = position;
        }
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
